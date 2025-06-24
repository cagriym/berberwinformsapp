using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace berberdenemeson
{
    public class ConnectionManager
    {
        private static readonly Lazy<ConnectionManager> _instance = new Lazy<ConnectionManager>(() => new ConnectionManager());
        public static ConnectionManager Instance => _instance.Value;

        private readonly HttpClient _httpClient;
        private readonly string _offlineDataFolder;
        private readonly string _pendingOperationsFile;
        private bool _isOnline = false;
        private Timer _connectionCheckTimer;
        private Timer _monitoringTimer;

        public event EventHandler<bool> ConnectionStatusChanged;

        public bool IsOnline => _isOnline;

        private ConnectionManager()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);

            // Offline veri klasörünü oluştur
            _offlineDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BerberRandevu", "OfflineData");
            Directory.CreateDirectory(_offlineDataFolder);

            _pendingOperationsFile = Path.Combine(_offlineDataFolder, "pendingOperations.json");

            // Bağlantı kontrolü için timer
            _connectionCheckTimer = new Timer();
            _connectionCheckTimer.Interval = 30000; // 30 saniye
            _connectionCheckTimer.Tick += (s, e) => CheckConnectionAsync();
            _connectionCheckTimer.Start();

            // İlk bağlantı kontrolü
            CheckConnectionAsync();
        }

        public bool CheckConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    Console.WriteLine($"DEBUG: ConnectionManager - Bağlantı kontrolü yapılıyor: https://oktay-sac-tasarim1.azurewebsites.net/api/Needs");
                    var response = client.GetAsync("https://oktay-sac-tasarim1.azurewebsites.net/api/Needs").Result;
                    Console.WriteLine($"DEBUG: ConnectionManager - HTTP Response Status: {response.StatusCode}");
                    var wasOnline = _isOnline;
                    _isOnline = response.IsSuccessStatusCode;
                    Console.WriteLine($"DEBUG: ConnectionManager - Bağlantı durumu: {_isOnline}");

                    if (wasOnline != _isOnline)
                    {
                        Console.WriteLine($"DEBUG: ConnectionManager.CheckConnectionAsync - ConnectionStatusChanged event tetikleniyor: wasOnline={wasOnline}, _isOnline={_isOnline}");
                        ConnectionStatusChanged?.Invoke(this, _isOnline);
                        
                        if (_isOnline)
                        {
                            // Online olduğunda bekleyen işlemleri senkronize et
                            Task.Run(() => SyncPendingOperationsAsync());
                        }
                    }

                    return _isOnline;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: ConnectionManager - Bağlantı kontrolü hatası: {ex.Message}");
                var wasOnline = _isOnline;
                _isOnline = false;
                
                if (wasOnline != _isOnline)
                {
                    ConnectionStatusChanged?.Invoke(this, _isOnline);
                }
                
                return false;
            }
        }

        public async Task<T> GetDataAsync<T>(string endpoint, Func<Task<T>> onlineOperation, Func<T> offlineOperation = null)
        {
            Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Endpoint: {endpoint}, _isOnline: {_isOnline}");
            
            if (_isOnline)
            {
                try
                {
                    Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Online operation başlatılıyor: {endpoint}");
                    var result = await onlineOperation();
                    Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Online operation başarılı: {endpoint}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Online operation hatası: {endpoint}, Hata: {ex.Message}");
                    
                    // Bağlantı durumunu tekrar kontrol et
                    var currentConnectionStatus = CheckConnectionAsync();
                    Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Yeniden bağlantı kontrolü: {currentConnectionStatus}");
                    
                    // Eğer bağlantı hala varsa, sadece bu endpoint için offline moda geç
                    if (currentConnectionStatus)
                    {
                        Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Bağlantı hala mevcut, sadece {endpoint} için offline moda geçiliyor");
                    }
                    else
                    {
                        // Gerçekten bağlantı yoksa _isOnline'ı false yap
                        var wasOnline = _isOnline;
                        _isOnline = false;
                        Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - _isOnline false yapıldı: {endpoint}");
                        
                        if (wasOnline != _isOnline)
                        {
                            Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - ConnectionStatusChanged event tetikleniyor: {endpoint}");
                            ConnectionStatusChanged?.Invoke(this, _isOnline);
                        }
                    }
                }
            }

            Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Offline moda geçiliyor: {endpoint}");
            
            // Offline modda çalış
            if (offlineOperation != null)
            {
                Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Offline operation kullanılıyor: {endpoint}");
                return offlineOperation();
            }

            // Offline veri varsa onu kullan
            var offlineData = LoadOfflineDataAsync<T>(endpoint);
            if (offlineData != null)
            {
                Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Offline data bulundu: {endpoint}");
                return offlineData;
            }

            Console.WriteLine($"DEBUG: ConnectionManager.GetDataAsync - Boş veri döndürülüyor: {endpoint}");
            
            // Offline veri yoksa, tipine göre boş liste veya default değer döndür
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            return default(T);
        }

        public async Task<bool> SaveDataAsync<T>(string endpoint, T data, Func<Task<bool>> onlineOperation)
        {
            if (_isOnline)
            {
                try
                {
                    var result = await onlineOperation();
                    if (result)
                    {
                        // Online başarılı olduğunda offline veriyi güncelle
                        SaveOfflineDataAsync(endpoint, data);
                        return true;
                    }
                }
                catch
                {
                    _isOnline = false;
                    ConnectionStatusChanged?.Invoke(this, _isOnline);
                }
            }

            // Offline modda bekleyen işlemler listesine ekle
            AddPendingOperationAsync(endpoint, data);
            
            // Offline veriyi güncelle
            SaveOfflineDataAsync(endpoint, data);
            
            return true; // Offline modda başarılı kabul et
        }

        private T LoadOfflineDataAsync<T>(string endpoint)
        {
            try
            {
                var fileName = $"{endpoint.Replace("/", "_")}.json";
                var filePath = Path.Combine(_offlineDataFolder, fileName);
                
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Offline veri okuma hatası: {ex.Message}");
            }
            
            return default(T);
        }

        private void SaveOfflineDataAsync<T>(string endpoint, T data)
        {
            try
            {
                var fileName = $"{endpoint.Replace("/", "_")}.json";
                var filePath = Path.Combine(_offlineDataFolder, fileName);
                
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Offline veri yazma hatası: {ex.Message}");
            }
        }

        private void AddPendingOperationAsync<T>(string endpoint, T data)
        {
            try
            {
                var operations = LoadPendingOperationsAsync();
                
                operations.Add(new PendingOperation
                {
                    Id = Guid.NewGuid().ToString(),
                    Endpoint = endpoint,
                    Data = JsonConvert.SerializeObject(data),
                    Timestamp = DateTime.Now
                });

                SavePendingOperationsAsync(operations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bekleyen işlem ekleme hatası: {ex.Message}");
            }
        }

        public async Task SyncPendingOperationsAsync()
        {
            try
            {
                var operations = LoadPendingOperationsAsync();
                var successfulOperations = new List<PendingOperation>();

                foreach (var operation in operations)
                {
                    try
                    {
                        // API'ye gönder
                        var content = new StringContent(operation.Data, System.Text.Encoding.UTF8, "application/json");
                        var response = await _httpClient.PostAsync($"https://oktay-sac-tasarim1.azurewebsites.net/api/{operation.Endpoint}", content);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            successfulOperations.Add(operation);
                        }
                    }
                    catch
                    {
                        // Başarısız olan işlemler listede kalır
                    }
                }

                // Başarılı olan işlemleri listeden çıkar
                operations.RemoveAll(op => successfulOperations.Contains(op));
                SavePendingOperationsAsync(operations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Senkronizasyon hatası: {ex.Message}");
            }
        }

        private List<PendingOperation> LoadPendingOperationsAsync()
        {
            try
            {
                if (File.Exists(_pendingOperationsFile))
                {
                    var json = File.ReadAllText(_pendingOperationsFile);
                    return JsonConvert.DeserializeObject<List<PendingOperation>>(json) ?? new List<PendingOperation>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bekleyen işlemler okuma hatası: {ex.Message}");
            }
            
            return new List<PendingOperation>();
        }

        private void SavePendingOperationsAsync(List<PendingOperation> operations)
        {
            try
            {
                var json = JsonConvert.SerializeObject(operations, Formatting.Indented);
                File.WriteAllText(_pendingOperationsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bekleyen işlemler yazma hatası: {ex.Message}");
            }
        }

        public void Dispose()
        {
            try
            {
                // Timer'ları durdur
                if (_connectionCheckTimer != null)
                {
                    _connectionCheckTimer.Stop();
                    _connectionCheckTimer.Dispose();
                    _connectionCheckTimer = null;
                }

                if (_monitoringTimer != null)
                {
                    _monitoringTimer.Stop();
                    _monitoringTimer.Dispose();
                    _monitoringTimer = null;
                }

                // HttpClient'ı dispose et
                _httpClient?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConnectionManager dispose hatası: {ex.Message}");
            }
        }

        private void CheckConnectionCallback(object state)
        {
            CheckConnectionAsync();
        }

        public void StartConnectionMonitoring()
        {
            _monitoringTimer = new Timer();
            _monitoringTimer.Interval = 30000; // 30 saniye
            _monitoringTimer.Tick += (s, e) => CheckConnectionAsync();
            _monitoringTimer.Start();
        }
    }

    public class PendingOperation
    {
        public string Id { get; set; }
        public string Endpoint { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 