// IhtiyaclarForm.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using berberdenemeson; // ApiService ve NeedModel için
using System.Globalization; // CultureInfo için

namespace berberdenemeson
{
    public partial class IhtiyaclarForm : Form
    {
        private readonly ApiService _apiService;
        private Timer _refreshTimer;
        private Timer _connectionStatusTimer; // Bağlantı durumu için timer
        private List<ApiService.NeedModel> _allNeeds; // ApiService.NeedModel olarak güncellendi
        private Form _modalForm; // Modal formu yönetmek için
        private bool _isLoading = false;

        public IhtiyaclarForm()
        {
            InitializeComponent();
            dgvIhtiyaclar.Visible = true;
            dgvIhtiyaclar.BringToFront();
            _apiService = ApiService.Instance;
            _allNeeds = new List<ApiService.NeedModel>();
            
            Console.WriteLine("IhtiyaclarForm constructor başladı");
            
            // Designer'daki butonların click eventlerini bağla
            btnYeniIhtiyac.Click += BtnYeniIhtiyac_Click; // Eğer farklı bir buton adı varsa düzeltin
            btnDuzenle.Click += BtnDuzenle_Click;
            btnSil.Click += BtnSil_Click;
            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click; // Genellikle yeni ekleme butonu
            btnRefresh.Click += BtnRefresh_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            
            // Grid event'leri
            dgvIhtiyaclar.CellClick += NeedsGrid_CellClick;
            
            // Form yüklendiğinde verileri otomatik yükle
            this.Load += IhtiyaclarForm_Load;
            StartAutoRefresh();
            StartConnectionStatusTimer(); // Bağlantı kontrol timer'ını başlat
            SetupSearchTextBoxPlaceholder();
        }

        private async void IhtiyaclarForm_Load(object sender, EventArgs e)
        {
            await LoadNeedsAsync();
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000; // Her 30 saniyede bir yenile
            _refreshTimer.Tick += (s, e) => Task.Run(async () => await LoadNeedsAsync());
            _refreshTimer.Start();
        }

        private void StartConnectionStatusTimer()
        {
            // _statusLabel Designer.cs'de tanımlı olduğundan emin olun.
            if (_statusLabel != null)
            {
                _connectionStatusTimer = new Timer();
                _connectionStatusTimer.Interval = 5000;
                _connectionStatusTimer.Tick += (s, e) => Task.Run(async () => await CheckConnectionAsync(_statusLabel));
                _connectionStatusTimer.Start();
                Task.Run(async () => await CheckConnectionAsync(_statusLabel)); // İlk kontrol
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

        private async Task LoadNeedsAsync()
        {
            if (_isLoading) return; // Zaten yükleniyorsa tekrar yükleme

            _isLoading = true;
            try
            {
                Console.WriteLine("DEBUG: IhtiyaclarForm.LoadNeedsAsync - Başladı");
                
                if (_statusLabel != null)
                {
                    _statusLabel.Text = "İhtiyaçlar yükleniyor...";
                    _statusLabel.ForeColor = Color.DarkBlue;
                }

                Console.WriteLine("DEBUG: IhtiyaclarForm.LoadNeedsAsync - API'den veri çekiliyor...");
                _allNeeds = await _apiService.GetNeedsAsync();
                Console.WriteLine($"DEBUG: IhtiyaclarForm.LoadNeedsAsync - API'den {_allNeeds?.Count ?? 0} veri geldi");
                
                // UI thread'inde DataGridView'i güncelle
                if (dgvIhtiyaclar.InvokeRequired)
                {
                    dgvIhtiyaclar.Invoke(new Action(() => {
                        FilterNeeds();
                        Console.WriteLine("DEBUG: IhtiyaclarForm.LoadNeedsAsync - FilterNeeds Invoke ile çağrıldı");
                    }));
                }
                else
                {
                    FilterNeeds();
                    Console.WriteLine("DEBUG: IhtiyaclarForm.LoadNeedsAsync - FilterNeeds direkt çağrıldı");
                }

                if (_statusLabel != null)
                {
                    _statusLabel.Text = $"Toplam {_allNeeds.Count} ihtiyaç listelendi.";
                    _statusLabel.ForeColor = Color.DarkGreen;
                }
                
                Console.WriteLine("DEBUG: IhtiyaclarForm.LoadNeedsAsync - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: IhtiyaclarForm.LoadNeedsAsync - Hata: {ex.Message}");
                
                if (_statusLabel != null)
                {
                    _statusLabel.Text = "İhtiyaçlar yüklenemedi: " + ex.Message;
                    _statusLabel.ForeColor = Color.DarkRed;
                }
                MessageBox.Show("İhtiyaçlar yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void FilterNeeds()
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();

                // Placeholder metni kontrol et
                if (searchText == "ihtiyaç ara...")
                {
                    searchText = "";
                }

                var filteredNeeds = _allNeeds.Where(n =>
                    (string.IsNullOrEmpty(searchText) ||
                     (n.IhtiyacTuru?.ToLower().Contains(searchText) ?? false))
                ).ToList();

                // DataGridView'i temizle ve yeniden yapılandır
                dgvIhtiyaclar.DataSource = null;
                dgvIhtiyaclar.Columns.Clear();

                dgvIhtiyaclar.AutoGenerateColumns = false;

                // Kolonları ekle
                dgvIhtiyaclar.Columns.Add(new DataGridViewTextBoxColumn { Name = "IhtiyacTuru", HeaderText = "İhtiyaç Türü", DataPropertyName = "IhtiyacTuru", ReadOnly = true });
                dgvIhtiyaclar.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fiyat", HeaderText = "Fiyat", DataPropertyName = "Fiyat", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });
                dgvIhtiyaclar.Columns.Add(new DataGridViewTextBoxColumn { Name = "Aciklama", HeaderText = "Açıklama", DataPropertyName = "Aciklama", ReadOnly = true });
                dgvIhtiyaclar.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Kayıt Tarihi", DataPropertyName = "CreatedAt", ReadOnly = true, DefaultCellStyle = { Format = "g", NullValue = "Yok" } });

                // Veriyi bağla
                dgvIhtiyaclar.DataSource = filteredNeeds;
                
                // Debug için satır sayısını logla
                Console.WriteLine($"DEBUG: IhtiyaclarForm - {filteredNeeds.Count} ihtiyaç filtrelendi ve DataGridView'e bağlandı");
                
                dgvIhtiyaclar.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: IhtiyaclarForm.FilterNeeds - Hata: {ex.Message}");
                MessageBox.Show($"İhtiyaç listesi güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NeedsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var selectedNeed = dgvIhtiyaclar.Rows[e.RowIndex].DataBoundItem as ApiService.NeedModel;
            if (selectedNeed == null) return;
        }

        private void ShowNeedModal(ApiService.NeedModel need = null)
        {
            _modalForm = new Form
            {
                Text = need == null ? "Yeni İhtiyaç Ekle" : "İhtiyaç Düzenle",
                Size = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var txtAd = new TextBox { Name = "txtAd", Left = 20, Top = 20, Width = 300 };
            var txtMiktar = new TextBox { Name = "txtMiktar", Left = 20, Top = 60, Width = 300 };
            txtMiktar.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            var txtBirimFiyat = new TextBox { Name = "txtBirimFiyat", Left = 20, Top = 100, Width = 300 };
            txtBirimFiyat.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == ',') && ((s as TextBox).Text.IndexOf(',') > -1))
                {
                    e.Handled = true;
                }
            };

            var btnSave = new Button { Text = "Kaydet", Left = 20, Top = 160, Width = 100, Height = 30, DialogResult = DialogResult.OK };
            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtAd.Text) || !int.TryParse(txtMiktar.Text, out int miktar) || !decimal.TryParse(txtBirimFiyat.Text, out decimal birimFiyat))
                    {
                        MessageBox.Show("Lütfen tüm alanları doğru doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _modalForm.DialogResult = DialogResult.None;
                        return;
                    }

                    var newNeed = new ApiService.NeedModel
                    {
                        IhtiyacTuru = txtAd.Text,
                        Fiyat = birimFiyat,
                        Aciklama = ""
                    };

                    if (need == null) // Yeni ihtiyaç
                    {
                        await _apiService.AddNeedAsync(newNeed);
                    }
                    else // İhtiyaç düzenleme
                    {
                        newNeed.IhtiyacID = need.IhtiyacID;
                        await _apiService.UpdateNeedAsync(need.IhtiyacID, newNeed);
                    }
                    MessageBox.Show("İhtiyaç başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadNeedsAsync(); // Listeyi güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show("İhtiyaç kaydedilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _modalForm.DialogResult = DialogResult.None;
                }
            };

            // Başlangıç değerlerini ata (düzenleme modunda)
            if (need != null)
            {
                txtAd.Text = need.IhtiyacTuru;
                txtMiktar.Text = "1"; // Varsayılan değer
                txtBirimFiyat.Text = need.Fiyat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            _modalForm.Controls.Add(new Label { Text = "Ad:", Left = 20, Top = 5, AutoSize = true });
            _modalForm.Controls.Add(txtAd);
            _modalForm.Controls.Add(new Label { Text = "Miktar:", Left = 20, Top = 45, AutoSize = true });
            _modalForm.Controls.Add(txtMiktar);
            _modalForm.Controls.Add(new Label { Text = "Birim Fiyat:", Left = 20, Top = 85, AutoSize = true });
            _modalForm.Controls.Add(txtBirimFiyat);
            _modalForm.Controls.Add(btnSave);

            _modalForm.ShowDialog(this);
        }

        private void BtnYeniIhtiyac_Click(object sender, EventArgs e)
        {
            ShowNeedModal(null);
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvIhtiyaclar.SelectedRows.Count > 0)
            {
                var selectedNeed = dgvIhtiyaclar.SelectedRows[0].DataBoundItem as ApiService.NeedModel;
                if (selectedNeed != null)
                {
                    ShowNeedModal(selectedNeed);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir ihtiyaç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvIhtiyaclar.SelectedRows.Count > 0)
            {
                var selectedNeed = dgvIhtiyaclar.SelectedRows[0].DataBoundItem as ApiService.NeedModel;
                if (selectedNeed != null)
                {
                    if (MessageBox.Show($"'{selectedNeed.IhtiyacTuru}' adlı ihtiyacı silmek istediğinizden emin misiniz?", "İhtiyaç Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            bool success = await _apiService.DeleteNeedAsync(selectedNeed.IhtiyacID);
                            if (success)
                            {
                                MessageBox.Show("İhtiyaç başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                await LoadNeedsAsync();
                            }
                            else
                            {
                                MessageBox.Show("İhtiyaç silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("İhtiyaç silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir ihtiyaç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Arama ve Yenileme Butonları ---
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterNeeds();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowNeedModal(null);
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadNeedsAsync();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterNeeds();
        }

        // txtSearch için Placeholder metin yönetimi
        private void SetupSearchTextBoxPlaceholder()
        {
            txtSearch.Text = "İhtiyaç ara...";
            txtSearch.ForeColor = SystemColors.GrayText;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "İhtiyaç ara...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "İhtiyaç ara...";
                txtSearch.ForeColor = SystemColors.GrayText;
            }
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
                Console.WriteLine($"IhtiyaclarForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }
    }
}