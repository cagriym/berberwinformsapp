using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http; // ApiService kullanıldığından sadece gerekliyse kalsın
using System.Globalization; // CultureInfo için
using berberdenemeson; // ApiService ve modeller için

namespace berberdenemeson
{
    public partial class RandevularForm : Form
    {
        private readonly ApiService _apiService;
        private Timer _refreshTimer;
        private Timer _connectionStatusTimer; // Bağlantı durumu için timer
        private List<ApiService.AppointmentModel> _allAppointments;
        private List<ApiService.CustomerModel> _allCustomers;
        private List<ApiService.ServiceModel> _allServices;
        // private HttpClient _httpClient; // ApiService kendi içinde yönettiği için kaldırıldı.
        private Form _modalForm;

        public RandevularForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            _allAppointments = new List<ApiService.AppointmentModel>();
            _allCustomers = new List<ApiService.CustomerModel>();
            _allServices = new List<ApiService.ServiceModel>();

            // ApiService event'lerini dinle
            _apiService.ErrorOccurred += OnApiError;
            _apiService.StatusChanged += OnApiStatusChanged;

            // Buton click eventlerini bağla (Designer.cs'deki isimleri kontrol edin)
            btnSearchAdd.Click += BtnSearchAdd_Click; // Yeni ekleme butonu
            btnDuzenle.Click += BtnDuzenle_Click;
            btnSil.Click += BtnSil_Click;
            btnTamamla.Click += BtnTamamla_Click;
            btnSearch.Click += BtnSearch_Click;
            btnRefresh.Click += BtnRefresh_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            dgvRandevular.CellClick += AppointmentsGrid_CellClick;

            this.Load += RandevularForm_Load;
            StartAutoRefresh();
            StartConnectionStatusTimer(); // Bağlantı kontrol timer'ını başlat
            SetupSearchTextBoxPlaceholder();
            SetupDatePickers();
        }

        private void SetupDatePickers()
        {
            dtpAramaTarihi.Format = DateTimePickerFormat.Custom;
            dtpAramaTarihi.CustomFormat = "dd.MM.yyyy"; // Sadece tarihi göster
            dtpAramaTarihi.ShowUpDown = false; // Saat seçici olmasın
            dtpAramaTarihi.Value = DateTime.Today; // Varsayılan olarak bugünü göster
            dtpAramaTarihi.Checked = false; // Başlangıçta seçim kutusu işaretli olmasın
            dtpAramaTarihi.ShowCheckBox = true; // CheckBox göster
        }

        private async void RandevularForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: RandevularForm.RandevularForm_Load - Başladı");
            await LoadCustomersAndServicesAsync(); // Önce müşteri ve servisleri yükle
            await LoadAppointmentsAsync(); // Sonra randevuları yükle
            Console.WriteLine("DEBUG: RandevularForm.RandevularForm_Load - Tamamlandı");
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000; // Her 30 saniyede bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadAppointmentsAsync();
            _refreshTimer.Start();
        }

        private void StartConnectionStatusTimer()
        {
            if (_statusLabel != null) // Kontrolün varlığını kontrol et
            {
                _connectionStatusTimer = new Timer();
                _connectionStatusTimer.Interval = 5000;
                _connectionStatusTimer.Tick += async (s, e) => await CheckConnectionAsync(_statusLabel);
                _connectionStatusTimer.Start();
                _ = CheckConnectionAsync(_statusLabel); // İlk kontrol
            }
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

        private async Task LoadAppointmentsAsync()
        {
            try
            {
                Console.WriteLine("DEBUG: RandevularForm.LoadAppointmentsAsync - Başladı");
                
                // UI thread'inde güvenli güncelleme
                if (_statusLabel != null)
                {
                    if (_statusLabel.InvokeRequired)
                    {
                        _statusLabel.Invoke(new Action(() => {
                            _statusLabel.Text = "Randevular yükleniyor...";
                            _statusLabel.ForeColor = Color.DarkBlue;
                        }));
                    }
                    else
                    {
                        _statusLabel.Text = "Randevular yükleniyor...";
                        _statusLabel.ForeColor = Color.DarkBlue;
                    }
                }

                _allAppointments = await _apiService.GetAppointmentsAsync();
                // Eğer randevu nesnesinde musteri nesnesi varsa MusteriAdi ve MusteriTelefon'u doldur
                foreach (var appt in _allAppointments)
                {
                    if (appt.musteri != null)
                    {
                        appt.MusteriAdi = appt.musteri.AdSoyad;
                        appt.MusteriTelefon = appt.musteri.Telefon;
                    }
                }
                Console.WriteLine($"DEBUG: RandevularForm.LoadAppointmentsAsync - API'den {_allAppointments?.Count ?? 0} randevu geldi");
                
                // UI thread'inde DataGridView'i güncelle
                if (dgvRandevular.InvokeRequired)
                {
                    dgvRandevular.Invoke(new Action(() => {
                        RefreshAppointmentsList();
                        Console.WriteLine("DEBUG: RandevularForm.LoadAppointmentsAsync - RefreshAppointmentsList Invoke ile çağrıldı");
                    }));
                }
                else
                {
                    RefreshAppointmentsList(); // Filtreleme ve DataGridView'e bağlama
                    Console.WriteLine("DEBUG: RandevularForm.LoadAppointmentsAsync - RefreshAppointmentsList direkt çağrıldı");
                }
                
                Console.WriteLine("DEBUG: RandevularForm.LoadAppointmentsAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: RandevularForm.LoadAppointmentsAsync - Hata: {ex.Message}");
                
                // Hata mesajını UI thread'inde göster
                if (_statusLabel != null)
                {
                    if (_statusLabel.InvokeRequired)
                    {
                        _statusLabel.Invoke(new Action<string>(error => {
                            _statusLabel.Text = "Randevular yüklenemedi: " + error;
                            _statusLabel.ForeColor = Color.DarkRed;
                        }), ex.Message);
                    }
                    else
                    {
                        _statusLabel.Text = "Randevular yüklenemedi: " + ex.Message;
                        _statusLabel.ForeColor = Color.DarkRed;
                    }
                }

                // Hata dialog'unu UI thread'inde göster
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(error => {
                        MessageBox.Show("Randevular yüklenirken bir hata oluştu: " + error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), ex.Message);
                }
                else
                {
                    MessageBox.Show("Randevular yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task LoadCustomersAndServicesAsync()
        {
            try
            {
                Console.WriteLine("DEBUG: RandevularForm.LoadCustomersAndServicesAsync - Başladı");
                
                // Müşterileri yükle
                _allCustomers = await _apiService.GetCustomersAsync();
                Console.WriteLine($"DEBUG: RandevularForm.LoadCustomersAndServicesAsync - {_allCustomers?.Count ?? 0} müşteri yüklendi");
                
                // Servisleri yükle
                _allServices = await _apiService.GetServicesAsync();
                Console.WriteLine($"DEBUG: RandevularForm.LoadCustomersAndServicesAsync - {_allServices?.Count ?? 0} servis yüklendi");
                
                Console.WriteLine("DEBUG: RandevularForm.LoadCustomersAndServicesAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: RandevularForm.LoadCustomersAndServicesAsync - Hata: {ex.Message}");
                MessageBox.Show("Müşteri veya servis bilgileri yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RefreshAppointmentsList()
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();
                var selectedDate = dtpAramaTarihi.Checked ? dtpAramaTarihi.Value.Date : (DateTime?)null;

                // Placeholder metni kontrol et
                if (searchText == "randevu ara...")
                {
                    searchText = "";
                }

                var filteredAppointments = _allAppointments.Where(a =>
                    (string.IsNullOrEmpty(searchText) ||
                     (a.MusteriAdi?.ToLower().Contains(searchText) ?? false) ||
                     (a.MusteriTelefon?.ToLower().Contains(searchText) ?? false) ||
                     (a.ServisAd?.ToLower().Contains(searchText) ?? false)) &&
                    (!selectedDate.HasValue || a.RandevuZamani.Date == selectedDate.Value)
                ).ToList();

                // Randevu verilerini servis bilgileriyle zenginleştir
                var enrichedAppointments = filteredAppointments.Select(appointment =>
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
                        appointment.MusteriID,
                        MusteriAdi = appointment.MusteriAdi ?? "Bilinmiyor",
                        MusteriTelefon = appointment.MusteriTelefon ?? "Bilinmiyor",
                        RandevuZamani = appointment.RandevuZamani.ToLocalTime(),
                        ServisAd = servisAd,
                        appointment.Ucret,
                        appointment.TamamlandiMi,
                        appointment.Aciklama,
                        appointment.PersonelID,
                        appointment.PersonelAdSoyad
                    };
                }).ToList();

                // DataGridView'i temizle ve yeniden yapılandır
                dgvRandevular.DataSource = null;
                dgvRandevular.Columns.Clear();

                dgvRandevular.AutoGenerateColumns = false;

                // Kolonları ekle
                dgvRandevular.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriAdi", HeaderText = "Müşteri", DataPropertyName = "MusteriAdi", ReadOnly = true });
                dgvRandevular.Columns.Add(new DataGridViewTextBoxColumn { Name = "MusteriTelefon", HeaderText = "Telefon", DataPropertyName = "MusteriTelefon", ReadOnly = true });
                dgvRandevular.Columns.Add(new DataGridViewTextBoxColumn { Name = "RandevuZamani", HeaderText = "Tarih & Saat", DataPropertyName = "RandevuZamani", ReadOnly = true, DefaultCellStyle = { Format = "g", NullValue = "Yok" } });
                dgvRandevular.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisAd", HeaderText = "Servis", DataPropertyName = "ServisAd", ReadOnly = true });
                dgvRandevular.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ucret", HeaderText = "Ücret", DataPropertyName = "Ucret", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });
                dgvRandevular.Columns.Add(new DataGridViewCheckBoxColumn { Name = "TamamlandiMi", HeaderText = "Tamamlandı", DataPropertyName = "TamamlandiMi", ReadOnly = true });

                // Veriyi bağla
                dgvRandevular.DataSource = enrichedAppointments;
                
                // Debug için satır sayısını logla
                Console.WriteLine($"DEBUG: RandevularForm - {enrichedAppointments.Count} randevu filtrelendi ve DataGridView'e bağlandı");
                
                dgvRandevular.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: RandevularForm.RefreshAppointmentsList - Hata: {ex.Message}");
                MessageBox.Show($"Randevu listesi güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppointmentsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var selectedAppointment = dgvRandevular.Rows[e.RowIndex].DataBoundItem as ApiService.AppointmentModel;
            if (selectedAppointment == null) return;
        }

        private void ShowAppointmentModal(ApiService.AppointmentModel appointment = null)
        {
            Console.WriteLine("DEBUG: RandevularForm.ShowAppointmentModal - Başladı");
            Console.WriteLine($"DEBUG: RandevularForm.ShowAppointmentModal - _allCustomers.Count: {_allCustomers?.Count ?? 0}");
            Console.WriteLine($"DEBUG: RandevularForm.ShowAppointmentModal - _allServices.Count: {_allServices?.Count ?? 0}");
            
            // Müşteri ve servis listeleri boşsa uyarı ver
            if (_allCustomers == null || _allCustomers.Count == 0)
            {
                MessageBox.Show("Müşteri listesi yüklenemedi. Lütfen sayfayı yenileyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_allServices == null || _allServices.Count == 0)
            {
                MessageBox.Show("Servis listesi yüklenemedi. Lütfen sayfayı yenileyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _modalForm = new Form
            {
                Text = appointment == null ? "Yeni Randevu Ekle" : "Randevuyu Düzenle",
                Size = new Size(400, 550), // Yükseklik artırıldı
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            // Kontroller
            var cmbMusteri = new ComboBox { Name = "cmbMusteri", Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMusteri.Items.AddRange(_allCustomers.Select(c => $"{c.AdSoyad} - {c.Telefon}").ToArray());
            Console.WriteLine($"DEBUG: RandevularForm.ShowAppointmentModal - {cmbMusteri.Items.Count} müşteri ComboBox'e eklendi");

            var cmbServis = new ComboBox { Name = "cmbServis", Left = 20, Top = 60, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbServis.Items.AddRange(_allServices.Select(s => $"{s.ServisAdi} ({s.VarsayilanUcret?.ToString("C2", new CultureInfo("tr-TR"))})").ToArray()); // Nullable kontrolü ve format
            Console.WriteLine($"DEBUG: RandevularForm.ShowAppointmentModal - {cmbServis.Items.Count} servis ComboBox'e eklendi");

            var dtpRandevuZamani = new DateTimePicker { Name = "dtpRandevuZamani", Left = 20, Top = 100, Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "dd.MM.yyyy HH:mm" };

            var txtUcret = new TextBox { Name = "txtUcret", Left = 20, Top = 140, Width = 300 };
            txtUcret.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == ',') && ((s as TextBox).Text.IndexOf(',') > -1))
                {
                    e.Handled = true;
                }
            };

            var txtAciklama = new TextBox { Name = "txtAciklama", Left = 20, Top = 180, Width = 300, Multiline = true, Height = 80 };

            var chkTamamlandiMi = new CheckBox { Name = "chkTamamlandiMi", Text = "Tamamlandı", Left = 20, Top = 270, AutoSize = true };

            // Personel Seçimi (Yeni eklendi)
            var cmbPersonel = new ComboBox { Name = "cmbPersonel", Left = 20, Top = 310, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Visible = true };
            // _allPersonnel listesini ApiService'den çekip doldurun
            cmbPersonel.Items.AddRange(_apiService.GetPersonnelAsync().Result.Select(p => $"{p.AdSoyad}").ToArray());
            cmbPersonel.Tag = _apiService.GetPersonnelAsync().Result; // Personel listesini Tag olarak sakla

            var btnSave = new Button { Text = "Kaydet", Left = 20, Top = 390, Width = 100, Height = 30, DialogResult = DialogResult.OK };
            btnSave.Click += async (btnSender, btnEvent) =>
            {
                try
                {
                    var selectedCustomerText = cmbMusteri.SelectedItem as string;
                    var customerModel = _allCustomers.FirstOrDefault(c => $"{c.AdSoyad} - {c.Telefon}" == selectedCustomerText);

                    var selectedServiceText = cmbServis.SelectedItem as string;
                    var serviceModel = _allServices.FirstOrDefault(srv => selectedServiceText != null && selectedServiceText.StartsWith(srv.ServisAdi));

                    var selectedPersonnelText = cmbPersonel.SelectedItem as string;
                    var personnelModel = _apiService.GetPersonnelAsync().Result.FirstOrDefault(p => $"{p.AdSoyad}" == selectedPersonnelText); // Personel modelini al

                    if (customerModel == null || serviceModel == null)
                    {
                        MessageBox.Show("Lütfen müşteri ve servis seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _modalForm.DialogResult = DialogResult.None;
                        return;
                    }

                    if (!decimal.TryParse(txtUcret.Text, NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal ucret))
                    {
                        MessageBox.Show("Geçerli bir ücret giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _modalForm.DialogResult = DialogResult.None;
                        return;
                    }

                    var newAppointment = new ApiService.AppointmentModel
                    {
                        MusteriID = customerModel.MusteriID,
                        MusteriAdi = customerModel.AdSoyad,
                        MusteriTelefon = customerModel.Telefon,
                        ServisID = serviceModel.ServisID,
                        ServisAd = serviceModel.ServisAdi,
                        RandevuZamani = dtpRandevuZamani.Value.ToLocalTime(),
                        Ucret = ucret,
                        Aciklama = txtAciklama.Text,
                        TamamlandiMi = chkTamamlandiMi.Checked,
                        PersonelID = personnelModel?.PersonelID, // Personel ID ekle
                        PersonelAdSoyad = personnelModel?.AdSoyad // Personel Ad Soyad ekle
                    };

                    if (appointment == null) // Yeni randevu
                    {
                        await _apiService.AddAppointmentAsync(newAppointment);
                    }
                    else // Randevu düzenleme
                    {
                        newAppointment.RandevuID = appointment.RandevuID;
                        await _apiService.UpdateAppointmentAsync(appointment.RandevuID, newAppointment);
                    }
                    MessageBox.Show("Randevu başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadAppointmentsAsync(); // Listeyi güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Randevu kaydedilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _modalForm.DialogResult = DialogResult.None;
                }
            };

            // Başlangıç değerlerini ata (düzenleme modunda)
            if (appointment != null)
            {
                var currentCustomer = _allCustomers.FirstOrDefault(c => c.MusteriID == appointment.MusteriID);
                if (currentCustomer != null)
                {
                    cmbMusteri.SelectedItem = $"{currentCustomer.AdSoyad} - {currentCustomer.Telefon}";
                }

                var currentService = _allServices.FirstOrDefault(s => s.ServisID == appointment.ServisID);
                if (currentService != null)
                {
                    cmbServis.SelectedItem = $"{currentService.ServisAdi} ({currentService.VarsayilanUcret?.ToString("C2", new CultureInfo("tr-TR"))})";
                }

                dtpRandevuZamani.Value = appointment.RandevuZamani.ToLocalTime();
                txtUcret.Text = appointment.Ucret.ToString(new CultureInfo("tr-TR")); // Kültüre göre formatlama
                txtAciklama.Text = appointment.Aciklama;
                chkTamamlandiMi.Checked = appointment.TamamlandiMi;

                // Personel Seçimini Ata
                var currentPersonnel = _apiService.GetPersonnelAsync().Result.FirstOrDefault(p => p.PersonelID == appointment.PersonelID);
                if (currentPersonnel != null)
                {
                    cmbPersonel.SelectedItem = currentPersonnel.AdSoyad;
                }
            }
            else
            {
                // Yeni randevu için varsayılan ücreti servis seçiminden al
                cmbServis.SelectedIndexChanged += (senderObj, eventArgs) =>
                {
                    var selectedServiceIndex = cmbServis.SelectedIndex;
                    if (selectedServiceIndex != -1 && selectedServiceIndex < _allServices.Count)
                    {
                        txtUcret.Text = _allServices[selectedServiceIndex].VarsayilanUcret?.ToString(new CultureInfo("tr-TR")) ?? "0";
                    }
                };
            }

            // Kontrolleri forma ekle
            _modalForm.Controls.Add(new Label { Text = "Müşteri:", Left = 20, Top = 5, AutoSize = true });
            _modalForm.Controls.Add(cmbMusteri);
            _modalForm.Controls.Add(new Label { Text = "Servis:", Left = 20, Top = 45, AutoSize = true });
            _modalForm.Controls.Add(cmbServis);
            _modalForm.Controls.Add(new Label { Text = "Randevu Zamanı:", Left = 20, Top = 85, AutoSize = true });
            _modalForm.Controls.Add(dtpRandevuZamani);
            _modalForm.Controls.Add(new Label { Text = "Ücret:", Left = 20, Top = 125, AutoSize = true });
            _modalForm.Controls.Add(txtUcret);
            _modalForm.Controls.Add(new Label { Text = "Açıklama:", Left = 20, Top = 165, AutoSize = true });
            _modalForm.Controls.Add(txtAciklama);
            _modalForm.Controls.Add(chkTamamlandiMi);
            _modalForm.Controls.Add(new Label { Text = "Personel:", Left = 20, Top = 290, AutoSize = true }); // Yeni personel label
            _modalForm.Controls.Add(cmbPersonel); // Yeni personel combobox
            _modalForm.Controls.Add(btnSave);

            _modalForm.ShowDialog(this);
        }

        private void BtnSearchAdd_Click(object sender, EventArgs e) // BtnYeniRandevu ve BtnAdd yerine bu
        {
            ShowAppointmentModal(null); // Yeni randevu
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvRandevular.SelectedRows.Count > 0)
            {
                var selectedAppointment = dgvRandevular.SelectedRows[0].DataBoundItem as ApiService.AppointmentModel;
                if (selectedAppointment != null)
                {
                    ShowAppointmentModal(selectedAppointment); // Düzenleme
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir randevu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvRandevular.SelectedRows.Count > 0)
            {
                var selectedAppointment = dgvRandevular.SelectedRows[0].DataBoundItem as ApiService.AppointmentModel;
                if (selectedAppointment != null)
                {
                    if (MessageBox.Show($"'{selectedAppointment.MusteriAdi}' adlı müşterinin randevusunu silmek istediğinizden emin misiniz?", "Randevu Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            bool success = await _apiService.DeleteAppointmentAsync(selectedAppointment.RandevuID);
                            if (success)
                            {
                                MessageBox.Show("Randevu başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                await LoadAppointmentsAsync(); // Listeyi güncelle
                }
                else
                {
                                MessageBox.Show("Randevu silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Randevu silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir randevu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnTamamla_Click(object sender, EventArgs e)
        {
            if (dgvRandevular.SelectedRows.Count > 0)
            {
                var selectedAppointment = dgvRandevular.SelectedRows[0].DataBoundItem as ApiService.AppointmentModel;
                if (selectedAppointment != null)
                {
                    if (!selectedAppointment.TamamlandiMi)
                    {
                        selectedAppointment.TamamlandiMi = true; // Durumu tamamlandı olarak işaretle
                        try
                        {
                            await _apiService.UpdateAppointmentAsync(selectedAppointment.RandevuID, selectedAppointment);
                            MessageBox.Show("Randevu başarıyla tamamlandı olarak işaretlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await LoadAppointmentsAsync(); // Listeyi güncelle
            }
            catch (Exception ex)
            {
                            MessageBox.Show("Randevu durumu güncellenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu randevu zaten tamamlanmış.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen tamamlamak için bir randevu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Arama ve Yenileme Butonları ---
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefreshAppointmentsList(); // Mevcut listeyi filtrele
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadAppointmentsAsync(); // API'den tüm randevuları yeniden yükle
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshAppointmentsList(); // Arama metni değiştiğinde listeyi filtrele
        }

        // txtSearch için Placeholder metin yönetimi
        private void SetupSearchTextBoxPlaceholder()
        {
            txtSearch.Text = "Randevu ara...";
            txtSearch.ForeColor = SystemColors.GrayText;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Randevu ara...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Randevu ara...";
                txtSearch.ForeColor = SystemColors.GrayText;
            }
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
                }

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
                Console.WriteLine($"RandevularForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }

        private void OnApiError(object sender, string error)
        {
            if (_statusLabel != null && _statusLabel.InvokeRequired)
            {
                _statusLabel.Invoke(new Action<object, string>(OnApiError), sender, error);
                return;
            }

            if (_statusLabel != null)
            {
                _statusLabel.Text = $"API Hatası: {error}";
                _statusLabel.ForeColor = Color.DarkRed;
            }
        }

        private void OnApiStatusChanged(object sender, string status)
        {
            if (_statusLabel != null && _statusLabel.InvokeRequired)
            {
                _statusLabel.Invoke(new Action<object, string>(OnApiStatusChanged), sender, status);
                return;
            }

            // Sadece genel durum mesajlarını göster
            if (_statusLabel != null && (status.Contains("Bağlantı durumu:") || status.Contains("API Hatası:")))
            {
                _statusLabel.Text = status;
                if (status.Contains("Çevrimiçi"))
                    _statusLabel.ForeColor = Color.DarkGreen;
                else if (status.Contains("Çevrimdışı"))
                    _statusLabel.ForeColor = Color.Orange;
                else if (status.Contains("Hata"))
                    _statusLabel.ForeColor = Color.DarkRed;
            }
        }
    }
}
