using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Timers;

namespace berberdenemeson
{
    public class ApiService : IDisposable
    {
        private static readonly Lazy<ApiService> _instance = new Lazy<ApiService>(() => new ApiService());
        public static ApiService Instance 
        { 
            get { return _instance.Value; } 
        }
        
        // Hata bildirimi için event
        public event EventHandler<string> ErrorOccurred;
        public event EventHandler<string> StatusChanged;
        public event EventHandler<string> ServiceStatusChanged; // Yeni event: Servis durumu değişiklikleri için
        
        private readonly HttpClient _httpClient;
        private bool _disposed = false;
        private const string BaseUrl = "https://oktay-sac-tasarim1.azurewebsites.net/api/"; // Uzak sunucu - sonda / eklendi
        private readonly string _cacheFolder;
        
        // Offline queue sistemi
        private readonly string _offlineQueueFile;
        private List<OfflineOperation> _offlineQueue;
        private bool _isOnline = false;
        private Timer _syncTimer;
        private readonly ConnectionManager _connectionManager;

        private ApiService()
        {
            // Retry policy için handler ekle
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
            
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl); // BaseAddress ayarla
            _httpClient.Timeout = TimeSpan.FromMinutes(5); // Timeout'u 5 dakikaya çıkar
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BerberYonetimSistemi/1.0");

            // Cache klasörünü oluştur
            _cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BerberRandevu", "Cache");
            Directory.CreateDirectory(_cacheFolder);

            // Offline queue dosyası
            _offlineQueueFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BerberRandevu", "offline_queue.json");
            _offlineQueue = new List<OfflineOperation>();
            LoadOfflineQueue();

            // ConnectionManager'ı başlat
            _connectionManager = ConnectionManager.Instance;
            _connectionManager.ConnectionStatusChanged += OnConnectionStatusChanged;
            
            // Bağlantı durumunu kontrol et ve _isOnline'ı güncelle
            _isOnline = _connectionManager.CheckConnectionAsync();
            Console.WriteLine($"DEBUG: ApiService - Başlangıçta _isOnline: {_isOnline}");

            // Sync timer'ı başlat
            StartSyncTimer();
        }

        private void OnConnectionStatusChanged(object sender, bool isOnline)
        {
            Console.WriteLine($"DEBUG: ApiService.OnConnectionStatusChanged - Event tetiklendi: sender={sender}, isOnline={isOnline}, mevcut _isOnline={_isOnline}");
            
            var wasOnline = _isOnline;
            _isOnline = isOnline;
            
            var status = isOnline ? "Çevrimiçi" : "Çevrimdışı";
            StatusChanged?.Invoke(this, $"Bağlantı durumu: {status}");
            Console.WriteLine($"DEBUG: ApiService.OnConnectionStatusChanged - _isOnline güncellendi: {_isOnline}, Bağlantı durumu değişti: {status}");
            
            // Eğer çevrimdışıdan çevrimiçiye geçiş yapıldıysa cache'i temizle
            if (!wasOnline && isOnline)
            {
                Console.WriteLine("DEBUG: ApiService.OnConnectionStatusChanged - Çevrimiçi moda geçildi, cache temizleniyor...");
                ClearAllCache();
                ServiceStatusChanged?.Invoke(this, "Bağlantı yeniden kuruldu. Veriler güncelleniyor...");
            }
        }

        private void ClearAllCache()
        {
            try
            {
                if (Directory.Exists(_cacheFolder))
                {
                    var cacheFiles = Directory.GetFiles(_cacheFolder, "*.json");
                    foreach (var file in cacheFiles)
                    {
                        File.Delete(file);
                    }
                    Console.WriteLine($"DEBUG: ApiService.ClearAllCache - {cacheFiles.Length} cache dosyası silindi");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: ApiService.ClearAllCache - Cache temizleme hatası: {ex.Message}");
            }
        }

        private void SyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ = SyncOfflineOperationsAsync();
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ApiService));
            }
        }

        // Hata bildirimi için yardımcı metod
        private void ReportError(string error)
        {
            ErrorOccurred?.Invoke(this, error);
            Console.WriteLine($"API Hatası: {error}");
        }

        // Durum bildirimi için yardımcı metod
        private void ReportStatus(string status)
        {
            StatusChanged?.Invoke(this, status);
            Console.WriteLine($"API Durumu: {status}");
        }

        private async Task<T> GetDataWithCacheAsync<T>(string endpoint)
        {
            try
            {
                ReportStatus($"{endpoint} verisi yükleniyor...");
                return await _connectionManager.GetDataAsync(endpoint, 
                    async () => await GetAsync<T>(endpoint),
                    () => GetFromCacheAsync<T>(endpoint));
            }
            catch (Exception ex)
            {
                ReportError($"{endpoint} verisi yüklenirken hata: {ex.Message}");
                throw;
            }
        }

        private T GetFromCacheAsync<T>(string endpoint)
        {
            try
            {
                string cacheKey = endpoint.Replace("/", "_").Replace("?", "_").Replace("=", "_").Replace("&", "_").Replace(":", "_").Replace(".", "_");
                string cacheFilePath = Path.Combine(_cacheFolder, cacheKey + ".json");

                if (File.Exists(cacheFilePath))
                {
                    var cachedJson = File.ReadAllText(cacheFilePath);
                    var result = JsonConvert.DeserializeObject<T>(cachedJson);
                    ReportStatus($"{endpoint} verisi cache'den yüklendi");
                    return result;
                }
            }
            catch (Exception ex)
            {
                ReportError($"Cache okuma hatası: {ex.Message}");
            }
            
            // Cache yoksa, tipine göre boş liste veya default değer döndür
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            return default(T);
        }

        private void SaveToCacheAsync<T>(string endpoint, T data)
        {
            try
            {
                string cacheKey = endpoint.Replace("/", "_").Replace("?", "_").Replace("=", "_").Replace("&", "_").Replace(":", "_").Replace(".", "_");
                string cacheFilePath = Path.Combine(_cacheFolder, cacheKey + ".json");
                
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(cacheFilePath, json);
                ReportStatus($"{endpoint} verisi cache'e kaydedildi");
            }
            catch (Exception ex)
            {
                ReportError($"Cache yazma hatası: {ex.Message}");
            }
        }

        private void ClearCacheForEndpoint(string endpoint)
        {
            try
            {
                string cacheKey = endpoint.Replace("/", "_").Replace("?", "_").Replace("=", "_").Replace("&", "_").Replace(":", "_").Replace(".", "_");
                string cacheFilePath = Path.Combine(_cacheFolder, cacheKey + ".json");
                
                if (File.Exists(cacheFilePath))
                {
                    File.Delete(cacheFilePath);
                    ReportStatus($"{endpoint} cache'i temizlendi");
                }
            }
            catch (Exception ex)
            {
                ReportError($"Cache temizleme hatası: {ex.Message}");
            }
        }

        // --- MERKEZİ MODEL TANIMLARI ---

        public class AppointmentModel
        {
            [JsonProperty("randevuID")]
            public int RandevuID { get; set; }
            [JsonProperty("musteriID")]
            public int MusteriID { get; set; }
            [JsonProperty("musteriAdi")]
            public string MusteriAdi { get; set; }
            [JsonProperty("musteriTelefon")]
            public string MusteriTelefon { get; set; }
            [JsonProperty("randevuZamani")]
            public DateTime RandevuZamani { get; set; }
            [JsonProperty("servisID")]
            public int ServisID { get; set; }
            [JsonProperty("servisAd")]
            public string ServisAd { get; set; }
            [JsonProperty("aciklama")]
            public string Aciklama { get; set; }
            [JsonProperty("ucret")]
            public decimal Ucret { get; set; } // Nullable olmayan decimal yapıldı (API'den geliyorsa)
            [JsonProperty("tamamlandiMi")]
            public bool TamamlandiMi { get; set; }
            [JsonProperty("createdAt")]
            public DateTime? CreatedAt { get; set; }
            [JsonProperty("personelID")]
            public int? PersonelID { get; set; }
            [JsonProperty("personelAdSoyad")]
            public string PersonelAdSoyad { get; set; }
            [JsonProperty("musteri")]
            public CustomerModel musteri { get; set; }
            [JsonProperty("servisler")]
            public List<ServiceModel> Servisler { get; set; } // Çoklu servis desteği
            [JsonProperty("randevuServisler")]
            public List<RandevuServisModel> RandevuServisler { get; set; } // API'den gelen randevu servisleri
        }

        public class RandevuServisModel
        {
            [JsonProperty("randevuServisID")]
            public int RandevuServisID { get; set; }
            [JsonProperty("randevuID")]
            public int RandevuID { get; set; }
            [JsonProperty("servisID")]
            public int ServisID { get; set; }
            [JsonProperty("servisAdi")]
            public string ServisAdi { get; set; }
            [JsonProperty("varsayilanUcret")]
            public decimal VarsayilanUcret { get; set; }
            [JsonProperty("servis")]
            public ServiceModel Servis { get; set; }
        }

        public class CustomerModel
        {
            [JsonProperty("musteriID")]
            public int MusteriID { get; set; }
            [JsonProperty("adSoyad")]
            public string AdSoyad { get; set; }
            [JsonProperty("telefon")]
            public string Telefon { get; set; }
            [JsonProperty("createdAt")]
            public DateTime? CreatedAt { get; set; }
            
            // UI için computed property'ler
            public string Ad => AdSoyad?.Split(' ').FirstOrDefault() ?? "";
            public string Soyad => AdSoyad?.Split(' ').Skip(1).FirstOrDefault() ?? "";
            public DateTime? SonGelisTarihi => CreatedAt; // Varsayılan olarak kayıt tarihi
        }

        public class NeedModel
        {
            [JsonProperty("ihtiyacID")]
            public int IhtiyacID { get; set; }
            [JsonProperty("ihtiyacTuru")]
            public string IhtiyacTuru { get; set; }
            [JsonProperty("fiyat")]
            public decimal Fiyat { get; set; }
            [JsonProperty("aciklama")]
            public string Aciklama { get; set; }
            [JsonProperty("createdAt")]
            public DateTime? CreatedAt { get; set; }
            
            // UI için computed property'ler
            public string Ad => IhtiyacTuru;
            public int Miktar => 1; // Varsayılan değer
            public decimal BirimFiyat => Fiyat;
            public decimal ToplamFiyat => Fiyat;
        }


        public class PersonnelModel
        {
            [JsonProperty("personelID")]
            public int? PersonelID { get; set; }
            [JsonProperty("adSoyad")]
            public string AdSoyad { get; set; }
            [JsonProperty("pozisyon")]
            public string Pozisyon { get; set; }
            [JsonProperty("telefon")]
            public string Telefon { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("maas")]
            public decimal? Maas { get; set; }
            [JsonProperty("aktif")]
            public bool? Aktif { get; set; }
            [JsonProperty("iseGirisTarihi")]
            public DateTime? IseGirisTarihi { get; set; }
            [JsonProperty("cikisTarihi")]
            public DateTime? CikisTarihi { get; set; }
            [JsonProperty("aciklama")]
            public string Aciklama { get; set; }
            
            // UI için computed property'ler
            public string Ad => AdSoyad?.Split(' ').FirstOrDefault() ?? "";
            public string Soyad => AdSoyad?.Split(' ').Skip(1).FirstOrDefault() ?? "";

            public PersonnelModel()
            {
                AdSoyad = string.Empty;
                Telefon = string.Empty;
                Pozisyon = string.Empty;
                Email = string.Empty;
                Aktif = true;
                Aciklama = string.Empty;
            }
        }

        public class ServiceModel
        {
            [JsonProperty("servisID")]
            public int ServisID { get; set; }
            [JsonProperty("servisAdi")]
            public string ServisAdi { get; set; }
            [JsonProperty("varsayilanUcret")]
            public decimal? VarsayilanUcret { get; set; }
        }

        public class ContactMessageDto
        {
            [JsonProperty("id")]
            public int Id { get; set; } // Swagger'da Id var
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("createdAt")]
            public DateTime CreatedAt { get; set; }
        }

        public class PersonnelStats
        {
            [JsonProperty("total")]
            public int? Total { get; set; }

            [JsonProperty("aktif")]
            public int? Aktif { get; set; }

            [JsonProperty("pasif")]
            public int? Pasif { get; set; }

            [JsonProperty("ortalamaMaas")]
            public decimal? OrtalamaMaas { get; set; }
        }

        // --- GENEL API METODLARI ---

        private async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                // Debug için tam URL'yi logla
                var requestUri = new Uri(_httpClient.BaseAddress, endpoint);
                Console.WriteLine($"DEBUG: BaseAddress: {_httpClient.BaseAddress}");
                Console.WriteLine($"DEBUG: Endpoint: {endpoint}");
                Console.WriteLine($"DEBUG: İstek gönderilen URL: {requestUri.AbsoluteUri}");
                Console.WriteLine($"DEBUG: _isOnline: {_isOnline}");
                ReportStatus($"API isteği gönderiliyor: {requestUri.AbsoluteUri}");

                if (_isOnline)
                {
                    ReportStatus($"{endpoint} API'den yükleniyor...");
                    string cacheKey = endpoint.Replace("/", "_").Replace("?", "_").Replace("=", "_").Replace("&", "_").Replace(":", "_").Replace(".", "_");
                    string cacheFilePath = Path.Combine(_cacheFolder, cacheKey + ".json");

                    try
                    {
                        Console.WriteLine($"DEBUG: HTTP isteği gönderiliyor...");
                        var response = await _httpClient.GetAsync(requestUri);
                        Console.WriteLine($"DEBUG: HTTP Response Status: {response.StatusCode}");
                        Console.WriteLine($"DEBUG: HTTP Response IsSuccessStatusCode: {response.IsSuccessStatusCode}");
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"DEBUG: Response JSON length: {responseJson?.Length ?? 0}");
                            Console.WriteLine($"DEBUG: Response JSON preview: {responseJson?.Substring(0, Math.Min(200, responseJson?.Length ?? 0))}");
                            
                            // Randevu verileri için özel debug
                            if (endpoint == "Appointments" && responseJson?.Length > 0)
                            {
                                Console.WriteLine($"DEBUG: Full Appointments JSON: {responseJson}");
                            }
                            
                            var result = JsonConvert.DeserializeObject<T>(responseJson);
                            Console.WriteLine($"DEBUG: Deserialized result type: {result?.GetType().Name}");
                            
                            // Cache'e kaydet
                            SaveToCacheAsync(endpoint, result);
                            ReportStatus($"{endpoint} API'den başarıyla yüklendi");
                            return result;
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"DEBUG: HTTP Error Response: {errorContent}");
                            
                            // 502 Bad Gateway için özel mesaj
                            if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                            {
                                ReportError($"{endpoint} API hatası: Sunucu geçici olarak kullanılamıyor (502 Bad Gateway). Cache'den veri yükleniyor.");
                            }
                            else
                            {
                                ReportError($"{endpoint} API hatası: {response.StatusCode} - {errorContent}");
                            }
                            
                            // Cache'den yükle
                            var cachedResult = GetFromCacheAsync<T>(endpoint);
                            if (cachedResult != null)
                            {
                                ReportStatus($"{endpoint} cache'den yüklendi (API hatası nedeniyle)");
                                return cachedResult;
                            }
                            throw new HttpRequestException($"HTTP {response.StatusCode}: {errorContent}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DEBUG: HTTP isteği hatası: {ex.Message}");
                        ReportError($"{endpoint} HTTP hatası: {ex.Message}");
                        
                        // Cache'den yükle
                        var cachedResult = GetFromCacheAsync<T>(endpoint);
                        if (cachedResult != null)
                        {
                            ReportStatus($"{endpoint} cache'den yüklendi (HTTP hatası nedeniyle)");
                            return cachedResult;
                        }
                        throw;
                    }
                }
                else
                {
                    ReportStatus($"{endpoint} çevrimdışı modda cache'den yükleniyor...");
                    var cachedResult = GetFromCacheAsync<T>(endpoint);
                    if (cachedResult != null)
                    {
                        ReportStatus($"{endpoint} cache'den yüklendi");
                        return cachedResult;
                    }
                    else
                    {
                        ReportStatus($"{endpoint} için cache bulunamadı, boş veri döndürülüyor");
                        // Cache yoksa, tipine göre boş liste veya default değer döndür
                        if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                        {
                            return (T)Activator.CreateInstance(typeof(T));
                        }
                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError($"{endpoint} genel hata: {ex.Message}");
                throw;
            }
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                if (_isOnline)
                {
                    var requestUri = new Uri(_httpClient.BaseAddress, endpoint);
                    var response = await _httpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(responseJson);
                }
                else
                {
                    _offlineQueue.Add(new OfflineOperation
                    {
                        Id = Guid.NewGuid().ToString(),
                        Operation = "POST",
                        Endpoint = endpoint,
                        Data = json,
                        Timestamp = DateTime.UtcNow
                    });
                    SaveOfflineQueue();
                    return default(TResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PostAsync metodu hatası ({endpoint}): {ex.Message}");
                throw;
            }
        }

        private async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                if (_isOnline)
                {
                    var requestUri = new Uri(_httpClient.BaseAddress, endpoint);
                    var response = await _httpClient.PutAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(responseJson);
                }
                else
                {
                    _offlineQueue.Add(new OfflineOperation
                    {
                        Id = Guid.NewGuid().ToString(),
                        Operation = "PUT",
                        Endpoint = endpoint,
                        Data = json,
                        Timestamp = DateTime.UtcNow
                    });
                    SaveOfflineQueue();
                    return default(TResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PutAsync metodu hatası ({endpoint}): {ex.Message}");
                throw;
            }
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                if (_isOnline)
                {
                    var requestUri = new Uri(_httpClient.BaseAddress, endpoint);
                    var response = await _httpClient.DeleteAsync(requestUri);
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    _offlineQueue.Add(new OfflineOperation
                    {
                        Id = Guid.NewGuid().ToString(),
                        Operation = "DELETE",
                        Endpoint = endpoint,
                        Data = "", // Delete için genellikle veri olmaz
                        Timestamp = DateTime.UtcNow
                    });
                    SaveOfflineQueue();
                    return true; // Offline'da başarılı kabul et
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteAsync metodu hatası ({endpoint}): {ex.Message}");
                throw;
            }
        }

        // --- SPECIFIC API CALLS ---

        public async Task<List<AppointmentModel>> GetAppointmentsAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "Appointments",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<AppointmentModel>>("Appointments");
                    return result ?? new List<AppointmentModel>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<AppointmentModel>>("Appointments");
                    return cachedData ?? new List<AppointmentModel>();
                }
            );
        }

        public async Task<AppointmentModel> GetAppointmentByIdAsync(int id)
        {
            return await GetAsync<AppointmentModel>($"Appointments/{id}");
        }

        public async Task<AppointmentModel> AddAppointmentAsync(AppointmentModel appointment)
        {
            return await PostAsync<AppointmentModel, AppointmentModel>("Appointments", appointment);
        }

        public async Task<AppointmentModel> UpdateAppointmentAsync(int id, AppointmentModel appointment)
        {
            return await PutAsync<AppointmentModel, AppointmentModel>($"Appointments/{id}", appointment);
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            return await DeleteAsync($"Appointments/{id}");
        }

        // Customer API Calls
        public async Task<List<CustomerModel>> GetCustomersAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "Musteriler",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<CustomerModel>>("Musteriler");
                    return result ?? new List<CustomerModel>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<CustomerModel>>("Musteriler");
                    return cachedData ?? new List<CustomerModel>();
                }
            );
        }

        public async Task<CustomerModel> GetCustomerByIdAsync(int id)
        {
            return await GetAsync<CustomerModel>($"Musteriler/{id}");
        }

        public async Task<CustomerModel> CreateCustomerAsync(CustomerModel customer)
        {
            CheckDisposed();
            return await PostAsync<CustomerModel, CustomerModel>("Musteriler", customer);
        }

        public async Task<CustomerModel> AddCustomerAsync(CustomerModel customer)
        {
            CheckDisposed();
            return await PostAsync<CustomerModel, CustomerModel>("Musteriler", customer);
        }

        public async Task<CustomerModel> UpdateCustomerAsync(int id, CustomerModel customer)
        {
            return await PutAsync<CustomerModel, CustomerModel>($"Musteriler/{id}", customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            return await DeleteAsync($"Musteriler/{id}");
        }

        // Need API Calls
        public async Task<List<NeedModel>> GetNeedsAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "Needs",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<NeedModel>>("Needs");
                    return result ?? new List<NeedModel>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<NeedModel>>("Needs");
                    return cachedData ?? new List<NeedModel>();
                }
            );
        }

        public async Task<NeedModel> GetNeedByIdAsync(int id)
        {
            return await GetAsync<NeedModel>($"Needs/{id}");
        }

        public async Task<NeedModel> AddNeedAsync(NeedModel need)
        {
            return await PostAsync<NeedModel, NeedModel>("Needs", need);
        }

        public async Task<NeedModel> UpdateNeedAsync(int id, NeedModel need)
        {
            return await PutAsync<NeedModel, NeedModel>($"Needs/{id}", need);
        }

        public async Task<bool> DeleteNeedAsync(int id)
        {
            return await DeleteAsync($"Needs/{id}");
        }

        public async Task<NeedModel> CreateNeedAsync(NeedModel need)
        {
            CheckDisposed();
            return await PostAsync<NeedModel, NeedModel>("Needs", need);
        }

        // Personnel API Calls
        public async Task<List<PersonnelModel>> GetPersonnelAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "Personel",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<PersonnelModel>>("Personel");
                    return result ?? new List<PersonnelModel>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<PersonnelModel>>("Personel");
                    return cachedData ?? new List<PersonnelModel>();
                }
            );
        }

        public async Task<PersonnelModel> GetPersonnelByIdAsync(int id)
        {
            return await GetAsync<PersonnelModel>($"Personel/{id}");
        }

        public async Task<PersonnelModel> AddPersonnelAsync(PersonnelModel personnel)
        {
            return await PostAsync<PersonnelModel, PersonnelModel>("Personel", personnel);
        }

        public async Task<PersonnelModel> UpdatePersonnelAsync(int id, PersonnelModel personnel)
        {
            return await PutAsync<PersonnelModel, PersonnelModel>($"Personel/{id}", personnel);
        }

        public async Task<bool> DeletePersonnelAsync(int id)
        {
            return await DeleteAsync($"Personel/{id}");
        }

        // Service API Calls
        public async Task<List<ServiceModel>> GetServicesAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "Services",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<ServiceModel>>("Services");
                    return result ?? new List<ServiceModel>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<ServiceModel>>("Services");
                    return cachedData ?? new List<ServiceModel>();
                }
            );
        }

        public async Task<ServiceModel> GetServiceByIdAsync(int id)
        {
            return await GetAsync<ServiceModel>($"Services/{id}");
        }

        public async Task<ServiceModel> AddServiceAsync(ServiceModel service)
        {
            return await PostAsync<ServiceModel, ServiceModel>("Services", service);
        }

        public async Task<ServiceModel> UpdateServiceAsync(int id, ServiceModel service)
        {
            return await PutAsync<ServiceModel, ServiceModel>($"Services/{id}", service);
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            return await DeleteAsync($"Services/{id}");
        }

        // Contact Messages API Calls
        public async Task<List<ContactMessageDto>> GetContactMessagesAsync()
        {
            CheckDisposed();
            return await _connectionManager.GetDataAsync(
                "ContactMessages",
                async () =>
                {
                    var result = await GetDataWithCacheAsync<List<ContactMessageDto>>("ContactMessages");
                    return result ?? new List<ContactMessageDto>();
                },
                () =>
                {
                    var cachedData = GetFromCacheAsync<List<ContactMessageDto>>("ContactMessages");
                    return cachedData ?? new List<ContactMessageDto>();
                }
            );
        }

        public async Task<ContactMessageDto> AddContactMessageAsync(ContactMessageDto message)
        {
            return await PostAsync<ContactMessageDto, ContactMessageDto>("ContactMessages", message);
        }

        // --- OFFLINE SYNC VE BAĞLANTI KONTROLÜ ---
        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var response = await client.GetAsync("https://oktay-sac-tasarim1.azurewebsites.net/api/Needs");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API bağlantı kontrolü hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetServiceStatusAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var response = await client.GetAsync("https://oktay-sac-tasarim1.azurewebsites.net/api/Needs");
                    
                    string status;
                    if (response.IsSuccessStatusCode)
                    {
                        status = "Çevrimiçi";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                    {
                        status = "Sunucu Bakımda (502)";
                        // 502 hatası için özel bildirim
                        ServiceStatusChanged?.Invoke(this, "Azure web servisi geçici olarak kullanılamıyor. Uygulama cache modunda çalışıyor.");
                    }
                    else
                    {
                        status = $"Hata: {response.StatusCode}";
                        ServiceStatusChanged?.Invoke(this, $"API servisi hatası: {response.StatusCode}");
                    }
                    
                    return status;
                }
            }
            catch (Exception ex)
            {
                string status = $"Bağlantı Hatası: {ex.Message}";
                ServiceStatusChanged?.Invoke(this, $"API bağlantı hatası: {ex.Message}");
                return status;
            }
        }

        private void StartSyncTimer()
        {
            _syncTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds); // Her 10 saniyede bir dene
            _syncTimer.Elapsed += async (sender, e) => await SyncOfflineOperationsAsync();
            _syncTimer.Start();
        }

        private async Task SyncOfflineOperationsAsync()
        {
            if (!_isOnline)
            {
                await CheckConnectionAsync(); // Bağlantı durumunu tekrar kontrol et
            }

            if (_isOnline && _offlineQueue.Any())
            {
                Console.WriteLine($"Çevrimdışı kuyruğu senkronize ediliyor. {_offlineQueue.Count} işlem bekliyor.");
                List<OfflineOperation> successfulOperations = new List<OfflineOperation>();

                foreach (var op in _offlineQueue.OrderBy(o => o.Timestamp))
            {
                try
                {
                        bool success = false;
                        StringContent postContent;
                        HttpResponseMessage responseMessage;

                    switch (op.Operation)
                    {
                            case "POST":
                                postContent = new StringContent(op.Data, Encoding.UTF8, "application/json");
                                responseMessage = await _httpClient.PostAsync(op.Endpoint, postContent);
                                success = responseMessage.IsSuccessStatusCode;
                                if (!success) Console.WriteLine($"POST işlemi başarısız: {responseMessage.StatusCode}");
                            break;
                            case "PUT":
                                postContent = new StringContent(op.Data, Encoding.UTF8, "application/json");
                                responseMessage = await _httpClient.PutAsync(op.Endpoint, postContent);
                                success = responseMessage.IsSuccessStatusCode;
                                if (!success) Console.WriteLine($"PUT işlemi başarısız: {responseMessage.StatusCode}");
                            break;
                            case "DELETE":
                                responseMessage = await _httpClient.DeleteAsync(op.Endpoint);
                                success = responseMessage.IsSuccessStatusCode;
                                if (!success) Console.WriteLine($"DELETE işlemi başarısız: {responseMessage.StatusCode}");
                            break;
                    }

                        if (success)
                        {
                            successfulOperations.Add(op);
                            Console.WriteLine($"Çevrimdışı işlem başarılı: {op.Operation} {op.Endpoint}");
                        }
                }
                catch (Exception ex)
                {
                        Console.WriteLine($"Çevrimdışı işlemi senkronize edilirken hata oluştu ({op.Operation} {op.Endpoint}): {ex.Message}");
                    }
                }

                _offlineQueue = _offlineQueue.Except(successfulOperations).ToList();
            SaveOfflineQueue();
            }
        }

        private void LoadOfflineQueue()
        {
            if (File.Exists(_offlineQueueFile))
            {
                try
            {
                var json = File.ReadAllText(_offlineQueueFile);
                    _offlineQueue = JsonConvert.DeserializeObject<List<OfflineOperation>>(json) ?? new List<OfflineOperation>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Çevrimdışı kuyruğu yüklenirken hata oluştu: {ex.Message}");
                    _offlineQueue = new List<OfflineOperation>();
                }
            }
            else
            {
                _offlineQueue = new List<OfflineOperation>();
            }
        }

        private void SaveOfflineQueue()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_offlineQueue, Formatting.Indented);
                File.WriteAllText(_offlineQueueFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Çevrimdışı kuyruğu kaydedilirken hata oluştu: {ex.Message}");
            }
        }

        public class OfflineOperation
        {
            public string Id { get; set; }
            public string Operation { get; set; }
            public string Endpoint { get; set; }
            public string Data { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                    _syncTimer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
