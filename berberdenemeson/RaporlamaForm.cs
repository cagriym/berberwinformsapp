using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization; // CultureInfo için gerekli
using berberdenemeson; // ApiService ve Model sınıfları için gerekli
using System.IO; // Dosya işlemleri için eklendi
using System.Text; // StringBuilder için eklendi

namespace berberdenemeson
{
    public partial class RaporlamaForm : Form
    {
        private readonly ApiService _apiService;
        private Timer _refreshTimer;
        private Timer _connectionStatusTimer;

        // Model listeleri ApiService.cs'deki tanımlamalardan gelecek
        private List<ApiService.AppointmentModel> _appointments;
        private List<ApiService.CustomerModel> _customers;
        private List<ApiService.NeedModel> _needs;
        private List<ApiService.PersonnelModel> _allPersonnel;
        private List<ApiService.ServiceModel> _allServices; // Servis listesi eklendi

        public RaporlamaForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            _appointments = new List<ApiService.AppointmentModel>();
            _customers = new List<ApiService.CustomerModel>();
            _needs = new List<ApiService.NeedModel>();
            _allPersonnel = new List<ApiService.PersonnelModel>();
            _allServices = new List<ApiService.ServiceModel>(); // Başlatma

            // Designer'daki butonların click eventlerini bağla
            btnYenile.Click += BtnYenile_Click;
            btnFiltrele.Click += BtnFiltrele_Click;
            btnExport.Click += BtnExport_Click; // Yeni eklenen buton eventi

            this.Load += RaporlamaForm_Load;
            StartAutoRefresh();
            SetupDatePickers();
        }

        private void SetupDatePickers()
        {
            // Varsayılan olarak bugünden bir ay öncesi ve bugünü seç
            dtpBitis.Value = DateTime.Today;
            dtpBaslangic.Value = DateTime.Today.AddMonths(-1);
        }

        private async void RaporlamaForm_Load(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
            StartConnectionStatusTimer();
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 60000; // Her 1 dakikada bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadReportDataAsync();
            _refreshTimer.Start();
        }


        private async Task LoadReportDataAsync()
        {
            try
            {
                Console.WriteLine("DEBUG: RaporlamaForm.LoadReportDataAsync - Başladı");
                
                lblConnectionStatus.Text = "Veriler yükleniyor...";
                lblConnectionStatus.ForeColor = Color.DarkBlue;

                // API'den verileri çek
                // Tarih filtrelerini uygulayarak çek
                DateTime startDate = dtpBaslangic.Value.Date;
                DateTime endDate = dtpBitis.Value.Date.AddDays(1).AddSeconds(-1);

                var allAppointments = await _apiService.GetAppointmentsAsync();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {allAppointments?.Count ?? 0} randevu yüklendi");
                
                var filteredAppointments = allAppointments.Where(a => a.RandevuZamani >= startDate && a.RandevuZamani <= endDate).ToList();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {filteredAppointments.Count} randevu filtrelendi");
                
                _appointments = filteredAppointments;
                _customers = await _apiService.GetCustomersAsync();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {_customers?.Count ?? 0} müşteri yüklendi");
                
                _needs = await _apiService.GetNeedsAsync();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {_needs?.Count ?? 0} ihtiyaç yüklendi");
                
                _allPersonnel = await _apiService.GetPersonnelAsync();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {_allPersonnel?.Count ?? 0} personel yüklendi");

                _allServices = await _apiService.GetServicesAsync();
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - {_allServices?.Count ?? 0} servis yüklendi");

                foreach (var a in _appointments)
                    a.RandevuZamani = a.RandevuZamani.ToLocalTime();

                UpdateSummaryCards();
                PopulateRandevularTab();
                PopulateMusterilerTab();
                PopulatePersonelTab();
                PopulateIhtiyaclarTab();

                lblConnectionStatus.Text = "Veriler güncellendi.";
                lblConnectionStatus.ForeColor = Color.DarkGreen;
                
                Console.WriteLine("DEBUG: RaporlamaForm.LoadReportDataAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: RaporlamaForm.LoadReportDataAsync - Hata: {ex.Message}");
                
                lblConnectionStatus.Text = "Veriler yüklenemedi: " + ex.Message;
                lblConnectionStatus.ForeColor = Color.DarkRed;
                MessageBox.Show("Rapor verileri yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummaryCards()
        {
            // Toplam Randevu Sayısı
            lblToplamRandevu.Text = $"Toplam Randevu: {_appointments?.Count ?? 0}";

            // Toplam Müşteri Sayısı
            lblToplamMusteri.Text = $"Toplam Müşteri: {_customers?.Count ?? 0}";

            // Toplam Personel Sayısı
            lblToplamPersonel.Text = $"Toplam Personel: {_allPersonnel?.Count ?? 0}";

            // Toplam İhtiyaç Kalemi Sayısı
            lblToplamIhtiyac.Text = $"Toplam İhtiyaç: {_needs?.Count ?? 0}";

            // Ek mali durum özetleri
            decimal totalRandevuRevenue = _appointments?.Sum(a => a.Ucret) ?? 0;
            decimal totalIhtiyacCost = _needs?.Sum(n => n.ToplamFiyat) ?? 0;

            lblToplamUcret.Text = $"Toplam Gelir: {totalRandevuRevenue.ToString("C2", new CultureInfo("tr-TR"))}";
            lblToplamIhtiyacMaliyet.Text = $"Toplam Maliyet: {totalIhtiyacCost.ToString("C2", new CultureInfo("tr-TR"))}";
            lblNetKarZarar.Text = $"Net Kar/Zarar: {(totalRandevuRevenue - totalIhtiyacCost).ToString("C2", new CultureInfo("tr-TR"))}";
        }


        private void PopulateRandevularTab()
        {
            dgvRandevuRapor.DataSource = null;
            dgvRandevuRapor.Columns.Clear();

            if (_appointments == null || !_appointments.Any())
            {
                return;
            }

            // Randevu verilerini müşteri ve servis bilgileriyle birleştir
            var enrichedAppointments = _appointments.Select(appointment =>
            {
                var customer = _customers?.FirstOrDefault(c => c.MusteriID == appointment.MusteriID);
                var personnel = _allPersonnel?.FirstOrDefault(p => p.PersonelID == appointment.PersonelID);
                
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
                    MusteriAdi = customer?.AdSoyad ?? appointment.MusteriAdi ?? "Bilinmiyor",
                    MusteriTelefon = customer?.Telefon ?? appointment.MusteriTelefon ?? "Bilinmiyor",
                    ServisAd = servisAd,
                    appointment.RandevuZamani,
                    appointment.Ucret,
                    appointment.TamamlandiMi,
                    appointment.Aciklama,
                    PersonelAdSoyad = personnel?.AdSoyad ?? appointment.PersonelAdSoyad ?? "Bilinmiyor"
                };
            }).ToList();

            dgvRandevuRapor.AutoGenerateColumns = false;

            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "RandevuID", HeaderText = "ID", DataPropertyName = "RandevuID", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriAdi", HeaderText = "Müşteri Adı", DataPropertyName = "MusteriAdi", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriTelefon", HeaderText = "Müşteri Telefon", DataPropertyName = "MusteriTelefon", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisAd", HeaderText = "Servis Adı", DataPropertyName = "ServisAd", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "RandevuZamani", HeaderText = "Randevu Zamanı", DataPropertyName = "RandevuZamani", ReadOnly = true, DefaultCellStyle = { Format = "g", FormatProvider = new CultureInfo("tr-TR") } });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ucret", HeaderText = "Ücret", DataPropertyName = "Ucret", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });
            dgvRandevuRapor.Columns.Add(new DataGridViewCheckBoxColumn { Name = "TamamlandiMi", HeaderText = "Tamamlandı Mı?", DataPropertyName = "TamamlandiMi", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Aciklama", HeaderText = "Açıklama", DataPropertyName = "Aciklama", ReadOnly = true });
            dgvRandevuRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonelAdSoyad", HeaderText = "Personel", DataPropertyName = "PersonelAdSoyad", ReadOnly = true });

            dgvRandevuRapor.DataSource = enrichedAppointments;
            dgvRandevuRapor.Refresh();
        }

        private void PopulateMusterilerTab()
        {
            dgvMusteriRapor.DataSource = null;
            dgvMusteriRapor.Columns.Clear();

            if (_customers == null || !_customers.Any())
            {
                return;
            }

            dgvMusteriRapor.AutoGenerateColumns = false;

            dgvMusteriRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriID", HeaderText = "ID", DataPropertyName = "MusteriID", ReadOnly = true });
            dgvMusteriRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "AdSoyad", HeaderText = "Ad Soyad", DataPropertyName = "AdSoyad", ReadOnly = true });
            dgvMusteriRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefon", HeaderText = "Telefon", DataPropertyName = "Telefon", ReadOnly = true });
            dgvMusteriRapor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SonGelisTarihi",
                HeaderText = "Son Geliş Tarihi",
                DataPropertyName = "SonGelisTarihi",
                ReadOnly = true,
                DefaultCellStyle = { Format = "d", NullValue = "Yok", FormatProvider = new CultureInfo("tr-TR") }
            });
            dgvMusteriRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Kayıt Tarihi", DataPropertyName = "CreatedAt", ReadOnly = true, DefaultCellStyle = { Format = "g", NullValue = "Yok", FormatProvider = new CultureInfo("tr-TR") } });

            dgvMusteriRapor.DataSource = _customers.ToList();
            dgvMusteriRapor.Refresh();
        }

        private void PopulatePersonelTab()
        {
            dgvPersonelRapor.DataSource = null;
            dgvPersonelRapor.Columns.Clear();

            if (_allPersonnel == null || !_allPersonnel.Any())
            {
                return;
            }

            dgvPersonelRapor.AutoGenerateColumns = false;

            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonelID", HeaderText = "ID", DataPropertyName = "PersonelID", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "AdSoyad", HeaderText = "Ad Soyad", DataPropertyName = "AdSoyad", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefon", HeaderText = "Telefon", DataPropertyName = "Telefon", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "E-posta", DataPropertyName = "Email", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Pozisyon", HeaderText = "Pozisyon", DataPropertyName = "Pozisyon", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Maas", HeaderText = "Maaş", DataPropertyName = "Maas", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR"), NullValue = "Yok" } });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "IseGirisTarihi", HeaderText = "İşe Giriş Tarihi", DataPropertyName = "IseGirisTarihi", ReadOnly = true, DefaultCellStyle = { Format = "d", NullValue = "Yok", FormatProvider = new CultureInfo("tr-TR") } });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "CikisTarihi", HeaderText = "Çıkış Tarihi", DataPropertyName = "CikisTarihi", ReadOnly = true, DefaultCellStyle = { Format = "d", NullValue = "Yok", FormatProvider = new CultureInfo("tr-TR") } });
            dgvPersonelRapor.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Aktif", HeaderText = "Aktif", DataPropertyName = "Aktif", ReadOnly = true });
            dgvPersonelRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Aciklama", HeaderText = "Açıklama", DataPropertyName = "Aciklama", ReadOnly = true });

            dgvPersonelRapor.DataSource = _allPersonnel.ToList();
            dgvPersonelRapor.Refresh();
        }

        private void PopulateIhtiyaclarTab()
        {
            dgvIhtiyacRapor.DataSource = null;
            dgvIhtiyacRapor.Columns.Clear();

            if (_needs == null || !_needs.Any())
            {
                return;
            }

            dgvIhtiyacRapor.AutoGenerateColumns = false;

            dgvIhtiyacRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "IhtiyacID", HeaderText = "ID", DataPropertyName = "IhtiyacID", ReadOnly = true });
            dgvIhtiyacRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "IhtiyacTuru", HeaderText = "İhtiyaç Türü", DataPropertyName = "IhtiyacTuru", ReadOnly = true });
            dgvIhtiyacRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fiyat", HeaderText = "Fiyat", DataPropertyName = "Fiyat", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });
            dgvIhtiyacRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "Aciklama", HeaderText = "Açıklama", DataPropertyName = "Aciklama", ReadOnly = true });
            dgvIhtiyacRapor.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Kayıt Tarihi", DataPropertyName = "CreatedAt", ReadOnly = true, DefaultCellStyle = { Format = "g", NullValue = "Yok", FormatProvider = new CultureInfo("tr-TR") } });

            dgvIhtiyacRapor.DataSource = _needs.ToList();
            dgvIhtiyacRapor.Refresh();
        }


        private async void BtnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Metin Dosyası (*.txt)|*.txt";
                sfd.Title = "Raporu Kaydet";
                sfd.FileName = $"Rapor_{dtpBaslangic.Value.ToString("yyyyMMdd")}_{dtpBitis.Value.ToString("yyyyMMdd")}.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblConnectionStatus.Text = "Rapor dışa aktarılıyor...";
                        lblConnectionStatus.ForeColor = Color.DarkBlue;

                        DateTime startDate = dtpBaslangic.Value.Date;
                        DateTime endDate = dtpBitis.Value.Date.AddDays(1).AddSeconds(-1);

                        var allAppointments = await _apiService.GetAppointmentsAsync();
                        var filteredAppointments = allAppointments.Where(a => a.RandevuZamani >= startDate && a.RandevuZamani <= endDate).ToList();
                        var allCustomers = await _apiService.GetCustomersAsync() ?? new List<ApiService.CustomerModel>();
                        var allNeeds = await _apiService.GetNeedsAsync() ?? new List<ApiService.NeedModel>();

                        StringBuilder reportContent = new StringBuilder();

                        reportContent.AppendLine("**************************************************");
                        reportContent.AppendLine("             BERBER YÖNETİM SİSTEMİ RAPORU        ");
                        reportContent.AppendLine("**************************************************");
                        reportContent.AppendLine($"Rapor Tarihi: {DateTime.Now.ToLocalTime():g}");
                        reportContent.AppendLine($"Rapor Aralığı: {startDate:d} - {endDate:d}");
                        reportContent.AppendLine("**************************************************");
                        reportContent.AppendLine();

                        decimal totalAppointmentRevenue = filteredAppointments.Sum(appt => appt.Ucret); // Nullable düzeltmesi yapıldı
                        reportContent.AppendLine("--- RANDEVU GELİRLERİ DETAYI ---");
                        reportContent.AppendLine("--------------------------------");
                        if (filteredAppointments.Any())
                        {
                            foreach (var appt in filteredAppointments.OrderBy(a => a.RandevuZamani))
                            {
                                var customer = allCustomers.FirstOrDefault(c => c.MusteriID == appt.MusteriID);
                                string customerInfo = customer != null ? $"{customer.AdSoyad} ({customer.Telefon})" : $"Müşteri ID: {appt.MusteriID} (Bulunamadı)";

                                reportContent.AppendLine($"Randevu ID: {appt.RandevuID}");
                                reportContent.AppendLine($"Müşteri: {customerInfo}");
                                reportContent.AppendLine($"Servis: {appt.ServisAd}");
                                reportContent.AppendLine($"Tarih/Saat: {appt.RandevuZamani.ToLocalTime():g}");
                                reportContent.AppendLine($"Ücret: {appt.Ucret.ToString("C2", new CultureInfo("tr-TR"))}");
                                reportContent.AppendLine("--------------------------------");
                            }
                        }
                        else
                        {
                            reportContent.AppendLine("Bu tarih aralığında randevu bulunamadı.");
                        }
                        reportContent.AppendLine();
                        reportContent.AppendLine($"** TOPLAM RANDEVU GELİRİ ({startDate:d} - {endDate:d}): {totalAppointmentRevenue.ToString("C2", new CultureInfo("tr-TR"))} **");
                        reportContent.AppendLine();

                        // Müşteri İstatistikleri
                        reportContent.AppendLine("--- HİZMET ALAN MÜŞTERİLER ---");
                        reportContent.AppendLine("----------------------------");
                        var customersVisited = filteredAppointments.Select(a => a.MusteriID).Distinct().ToList();
                        if (customersVisited.Any())
                        {
                            reportContent.AppendLine($"Bu tarih aralığında hizmet alan toplam müşteri sayısı: {customersVisited.Count}");
                            reportContent.AppendLine("Müşteri Listesi:");
                            foreach (var custId in customersVisited)
                            {
                                var customer = allCustomers.FirstOrDefault(c => c.MusteriID == custId);
                                if (customer != null)
                                {
                                    decimal customerTotalSpent = filteredAppointments
                                        .Where(a => a.MusteriID == custId)
                                        .Sum(a => a.Ucret); // Nullable düzeltmesi yapıldı
                                    reportContent.AppendLine($"- {customer.AdSoyad} ({customer.Telefon}) - Toplam Harcama: {customerTotalSpent.ToString("C2", new CultureInfo("tr-TR"))}");
                                }
                            }
                        }
                        else
                        {
                            reportContent.AppendLine("Bu tarih aralığında hizmet alan müşteri bulunamadı.");
                        }
                        reportContent.AppendLine();

                        // İhtiyaç Maliyetleri
                        decimal totalNeedCost = allNeeds.Sum(n => n.ToplamFiyat); // Nullable düzeltmesi yapıldı
                        reportContent.AppendLine("--- İHTİYAÇ MALİYETLERİ ---");
                        reportContent.AppendLine("--------------------------");
                        if (allNeeds.Any())
                        {
                            foreach (var need in allNeeds.OrderBy(n => n.CreatedAt))
                            {
                                reportContent.AppendLine($"İhtiyaç ID: {need.IhtiyacID}, Tür: {need.IhtiyacTuru}, Miktar: {need.Miktar}, Birim Fiyat: {need.Fiyat.ToString("C2", new CultureInfo("tr-TR"))}, Toplam Fiyat: {need.ToplamFiyat.ToString("C2", new CultureInfo("tr-TR"))}, Tarih: {need.CreatedAt?.ToString("g") ?? "Bilinmiyor"}");
                            }
                        }
                        else
                        {
                            reportContent.AppendLine("Bu tarih aralığında ihtiyaç kaydı bulunamadı.");
                        }
                        reportContent.AppendLine();
                        reportContent.AppendLine($"** TOPLAM İHTİYAÇ MALİYETİ ({startDate:d} - {endDate:d}): {totalNeedCost.ToString("C2", new CultureInfo("tr-TR"))} **");
                        reportContent.AppendLine();


                        // Net Kar/Zarar
                        decimal netProfitLoss = totalAppointmentRevenue - totalNeedCost;
                        reportContent.AppendLine("--- NET KAR/ZARAR ---");
                        reportContent.AppendLine("---------------------");
                        reportContent.AppendLine($"NET KAR/ZARAR: {netProfitLoss.ToString("C2", new CultureInfo("tr-TR"))}");
                        reportContent.AppendLine("**************************************************");

                        // Dosyaya yaz
                        File.WriteAllText(sfd.FileName, reportContent.ToString());

                        lblConnectionStatus.Text = "Rapor başarıyla dışa aktarıldı!";
                        lblConnectionStatus.ForeColor = Color.DarkGreen;
                        MessageBox.Show("Rapor başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        lblConnectionStatus.Text = "Rapor dışa aktarılırken hata oluştu: " + ex.Message;
                        lblConnectionStatus.ForeColor = Color.DarkRed;
                        MessageBox.Show("Rapor kaydedilirken bir hata oluştu:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BtnYenile_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        private async void BtnFiltrele_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync(); // Filtreleme için tarih seçicilerden değeri alıp API'ye gönderir.
        }

        private async Task CheckConnectionAsync(Label statusLabel)
        {
            try
            {
                bool isOnline = await _apiService.CheckConnectionAsync();
                UpdateConnectionStatus(statusLabel, isOnline);
            }
            catch
            {
                UpdateConnectionStatus(statusLabel, false);
            }
        }

        private void UpdateConnectionStatus(Label statusLabel, bool isOnline)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action<Label, bool>(UpdateConnectionStatus), statusLabel, isOnline);
                return;
            }

            if (isOnline)
            {
                statusLabel.Text = "🟢 Çevrimiçi";
                statusLabel.ForeColor = Color.FromArgb(34, 197, 94);
            }
            else
            {
                statusLabel.Text = "🔴 Çevrimdışı";
                statusLabel.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private void StartConnectionStatusTimer()
        {
            _connectionStatusTimer = new Timer();
            _connectionStatusTimer.Interval = 5000;
            _connectionStatusTimer.Tick += async (s, e) => await CheckConnectionAsync(lblConnectionStatus);
            _connectionStatusTimer.Start();
            _ = CheckConnectionAsync(lblConnectionStatus); // İlk kontrol
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Timer'ları durdur
                if (_refreshTimer != null)
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Dispose();
                    _refreshTimer = null;
                }

                if (_connectionStatusTimer != null)
                {
                    _connectionStatusTimer.Stop();
                    _connectionStatusTimer.Dispose();
                    _connectionStatusTimer = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RaporlamaForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }
    }
}
