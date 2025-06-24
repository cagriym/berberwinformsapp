using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization; // CultureInfo için
using berberdenemeson; // ApiService ve modeller için

namespace berberdenemeson
{
    public partial class PersonelForm : Form
    {
        private Timer _refreshTimer;
        private Timer _connectionStatusTimer; // Bağlantı durumu için timer
        private List<ApiService.PersonnelModel> _allPersonnel;
        private Form _modalForm;
        private ApiService _apiService;

        public PersonelForm()
        {
            InitializeComponent();
            dgvPersonel.Visible = true;
            dgvPersonel.BringToFront();
            _apiService = ApiService.Instance;
            _allPersonnel = new List<ApiService.PersonnelModel>();

            btnAdd.Click += BtnAdd_Click;
            btnDuzenle.Click += BtnDuzenle_Click;
            btnSil.Click += BtnSil_Click;
            btnSearch.Click += BtnSearch_Click;
            btnRefresh.Click += BtnRefresh_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            dgvPersonel.CellClick += PersonnelGrid_CellClick;

            this.Load += PersonelForm_Load;
            StartAutoRefresh();
            StartConnectionStatusTimer(); // Bağlantı kontrol timer'ını başlat
            SetupSearchTextBoxPlaceholder();
        }

        private async void PersonelForm_Load(object sender, EventArgs e)
        {
            await LoadPersonnel();
        }

        private void StartAutoRefresh()
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            }
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000; // Her 30 saniyede bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadPersonnel();
            _refreshTimer.Start();
        }

        private void StartConnectionStatusTimer()
        {
            if (_statusLabel != null) // _statusLabel Designer'da tanımlı olduğundan emin olun.
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

        private async Task LoadPersonnel()
        {
            try
            {
                Console.WriteLine("DEBUG: PersonelForm.LoadPersonnel - Başladı");
                
                if (_statusLabel != null)
                {
                    _statusLabel.Text = "Personel yükleniyor...";
                    _statusLabel.ForeColor = Color.DarkBlue;
                }

                _allPersonnel = await _apiService.GetPersonnelAsync();
                Console.WriteLine($"DEBUG: PersonelForm.LoadPersonnel - API'den {_allPersonnel?.Count ?? 0} personel geldi");
                
                // UI thread'inde DataGridView'i güncelle
                if (dgvPersonel.InvokeRequired)
                {
                    dgvPersonel.Invoke(new Action(() => {
                        FilterPersonnelList();
                        Console.WriteLine("DEBUG: PersonelForm.LoadPersonnel - FilterPersonnelList Invoke ile çağrıldı");
                    }));
                }
                else
                {
                    FilterPersonnelList();
                    Console.WriteLine("DEBUG: PersonelForm.LoadPersonnel - FilterPersonnelList direkt çağrıldı");
                }

                if (_statusLabel != null)
                {
                    _statusLabel.Text = $"Toplam {_allPersonnel.Count} personel listelendi.";
                    _statusLabel.ForeColor = Color.DarkGreen;
                }
                
                Console.WriteLine("DEBUG: PersonelForm.LoadPersonnel - Tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: PersonelForm.LoadPersonnel - Hata: {ex.Message}");
                
                if (_statusLabel != null)
                {
                    _statusLabel.Text = "Personel yüklenemedi: " + ex.Message;
                    _statusLabel.ForeColor = Color.DarkRed;
                }
                MessageBox.Show("Personel yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterPersonnelList()
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();

                // Placeholder metni kontrol et
                if (searchText == "personel ara...")
                {
                    searchText = "";
                }

                var filteredPersonnel = _allPersonnel.Where(p =>
                    (string.IsNullOrEmpty(searchText) ||
                     (p.AdSoyad?.ToLower().Contains(searchText) ?? false) ||
                     (p.Pozisyon?.ToLower().Contains(searchText) ?? false) ||
                     (p.Telefon?.ToLower().Contains(searchText) ?? false))
                ).ToList();

                // DataGridView'i temizle ve yeniden yapılandır
                dgvPersonel.DataSource = null;
                dgvPersonel.Columns.Clear();

                dgvPersonel.AutoGenerateColumns = false;

                // Kolonları ekle
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "AdSoyad", HeaderText = "Ad Soyad", DataPropertyName = "AdSoyad", ReadOnly = true });
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "Pozisyon", HeaderText = "Pozisyon", DataPropertyName = "Pozisyon", ReadOnly = true });
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefon", HeaderText = "Telefon", DataPropertyName = "Telefon", ReadOnly = true });
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "E-posta", DataPropertyName = "Email", ReadOnly = true });
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "Maas", HeaderText = "Maaş", DataPropertyName = "Maas", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR"), NullValue = "0" } });
                dgvPersonel.Columns.Add(new DataGridViewTextBoxColumn { Name = "IseGirisTarihi", HeaderText = "İşe Giriş", DataPropertyName = "IseGirisTarihi", ReadOnly = true, DefaultCellStyle = { Format = "d", NullValue = "Yok" } });
                dgvPersonel.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Aktif", HeaderText = "Aktif", DataPropertyName = "Aktif", ReadOnly = true });

                // Veriyi bağla
                dgvPersonel.DataSource = filteredPersonnel;
                
                // Debug için satır sayısını logla
                Console.WriteLine($"DEBUG: PersonelForm - {filteredPersonnel.Count} personel filtrelendi ve DataGridView'e bağlandı");
                
                dgvPersonel.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: PersonelForm.FilterPersonnelList - Hata: {ex.Message}");
                MessageBox.Show($"Personel listesi güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonnelGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var selectedPersonnel = dgvPersonel.Rows[e.RowIndex].DataBoundItem as ApiService.PersonnelModel;
            if (selectedPersonnel == null) return;
        }

        private void ShowPersonnelModal(ApiService.PersonnelModel personnel = null)
        {
            _modalForm = new Form
            {
                Text = personnel == null ? "Yeni Personel Ekle" : "Personel Düzenle",
                Size = new Size(400, 600),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var txtAd = new TextBox { Name = "txtAd", Left = 20, Top = 20, Width = 300 };
            var txtSoyad = new TextBox { Name = "txtSoyad", Left = 20, Top = 60, Width = 300 };
            var txtTelefon = new TextBox { Name = "txtTelefon", Left = 20, Top = 100, Width = 300 };
            var txtEmail = new TextBox { Name = "txtEmail", Left = 20, Top = 140, Width = 300 };
            var txtPozisyon = new TextBox { Name = "txtPozisyon", Left = 20, Top = 180, Width = 300 };
            var txtMaas = new TextBox { Name = "txtMaas", Left = 20, Top = 220, Width = 300 };
            txtMaas.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == ',') && ((s as TextBox).Text.IndexOf(',') > -1))
                {
                    e.Handled = true;
                }
            };

            var dtpIseGirisTarihi = new DateTimePicker { Name = "dtpIseGirisTarihi", Left = 20, Top = 260, Width = 300, Format = DateTimePickerFormat.Short };
            var dtpCikisTarihi = new DateTimePicker { Name = "dtpCikisTarihi", Left = 20, Top = 300, Width = 300, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false };
            var chkAktif = new CheckBox { Name = "chkAktif", Text = "Aktif", Left = 20, Top = 340, AutoSize = true };
            var txtAciklama = new TextBox { Name = "txtAciklama", Left = 20, Top = 380, Width = 300, Multiline = true, Height = 80 };


            var btnSave = new Button { Text = "Kaydet", Left = 20, Top = 480, Width = 100, Height = 30, DialogResult = DialogResult.OK };
            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (!decimal.TryParse(txtMaas.Text, NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal maas))
                    {
                        maas = 0; // Varsayılan değer veya hata yönetimi
                    }

                    var newPersonnel = new ApiService.PersonnelModel
                    {
                        AdSoyad = $"{txtAd.Text} {txtSoyad.Text}".Trim(),
                        Telefon = txtTelefon.Text,
                        Email = txtEmail.Text,
                        Pozisyon = txtPozisyon.Text,
                        Maas = maas,
                        IseGirisTarihi = dtpIseGirisTarihi.Value,
                        Aktif = chkAktif.Checked,
                        Aciklama = txtAciklama.Text
                    };

                    if (personnel == null) // Yeni personel
                    {
                        await _apiService.AddPersonnelAsync(newPersonnel);
                    }
                    else // Personel düzenleme
                    {
                        newPersonnel.PersonelID = personnel.PersonelID;
                        await _apiService.UpdatePersonnelAsync(personnel.PersonelID.Value, newPersonnel);
                    }
                    MessageBox.Show("Personel başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadPersonnel(); // Listeyi güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Personel kaydedilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _modalForm.DialogResult = DialogResult.None; // Hata olursa modalı kapatma
                }
            };

            // Başlangıç değerlerini ata (düzenleme modunda)
            if (personnel != null)
            {
                // AdSoyad'ı Ad ve Soyad'a ayır
                var adSoyadParts = personnel.AdSoyad?.Split(new char[] { ' ' }, 2) ?? new string[0];
                txtAd.Text = adSoyadParts.Length > 0 ? adSoyadParts[0] : "";
                txtSoyad.Text = adSoyadParts.Length > 1 ? adSoyadParts[1] : "";
                txtTelefon.Text = personnel.Telefon;
                txtEmail.Text = personnel.Email;
                txtPozisyon.Text = personnel.Pozisyon;
                txtMaas.Text = personnel.Maas?.ToString(new CultureInfo("tr-TR")) ?? "";
                if (personnel.IseGirisTarihi.HasValue) dtpIseGirisTarihi.Value = personnel.IseGirisTarihi.Value;
                if (personnel.CikisTarihi.HasValue)
                {
                    dtpCikisTarihi.Value = personnel.CikisTarihi.Value;
                    dtpCikisTarihi.Checked = true;
                }
                else
                {
                    dtpCikisTarihi.Checked = false;
                }
                chkAktif.Checked = personnel.Aktif ?? false; // Nullable bool için varsayılan değer
                txtAciklama.Text = personnel.Aciklama;
            }

            _modalForm.Controls.Add(new Label { Text = "Ad:", Left = 20, Top = 5, AutoSize = true });
            _modalForm.Controls.Add(txtAd);
            _modalForm.Controls.Add(new Label { Text = "Soyad:", Left = 20, Top = 45, AutoSize = true });
            _modalForm.Controls.Add(txtSoyad);
            _modalForm.Controls.Add(new Label { Text = "Telefon:", Left = 20, Top = 85, AutoSize = true });
            _modalForm.Controls.Add(txtTelefon);
            _modalForm.Controls.Add(new Label { Text = "E-posta:", Left = 20, Top = 125, AutoSize = true });
            _modalForm.Controls.Add(txtEmail);
            _modalForm.Controls.Add(new Label { Text = "Pozisyon:", Left = 20, Top = 165, AutoSize = true });
            _modalForm.Controls.Add(txtPozisyon);
            _modalForm.Controls.Add(new Label { Text = "Maaş:", Left = 20, Top = 205, AutoSize = true });
            _modalForm.Controls.Add(txtMaas);
            _modalForm.Controls.Add(new Label { Text = "İşe Giriş Tarihi:", Left = 20, Top = 245, AutoSize = true });
            _modalForm.Controls.Add(dtpIseGirisTarihi);
            _modalForm.Controls.Add(new Label { Text = "Çıkış Tarihi:", Left = 20, Top = 285, AutoSize = true });
            _modalForm.Controls.Add(dtpCikisTarihi);
            _modalForm.Controls.Add(chkAktif);
            _modalForm.Controls.Add(new Label { Text = "Açıklama:", Left = 20, Top = 365, AutoSize = true });
            _modalForm.Controls.Add(txtAciklama);
            _modalForm.Controls.Add(btnSave);

            _modalForm.ShowDialog(this);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowPersonnelModal(null);
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvPersonel.SelectedRows.Count > 0)
            {
                var selectedPersonnel = dgvPersonel.SelectedRows[0].DataBoundItem as ApiService.PersonnelModel;
                if (selectedPersonnel != null)
                {
                    ShowPersonnelModal(selectedPersonnel);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvPersonel.SelectedRows.Count > 0)
            {
                var selectedPersonnel = dgvPersonel.SelectedRows[0].DataBoundItem as ApiService.PersonnelModel;
                if (selectedPersonnel != null && selectedPersonnel.PersonelID.HasValue)
                {
                    if (MessageBox.Show($"'{selectedPersonnel.AdSoyad}' adlı personeli silmek istediğinizden emin misiniz?", "Personel Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            bool success = await _apiService.DeletePersonnelAsync(selectedPersonnel.PersonelID.Value);
                            if (success)
                            {
                                MessageBox.Show("Personel başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                await LoadPersonnel();
                            }
                            else
                            {
                                MessageBox.Show("Personel silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Personel silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterPersonnelList();
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadPersonnel();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterPersonnelList();
        }

        // txtSearch için Placeholder metin yönetimi
        private void SetupSearchTextBoxPlaceholder()
        {
            txtSearch.Text = "Personel ara...";
            txtSearch.ForeColor = SystemColors.GrayText;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Personel ara...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Personel ara...";
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
                Console.WriteLine($"PersonelForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }
    }
}
