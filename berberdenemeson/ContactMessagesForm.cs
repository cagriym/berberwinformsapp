using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http; // Kaldırılabilir eğer sadece ApiService kullanılıyorsa
using Newtonsoft.Json;
using berberdenemeson; // ApiService ve modeller için

namespace berberdenemeson
{
    public partial class ContactMessagesForm : Form
    {
        // private readonly HttpClient _httpClient; // Kaldırıldı
        private readonly ApiService _apiService;
        private Timer _refreshTimer; // Otomatik yenileme için timer

        public ContactMessagesForm()
        {
            InitializeComponent();
            _apiService = ApiService.Instance;
            
            // ApiService event'lerini dinle
            _apiService.ErrorOccurred += OnApiError;
            _apiService.StatusChanged += OnApiStatusChanged;
            
            // _httpClient = new HttpClient(); // Kaldırıldı
            // _httpClient.BaseAddress = new Uri("https://oktay-sac-tasarim1.azurewebsites.net/api/"); // Kaldırıldı
            btnRefresh.Click += BtnRefresh_Click;
            this.Load += ContactMessagesForm_Load;
            StartAutoRefresh(); // Otomatik yenilemeyi başlat
        }

        private void OnApiError(object sender, string error)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<object, string>(OnApiError), sender, error);
                return;
            }

            lblStatus.Text = $"API Hatası: {error}";
            lblStatus.ForeColor = Color.DarkRed;
        }

        private void OnApiStatusChanged(object sender, string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<object, string>(OnApiStatusChanged), sender, status);
                return;
            }

            // Sadece genel durum mesajlarını göster, detaylı API mesajlarını gösterme
            if (status.Contains("Bağlantı durumu:") || status.Contains("API Hatası:"))
            {
                lblStatus.Text = status;
                if (status.Contains("Çevrimiçi"))
                    lblStatus.ForeColor = Color.DarkGreen;
                else if (status.Contains("Çevrimdışı"))
                    lblStatus.ForeColor = Color.Orange;
                else if (status.Contains("Hata"))
                    lblStatus.ForeColor = Color.DarkRed;
            }
        }

        private async void ContactMessagesForm_Load(object sender, EventArgs e)
        {
            await LoadMessagesAsync();
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 60000; // Her 1 dakikada bir yenile
            _refreshTimer.Tick += async (s, e) => await LoadMessagesAsync();
            _refreshTimer.Start();
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadMessagesAsync();
        }

        private async Task LoadMessagesAsync()
        {
            try
            {
                // UI thread'inde güvenli güncelleme
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new Action(() => {
                        lblStatus.Text = "Mesajlar yükleniyor...";
                        lblStatus.ForeColor = Color.DarkBlue;
                    }));
                }
                else
                {
                    lblStatus.Text = "Mesajlar yükleniyor...";
                    lblStatus.ForeColor = Color.DarkBlue;
                }

                // DataGridView'i temizle
                if (dgvMessages.InvokeRequired)
                {
                    dgvMessages.Invoke(new Action(() => {
                        dgvMessages.Rows.Clear();
                        dgvMessages.Columns.Clear();
                    }));
                }
                else
                {
                    dgvMessages.Rows.Clear();
                    dgvMessages.Columns.Clear();
                }

                // API'den mesajları çek
                var messages = await _apiService.GetContactMessagesAsync();

                // UI thread'inde DataGridView'i güncelle
                if (dgvMessages.InvokeRequired)
                {
                    dgvMessages.Invoke(new Action<List<ApiService.ContactMessageDto>>(UpdateDataGridView), messages);
                }
                else
                {
                    UpdateDataGridView(messages);
                }

                // Başarı mesajını göster
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new Action<int>(count => {
                        lblStatus.Text = $"Toplam {count} mesaj listelendi.";
                        lblStatus.ForeColor = Color.DarkGreen;
                    }), messages.Count);
                }
                else
                {
                    lblStatus.Text = $"Toplam {messages.Count} mesaj listelendi.";
                    lblStatus.ForeColor = Color.DarkGreen;
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını UI thread'inde göster
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new Action<string>(error => {
                        lblStatus.Text = "Mesajlar yüklenemedi: " + error;
                        lblStatus.ForeColor = Color.DarkRed;
                    }), ex.Message);
                }
                else
                {
                    lblStatus.Text = "Mesajlar yüklenemedi: " + ex.Message;
                    lblStatus.ForeColor = Color.DarkRed;
                }

                // Hata dialog'unu UI thread'inde göster
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(error => {
                        MessageBox.Show("İletişim mesajları yüklenirken bir hata oluştu: " + error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), ex.Message);
                }
                else
                {
                    MessageBox.Show("İletişim mesajları yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDataGridView(List<ApiService.ContactMessageDto> messages)
        {
            // Kolonları ekle
            dgvMessages.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Ad", DataPropertyName = "Name", ReadOnly = true });
            dgvMessages.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "E-posta", DataPropertyName = "Email", ReadOnly = true });
            dgvMessages.Columns.Add(new DataGridViewTextBoxColumn { Name = "Message", HeaderText = "Mesaj", DataPropertyName = "Message", ReadOnly = true });
            dgvMessages.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Tarih", DataPropertyName = "CreatedAt", ReadOnly = true, DefaultCellStyle = { Format = "g" } });

            // DataGridView'e veriyi bağla
            dgvMessages.DataSource = messages.ToList();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ContactMessagesForm kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }

        // ContactMessageDto sınıfı ApiService.cs'ye taşındı.
        // Artık burada tanımlı olmasına gerek yok.
        // private class ContactMessageDto { ... }
    }
}