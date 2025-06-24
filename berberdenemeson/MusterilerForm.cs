using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http; // Kaldırılabilir eğer sadece ApiService kullanılıyorsa
using Newtonsoft.Json;
using berberdenemeson; // ApiService ve modeller için

namespace berberdenemeson
{
    public partial class MusterilerForm : Form
    {
        private readonly ApiService _apiService;
        private Timer _refreshTimer;
        private Timer _connectionStatusTimer; // Bağlantı durumu için timer
        private List<ApiService.CustomerModel> _allCustomers; // ApiService.CustomerModel olarak güncellendi
        // private HttpClient _httpClient; // Kaldırıldı
        private Form _modalForm; // Modal formu yönetmek için

        public MusterilerForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            
            // ApiService event'lerini dinle
            _apiService.ErrorOccurred += OnApiError;
            _apiService.StatusChanged += OnApiStatusChanged;
            
            // _httpClient = new HttpClient(); // Kaldırıldı
            // _httpClient.BaseAddress = new Uri("https://oktay-sac-tasarim1.azurewebsites.net/api/"); // Kaldırıldı
            _allCustomers = new List<ApiService.CustomerModel>();

            // Designer'daki butonların click eventlerini bağla
            btnYeniMusteri.Click += BtnYeniMusteri_Click; // Eğer farklı bir buton adı varsa düzeltin
            btnDuzenle.Click += BtnDuzenle_Click;
            btnSil.Click += BtnSil_Click;
            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click; // Genellikle yeni ekleme butonu
            btnRefresh.Click += BtnRefresh_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Grid event'leri
            dgvMusteriler.CellClick += CustomersGrid_CellClick;

            this.Load += MusterilerForm_Load;
            StartAutoRefresh();
            StartConnectionStatusTimer(); // Bağlantı kontrol timer'ını başlat
            SetupSearchTextBoxPlaceholder();

            // Başlık
            // lblBaslik.Font = new Font("Segoe UI", 18, FontStyle.Bold); // Zaten designer'da ayarlanmış olabilir.
        }

        private async void MusterilerForm_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000; // Her 30 saniyede bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadCustomersAsync();
            _refreshTimer.Start();
        }

        private void StartConnectionStatusTimer()
        {
            // _statusLabel Designer.cs'de tanımlı olduğundan emin olun.
            if (_statusLabel != null)
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

        private async Task LoadCustomersAsync()
        {
            try
            {
                Console.WriteLine("DEBUG: MusterilerForm.LoadCustomersAsync - Başladı");
                
                // UI thread'inde güvenli güncelleme
                if (_statusLabel != null)
                {
                    if (_statusLabel.InvokeRequired)
                    {
                        _statusLabel.Invoke(new Action(() => {
                            _statusLabel.Text = "Müşteriler yükleniyor...";
                            _statusLabel.ForeColor = Color.DarkBlue;
                        }));
                    }
                    else
                    {
                        _statusLabel.Text = "Müşteriler yükleniyor...";
                        _statusLabel.ForeColor = Color.DarkBlue;
                    }
                }

                // API'den müşterileri çek
                _allCustomers = await _apiService.GetCustomersAsync();
                Console.WriteLine($"DEBUG: MusterilerForm.LoadCustomersAsync - API'den {_allCustomers?.Count ?? 0} müşteri geldi");
                
                // UI thread'inde DataGridView'i güncelle
                if (dgvMusteriler.InvokeRequired)
                {
                    dgvMusteriler.Invoke(new Action(() => {
                        FilterCustomers();
                        Console.WriteLine("DEBUG: MusterilerForm.LoadCustomersAsync - FilterCustomers Invoke ile çağrıldı");
                    }));
                }
                else
                {
                    FilterCustomers();
                    Console.WriteLine("DEBUG: MusterilerForm.LoadCustomersAsync - FilterCustomers direkt çağrıldı");
                }

                // Başarı mesajını göster
                if (_statusLabel != null)
                {
                    if (_statusLabel.InvokeRequired)
                    {
                        _statusLabel.Invoke(new Action<int>(count => {
                            _statusLabel.Text = $"Toplam {count} müşteri listelendi.";
                            _statusLabel.ForeColor = Color.DarkGreen;
                        }), _allCustomers.Count);
                    }
                    else
                    {
                        _statusLabel.Text = $"Toplam {_allCustomers.Count} müşteri listelendi.";
                        _statusLabel.ForeColor = Color.DarkGreen;
                    }
                }
                
                Console.WriteLine("DEBUG: MusterilerForm.LoadCustomersAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: MusterilerForm.LoadCustomersAsync - Hata: {ex.Message}");
                
                // Hata mesajını UI thread'inde göster
                if (_statusLabel != null)
                {
                    if (_statusLabel.InvokeRequired)
                    {
                        _statusLabel.Invoke(new Action<string>(error => {
                            _statusLabel.Text = "Müşteriler yüklenemedi: " + error;
                            _statusLabel.ForeColor = Color.DarkRed;
                        }), ex.Message);
                    }
                    else
                    {
                        _statusLabel.Text = "Müşteriler yüklenemedi: " + ex.Message;
                        _statusLabel.ForeColor = Color.DarkRed;
                    }
                }

                // Hata dialog'unu UI thread'inde göster
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(error => {
                        MessageBox.Show("Müşteriler yüklenirken hata oluştu: " + error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), ex.Message);
                }
                else
                {
                    MessageBox.Show("Müşteriler yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FilterCustomers()
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();

                // Placeholder metni kontrol et
                if (searchText == "müşteri ara...")
                {
                    searchText = "";
                }

                var filteredCustomers = _allCustomers.Where(c =>
                    (string.IsNullOrEmpty(searchText) ||
                     (c.AdSoyad?.ToLower().Contains(searchText) ?? false) ||
                     (c.Telefon?.ToLower().Contains(searchText) ?? false))
                ).ToList();

                // DataGridView'i temizle ve yeniden yapılandır
                dgvMusteriler.DataSource = null;
                dgvMusteriler.Columns.Clear();

                dgvMusteriler.AutoGenerateColumns = false;

                // Kolonları ekle
                dgvMusteriler.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "AdSoyad",
                    HeaderText = "Ad Soyad",
                    DataPropertyName = "AdSoyad",
                    ReadOnly = true
                });
                dgvMusteriler.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefon",
                    HeaderText = "Telefon",
                    DataPropertyName = "Telefon",
                    ReadOnly = true
                });
                dgvMusteriler.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Email",
                    HeaderText = "E-posta",
                    DataPropertyName = "Email",
                    ReadOnly = true
                });
                dgvMusteriler.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Adres",
                    HeaderText = "Adres",
                    DataPropertyName = "Adres",
                    ReadOnly = true
                });
                dgvMusteriler.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CreatedAt",
                    HeaderText = "Kayıt Tarihi",
                    DataPropertyName = "CreatedAt",
                    ReadOnly = true,
                    DefaultCellStyle = { Format = "g", NullValue = "Yok" }
                });

                // Veriyi bağla
                dgvMusteriler.DataSource = filteredCustomers;
                
                // Debug için satır sayısını logla
                Console.WriteLine($"DEBUG: MusterilerForm - {filteredCustomers.Count} müşteri filtrelendi ve DataGridView'e bağlandı");
                
                dgvMusteriler.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: MusterilerForm.FilterCustomers - Hata: {ex.Message}");
                MessageBox.Show($"Müşteri listesi güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var selectedCustomer = dgvMusteriler.Rows[e.RowIndex].DataBoundItem as ApiService.CustomerModel;
            if (selectedCustomer == null) return;
        }

        private void ShowCustomerModal(ApiService.CustomerModel customer = null)
        {
            _modalForm = new Form
            {
                Text = customer == null ? "Yeni Müşteri Ekle" : "Müşteri Düzenle",
                Size = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var txtAd = new TextBox { Name = "txtAd", Left = 20, Top = 20, Width = 300 };
            var txtSoyad = new TextBox { Name = "txtSoyad", Left = 20, Top = 60, Width = 300 };
            var txtTelefon = new TextBox { Name = "txtTelefon", Left = 20, Top = 100, Width = 300 };
            var dtpSonGelisTarihi = new DateTimePicker { Name = "dtpSonGelisTarihi", Left = 20, Top = 140, Width = 300, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false };

            var btnSave = new Button { Text = "Kaydet", Left = 20, Top = 200, Width = 100, Height = 30, DialogResult = DialogResult.OK };
            btnSave.Click += async (s, e) =>
            {
                try
                {
                    var newCustomer = new ApiService.CustomerModel
                    {
                        AdSoyad = $"{txtAd.Text} {txtSoyad.Text}".Trim(),
                        Telefon = txtTelefon.Text
                    };

                    if (customer == null) // Yeni müşteri
                    {
                        await _apiService.AddCustomerAsync(newCustomer);
                    }
                    else // Müşteri düzenleme
                    {
                        newCustomer.MusteriID = customer.MusteriID;
                        await _apiService.UpdateCustomerAsync(customer.MusteriID, newCustomer);
                    }
                    MessageBox.Show("Müşteri başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadCustomersAsync(); // Listeyi güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Müşteri kaydedilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _modalForm.DialogResult = DialogResult.None;
                }
            };

            // Başlangıç değerlerini ata (düzenleme modunda)
            if (customer != null)
            {
                txtAd.Text = customer.Ad;
                txtSoyad.Text = customer.Soyad;
                txtTelefon.Text = customer.Telefon;
                if (customer.SonGelisTarihi.HasValue)
                {
                    dtpSonGelisTarihi.Value = customer.SonGelisTarihi.Value;
                    dtpSonGelisTarihi.Checked = true;
                }
                else
                {
                    dtpSonGelisTarihi.Checked = false;
                }
            }

            _modalForm.Controls.Add(new Label { Text = "Ad:", Left = 20, Top = 5, AutoSize = true });
            _modalForm.Controls.Add(txtAd);
            _modalForm.Controls.Add(new Label { Text = "Soyad:", Left = 20, Top = 45, AutoSize = true });
            _modalForm.Controls.Add(txtSoyad);
            _modalForm.Controls.Add(new Label { Text = "Telefon:", Left = 20, Top = 85, AutoSize = true });
            _modalForm.Controls.Add(txtTelefon);
            _modalForm.Controls.Add(new Label { Text = "Son Geliş Tarihi:", Left = 20, Top = 125, AutoSize = true });
            _modalForm.Controls.Add(dtpSonGelisTarihi);
            _modalForm.Controls.Add(btnSave);

            _modalForm.ShowDialog(this);
        }

        private void BtnYeniMusteri_Click(object sender, EventArgs e)
        {
            ShowCustomerModal(null);
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.SelectedRows.Count > 0)
            {
                var selectedCustomer = dgvMusteriler.SelectedRows[0].DataBoundItem as ApiService.CustomerModel;
                if (selectedCustomer != null)
                {
                    ShowCustomerModal(selectedCustomer);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.SelectedRows.Count > 0)
            {
                var selectedCustomer = dgvMusteriler.SelectedRows[0].DataBoundItem as ApiService.CustomerModel;
                if (selectedCustomer != null)
                {
                    if (MessageBox.Show($"'{selectedCustomer.AdSoyad}' adlı müşteriyi silmek istediğinizden emin misiniz?", "Müşteri Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            bool success = await _apiService.DeleteCustomerAsync(selectedCustomer.MusteriID);
                            if (success)
                            {
                                MessageBox.Show("Müşteri başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                await LoadCustomersAsync();
                            }
                            else
                            {
                                MessageBox.Show("Müşteri silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Müşteri silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Arama ve Yenileme Butonları ---
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterCustomers();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowCustomerModal(null);
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterCustomers();
        }

        // txtSearch için Placeholder metin yönetimi
        private void SetupSearchTextBoxPlaceholder()
        {
            txtSearch.Text = "Müşteri ara...";
            txtSearch.ForeColor = SystemColors.GrayText;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Müşteri ara...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Müşteri ara...";
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
                Console.WriteLine($"MusterilerForm kapatma hatası: {ex.Message}");
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