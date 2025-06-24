using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace berberdenemeson
{
    public partial class ServislerForm : Form
    {
        private readonly ApiService _apiService;
        private List<ApiService.ServiceModel> _allServices;

        public ServislerForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            _allServices = new List<ApiService.ServiceModel>();
            this.Load += ServislerForm_Load;
            
            // Yeni butonlar için event handler'ları ekle
            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click;
            btnRefresh.Click += BtnRefresh_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            
            // Form panel butonları için event handler'ları
            btnKaydet.Click += BtnKaydet_Click;
            btnIptal.Click += BtnIptal_Click;
            
            // Placeholder metin yönetimi
            SetupSearchTextBoxPlaceholder();
        }

        private async void ServislerForm_Load(object sender, EventArgs e)
        {
            await LoadServicesAsync();
        }

        private async Task LoadServicesAsync()
        {
            try
            {
                _allServices = await _apiService.GetServicesAsync();
                dgvServices.DataSource = null;
                dgvServices.Columns.Clear();
                dgvServices.AutoGenerateColumns = false;
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisID", HeaderText = "ID", DataPropertyName = "ServisID", ReadOnly = true });
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisAdi", HeaderText = "Servis Adı", DataPropertyName = "ServisAdi", ReadOnly = true });
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "VarsayilanUcret", HeaderText = "Varsayılan Ücret", DataPropertyName = "VarsayilanUcret", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });
                dgvServices.DataSource = _allServices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Servisler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string servisAdi = txtServisAdi.Text.Trim();
                if (string.IsNullOrEmpty(servisAdi))
                {
                    MessageBox.Show("Servis adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(txtVarsayilanUcret.Text, NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal ucret))
                {
                    MessageBox.Show("Geçerli bir ücret giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var newService = new ApiService.ServiceModel { ServisAdi = servisAdi, VarsayilanUcret = ucret };
                await _apiService.AddServiceAsync(newService);
                await LoadServicesAsync();
                txtServisAdi.Text = "";
                txtVarsayilanUcret.Text = "";
                
                // Form panelini gizle
                panelForm.Visible = false;
                panelMain.Width = 760;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Servis eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvServices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir servis seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selected = dgvServices.SelectedRows[0].DataBoundItem as ApiService.ServiceModel;
            if (selected == null) return;
            if (MessageBox.Show($"'{selected.ServisAdi}' servisini silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _apiService.DeleteServiceAsync(selected.ServisID);
                    await LoadServicesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Servis silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvServices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir servis seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selected = dgvServices.SelectedRows[0].DataBoundItem as ApiService.ServiceModel;
            if (selected == null) return;
            
            ShowServiceModal(selected);
        }

        private void dgvServices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvServices.SelectedRows.Count == 0) return;
            var selected = dgvServices.SelectedRows[0].DataBoundItem as ApiService.ServiceModel;
            if (selected == null) return;
            txtServisAdi.Text = selected.ServisAdi;
            txtVarsayilanUcret.Text = selected.VarsayilanUcret?.ToString("F2", new CultureInfo("tr-TR")) ?? "";
        }

        // --- Arama ve Yenileme Butonları ---
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterServices();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowServiceModal(null);
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadServicesAsync();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterServices();
        }

        // txtSearch için Placeholder metin yönetimi
        private void SetupSearchTextBoxPlaceholder()
        {
            txtSearch.Text = "Servis ara...";
            txtSearch.ForeColor = SystemColors.GrayText;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Servis ara...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Servis ara...";
                txtSearch.ForeColor = SystemColors.GrayText;
            }
        }

        private void FilterServices()
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();

                // Placeholder metni kontrol et
                if (searchText == "servis ara...")
                {
                    searchText = "";
                }

                var filteredServices = _allServices.Where(s =>
                    (string.IsNullOrEmpty(searchText) ||
                     (s.ServisAdi?.ToLower().Contains(searchText) ?? false))
                ).ToList();

                // DataGridView'i temizle ve yeniden yapılandır
                dgvServices.DataSource = null;
                dgvServices.Columns.Clear();

                dgvServices.AutoGenerateColumns = false;

                // Kolonları ekle
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisID", HeaderText = "ID", DataPropertyName = "ServisID", ReadOnly = true });
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServisAdi", HeaderText = "Servis Adı", DataPropertyName = "ServisAdi", ReadOnly = true });
                dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "VarsayilanUcret", HeaderText = "Varsayılan Ücret", DataPropertyName = "VarsayilanUcret", ReadOnly = true, DefaultCellStyle = { Format = "C2", FormatProvider = new CultureInfo("tr-TR") } });

                // Veriyi bağla
                dgvServices.DataSource = filteredServices;
                
                dgvServices.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Servis listesi güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowServiceModal(ApiService.ServiceModel service = null)
        {
            // Modal form yerine panelForm'u göster/gizle
            if (service == null)
            {
                // Yeni servis ekleme modu
                txtServisAdi.Text = "";
                txtVarsayilanUcret.Text = "";
                panelForm.Visible = true;
                panelForm.Width = 300;
                panelMain.Width = 460;
            }
            else
            {
                // Servis düzenleme modu
                txtServisAdi.Text = service.ServisAdi;
                txtVarsayilanUcret.Text = service.VarsayilanUcret?.ToString("F2", new CultureInfo("tr-TR")) ?? "";
                panelForm.Visible = true;
                panelForm.Width = 300;
                panelMain.Width = 460;
            }
        }

        private async void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                string servisAdi = txtServisAdi.Text.Trim();
                if (string.IsNullOrEmpty(servisAdi))
                {
                    MessageBox.Show("Servis adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(txtVarsayilanUcret.Text, NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal ucret))
                {
                    MessageBox.Show("Geçerli bir ücret giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Seçili servis varsa güncelle, yoksa yeni ekle
                if (dgvServices.SelectedRows.Count > 0)
                {
                    var selected = dgvServices.SelectedRows[0].DataBoundItem as ApiService.ServiceModel;
                    if (selected != null)
                    {
                        selected.ServisAdi = servisAdi;
                        selected.VarsayilanUcret = ucret;
                        await _apiService.UpdateServiceAsync(selected.ServisID, selected);
                    }
                }
                else
                {
                    var newService = new ApiService.ServiceModel { ServisAdi = servisAdi, VarsayilanUcret = ucret };
                    await _apiService.AddServiceAsync(newService);
                }

                await LoadServicesAsync();
                
                // Form panelini gizle
                panelForm.Visible = false;
                panelMain.Width = 760;
                
                MessageBox.Show("Servis başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Servis kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            // Form panelini gizle
            panelForm.Visible = false;
            panelMain.Width = 760;
        }
    }
} 