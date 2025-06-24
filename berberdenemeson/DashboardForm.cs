using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization; // CultureInfo için gerekli
using berberdenemeson; // ApiService ve modeller için

namespace berberdenemeson
{
    public partial class DashboardForm : Form
    {
        private readonly ApiService _apiService;
        private Timer _refreshTimer;
        private Timer _connectionCheckTimer;
        private List<ApiService.ServiceModel> _allServices; // Servis listesi eklendi

        // Eventler (MainForm tarafından abone olunacak)
        public event EventHandler RandevularRequested;
        public event EventHandler MusterilerRequested;
        public event EventHandler IhtiyaclarRequested;
        public event EventHandler RaporlarRequested;

        // Designer'dan gelen kontroller
        private Label lblPersonnelTitle;
        private Label lblCustomersTitle;
        private Label lblAppointmentsTitle;
        private Label lblRevenueTitle;
        private Label lblTodayAppointments;

        public DashboardForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            _allServices = new List<ApiService.ServiceModel>(); // Başlatma
            
            // ApiService event'lerini dinle
            _apiService.ErrorOccurred += OnApiError;
            _apiService.StatusChanged += OnApiStatusChanged;
            _apiService.ServiceStatusChanged += OnServiceStatusChanged; // Yeni event handler
            
            // InitializeModernUI(); // Bu metodun içeriği Designer.cs'den gelmeli veya manuel UI kodu olmalı
            this.Load += DashboardForm_Load;
            StartAutoRefresh();
            StartConnectionStatusTimer(); // Bağlantı kontrol timer'ını başlat
        }

        private void OnApiError(object sender, string error)
        {
            // Dashboard'da hata mesajlarını göstermek için uygun bir yer yoksa sadece log'la
            Console.WriteLine($"Dashboard API Hatası: {error}");
        }

        private void OnApiStatusChanged(object sender, string status)
        {
            // Sadece bağlantı durumu değişikliklerini göster
            if (status.Contains("Bağlantı durumu:"))
            {
                if (lblConnectionStatus != null && lblConnectionStatus.InvokeRequired)
                {
                    lblConnectionStatus.Invoke(new Action<string>(s => {
                        if (s.Contains("Çevrimiçi"))
                        {
                            lblConnectionStatus.Text = "🟢 Çevrimiçi";
                            lblConnectionStatus.ForeColor = Color.FromArgb(34, 197, 94);
                        }
                        else if (s.Contains("Çevrimdışı"))
                        {
                            lblConnectionStatus.Text = "🔴 Çevrimdışı";
                            lblConnectionStatus.ForeColor = Color.FromArgb(239, 68, 68);
                        }
                    }), status);
                }
                else if (lblConnectionStatus != null)
                {
                    if (status.Contains("Çevrimiçi"))
                    {
                        lblConnectionStatus.Text = "🟢 Çevrimiçi";
                        lblConnectionStatus.ForeColor = Color.FromArgb(34, 197, 94);
                    }
                    else if (status.Contains("Çevrimdışı"))
                    {
                        lblConnectionStatus.Text = "🔴 Çevrimdışı";
                        lblConnectionStatus.ForeColor = Color.FromArgb(239, 68, 68);
                    }
                }
            }
        }

        private void OnServiceStatusChanged(object sender, string message)
        {
            // UI thread'inde güvenli şekilde bildirim göster
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, string>(OnServiceStatusChanged), sender, message);
                return;
            }

            // Basit bir MessageBox ile bildirim göster
            MessageBox.Show(message, "Servis Durumu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: DashboardForm.DashboardForm_Load - Başladı");
            await LoadDashboardDataAsync();
            Console.WriteLine("DEBUG: DashboardForm.DashboardForm_Load - Tamamlandı");
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 60000; // Her 1 dakikada bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadDashboardDataAsync();
            _refreshTimer.Start();
        }

        private void StartConnectionStatusTimer()
        {
            // lblConnectionStatus Designer.cs'de tanımlı olduğundan emin olun.
            if (lblConnectionStatus != null)
            {
                _connectionCheckTimer = new Timer();
                _connectionCheckTimer.Interval = 5000;
                _connectionCheckTimer.Tick += async (s, e) => await CheckConnectionAsync(lblConnectionStatus);
                _connectionCheckTimer.Start();
                _ = CheckConnectionAsync(lblConnectionStatus); // İlk kontrol
            }
        }

        private async Task CheckConnectionAsync(Label statusLabel)
        {
            try
            {
                // Önce genel bağlantı durumunu kontrol et
                bool isOnline = await _apiService.CheckConnectionAsync();
                
                // Sonra detaylı servis durumunu al
                string serviceStatus = await _apiService.GetServiceStatusAsync();
                
                UpdateConnectionStatus(statusLabel, isOnline, serviceStatus);
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus(statusLabel, false, $"Hata: {ex.Message}");
            }
        }

        private void UpdateConnectionStatus(Label statusLabel, bool isOnline, string serviceStatus = null)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action<Label, bool, string>(UpdateConnectionStatus), statusLabel, isOnline, serviceStatus);
                return;
            }

            if (isOnline)
            {
                statusLabel.Text = "🟢 Çevrimiçi";
                statusLabel.ForeColor = Color.FromArgb(34, 197, 94);
                
                // Tooltip ile detaylı bilgi göster
                if (!string.IsNullOrEmpty(serviceStatus))
                {
                    statusLabel.Text += $" - {serviceStatus}";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(serviceStatus) && serviceStatus.Contains("502"))
                {
                    statusLabel.Text = "🟡 Sunucu Bakımda (502)";
                    statusLabel.ForeColor = Color.FromArgb(245, 158, 11); // Orange
                }
                else if (!string.IsNullOrEmpty(serviceStatus) && serviceStatus.Contains("Bağlantı Hatası"))
                {
                    statusLabel.Text = "🔴 Bağlantı Hatası";
                    statusLabel.ForeColor = Color.FromArgb(239, 68, 68);
                }
                else
                {
                    statusLabel.Text = "🔴 Çevrimdışı";
                    statusLabel.ForeColor = Color.FromArgb(239, 68, 68);
                }
                
                // Tooltip ile detaylı bilgi göster
                if (!string.IsNullOrEmpty(serviceStatus))
                {
                    statusLabel.Text += $" - {serviceStatus}";
                }
            }
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                Console.WriteLine("DEBUG: DashboardForm.LoadDashboardDataAsync - Başladı");
                
                // Randevu sayısı
                var appointments = await _apiService.GetAppointmentsAsync();
                // Eğer randevu nesnesinde musteri nesnesi varsa MusteriAdi ve MusteriTelefon'u doldur
                foreach (var appt in appointments)
                {
                    if (appt.musteri != null)
                    {
                        appt.MusteriAdi = appt.musteri.AdSoyad;
                        appt.MusteriTelefon = appt.musteri.Telefon;
                    }
                }
                Console.WriteLine($"DEBUG: DashboardForm - {appointments?.Count ?? 0} randevu yüklendi");
                
                // Müşteri sayısı
                var customers = await _apiService.GetCustomersAsync();
                Console.WriteLine($"DEBUG: DashboardForm - {customers?.Count ?? 0} müşteri yüklendi");
                
                // Personel sayısı
                var personnel = await _apiService.GetPersonnelAsync();
                Console.WriteLine($"DEBUG: DashboardForm - {personnel?.Count ?? 0} personel yüklendi");
                
                // İhtiyaç sayısı
                var needs = await _apiService.GetNeedsAsync();
                Console.WriteLine($"DEBUG: DashboardForm - {needs?.Count ?? 0} ihtiyaç yüklendi");

                // Servis verilerini yükle
                _allServices = await _apiService.GetServicesAsync();
                Console.WriteLine($"DEBUG: DashboardForm - {_allServices?.Count ?? 0} servis yüklendi");

                // Randevu verilerindeki müşteri bilgilerini kontrol et
                if (appointments != null && appointments.Any())
                {
                    foreach (var appointment in appointments.Take(3)) // İlk 3 randevuyu kontrol et
                    {
                        Console.WriteLine($"DEBUG: Dashboard - Randevu ID: {appointment.RandevuID}, Müşteri ID: {appointment.MusteriID}, Müşteri Adı: '{appointment.MusteriAdi}', Müşteri Telefon: '{appointment.MusteriTelefon}'");
                    }
                }

                // UI thread'inde güvenli güncelleme
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateDashboardUI(appointments, customers, personnel, needs)));
                }
                else
                {
                    UpdateDashboardUI(appointments, customers, personnel, needs);
                }
                
                Console.WriteLine("DEBUG: DashboardForm.LoadDashboardDataAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: DashboardForm.LoadDashboardDataAsync - Hata: {ex.Message}");
                
                // Hata dialog'unu UI thread'inde göster
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(error => {
                        MessageBox.Show("Gösterge paneli verileri yüklenirken bir hata oluştu: " + error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), ex.Message);
                }
                else
                {
                    MessageBox.Show("Gösterge paneli verileri yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDashboardUI(List<ApiService.AppointmentModel> appointments, 
                                     List<ApiService.CustomerModel> customers, 
                                     List<ApiService.PersonnelModel> personnel, 
                                     List<ApiService.NeedModel> needs)
        {
            Console.WriteLine("DEBUG: DashboardForm.UpdateDashboardUI - Başladı");
            
            // Sayıları güncelle
            if (lblAppointmentCount != null)
            {
                lblAppointmentCount.Text = appointments.Count.ToString();
                Console.WriteLine($"DEBUG: DashboardForm - Randevu sayısı: {appointments.Count}");
            }
            
            if (lblCustomerCount != null)
            {
                lblCustomerCount.Text = customers.Count.ToString();
                Console.WriteLine($"DEBUG: DashboardForm - Müşteri sayısı: {customers.Count}");
            }
            
            if (lblPersonnelCount != null)
            {
                lblPersonnelCount.Text = personnel.Count.ToString();
                Console.WriteLine($"DEBUG: DashboardForm - Personel sayısı: {personnel.Count}");
            }
            
            if (lblNeedsCount != null)
            {
                lblNeedsCount.Text = needs.Count.ToString();
                Console.WriteLine($"DEBUG: DashboardForm - İhtiyaç sayısı: {needs.Count}");
            }

            // Mali Durum Özeti (sadece client tarafında hesapla)
            decimal totalRandevuRevenue = appointments.Sum(a => a.Ucret);
            decimal totalIhtiyacCost = needs.Sum(n => n.ToplamFiyat);

            if (lblTotalRevenue != null)
                lblTotalRevenue.Text = totalRandevuRevenue.ToString("C2", new CultureInfo("tr-TR"));
            
            if (lblTotalCost != null)
                lblTotalCost.Text = totalIhtiyacCost.ToString("C2", new CultureInfo("tr-TR"));
            
            if (lblNetProfit != null)
                lblNetProfit.Text = (totalRandevuRevenue - totalIhtiyacCost).ToString("C2", new CultureInfo("tr-TR"));

            // Son Randevular DataGridView'i doldur
            if (dgvRecentAppointments != null)
            {
                Console.WriteLine("DEBUG: DashboardForm - DataGridView güncelleniyor");
                dgvRecentAppointments.DataSource = null; // Temizle
                dgvRecentAppointments.Columns.Clear();   // Kolonları temizle

                // Son 10 randevu veya belirli bir sayıdaki randevu
                var recentAppointments = appointments.OrderByDescending(a => a.RandevuZamani).Take(10).ToList();
                foreach (var a in recentAppointments)
                    a.RandevuZamani = a.RandevuZamani.ToLocalTime();
                Console.WriteLine($"DEBUG: DashboardForm - {recentAppointments.Count} son randevu gösterilecek");

                // Randevu verilerini servis bilgileriyle zenginleştir
                var enrichedAppointments = recentAppointments.Select(appointment =>
                {
                    // Servis adını RandevuServisler array'inden al
                    string servisAd = "Bilinmiyor";
                    if (appointment.RandevuServisler != null && appointment.RandevuServisler.Any())
                    {
                        var firstService = appointment.RandevuServisler.First();
                        servisAd = firstService.ServisAdi ?? firstService.Servis?.ServisAdi ?? appointment.ServisAd ?? "Bilinmiyor";
                    }
                    else
                    {
                        // Fallback olarak mevcut servis verilerini kullan
                        var service = _allServices?.FirstOrDefault(s => s.ServisID == appointment.ServisID);
                        servisAd = service?.ServisAdi ?? appointment.ServisAd ?? "Bilinmiyor";
                    }
                    
                    return new
                    {
                        appointment.RandevuID,
                        MusteriAdi = appointment.MusteriAdi ?? "Bilinmiyor",
                        ServisAd = servisAd,
                        appointment.RandevuZamani,
                        appointment.Ucret
                    };
                }).ToList();

                if (enrichedAppointments.Any())
                {
                    dgvRecentAppointments.AutoGenerateColumns = false;
                    dgvRecentAppointments.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriAdi", HeaderText = "Müşteri", DataPropertyName = "MusteriAdi", ReadOnly = true });
                    dgvRecentAppointments.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisAd", HeaderText = "Servis", DataPropertyName = "ServisAd", ReadOnly = true });
                    dgvRecentAppointments.Columns.Add(new DataGridViewTextBoxColumn { Name = "RandevuZamani", HeaderText = "Zaman", DataPropertyName = "RandevuZamani", ReadOnly = true, DefaultCellStyle = { Format = "g", FormatProvider = new CultureInfo("tr-TR") } });
                    dgvRecentAppointments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ucret", HeaderText = "Ücret", DataPropertyName = "Ucret", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });

                    dgvRecentAppointments.DataSource = enrichedAppointments;
                    dgvRecentAppointments.Refresh();
                    Console.WriteLine("DEBUG: DashboardForm - DataGridView verileri başarıyla güncellendi");
                }
                else
                {
                    dgvRecentAppointments.DataSource = null; // Veri yoksa temizle
                    Console.WriteLine("DEBUG: DashboardForm - Gösterilecek randevu yok");
                }
            }
            else
            {
                Console.WriteLine("DEBUG: DashboardForm - dgvRecentAppointments null");
            }

            // Son güncelleme zamanını göster
            if (lblLastUpdate != null)
                lblLastUpdate.Text = $"Son Güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
                
            Console.WriteLine("DEBUG: DashboardForm.UpdateDashboardUI - Tamamlandı");
        }

        // --- Buton Click Eventleri (MainForm tarafından tetiklenebilir) ---
        // Bu metodlar MainForm'dan tetiklenecek eventler için yer tutucudur.
        private void btnGoToRandevular_Click(object sender, EventArgs e)
        {
            RandevularRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnGoToMusteriler_Click(object sender, EventArgs e)
        {
            MusterilerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnGoToIhtiyaclar_Click(object sender, EventArgs e)
        {
            IhtiyaclarRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnGoToRaporlar_Click(object sender, EventArgs e)
        {
            RaporlarRequested?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // ApiService event'lerini dinlemeyi bırak
                if (_apiService != null)
                {
                    _apiService.ErrorOccurred -= OnApiError;
                    _apiService.StatusChanged -= OnApiStatusChanged;
                    _apiService.ServiceStatusChanged -= OnServiceStatusChanged;
                }

                // Timer'ları durdur
                if (_refreshTimer != null)
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Dispose();
                    _refreshTimer = null;
                }

                if (_connectionCheckTimer != null)
                {
                    _connectionCheckTimer.Stop();
                    _connectionCheckTimer.Dispose();
                    _connectionCheckTimer = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DashboardForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }

        private void panelStat4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
