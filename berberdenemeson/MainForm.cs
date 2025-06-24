using System;
using System.Drawing;
using System.Windows.Forms;

namespace berberdenemeson
{
    public partial class MainForm : Form
    {
        private Form _currentForm;
        private Button activeButton; // Aktif butonu takip etmek için

        public MainForm()
        {
            InitializeComponent();
            SetupEventHandlers();
            LoadDashboard(); // Uygulama başlangıcında dashboard'u yükle
        }

        private void SetupEventHandlers()
        {
            // Buton event handler'larını bağla
            btnAnaEkran.Click += btnAnaEkran_Click;
            btnRandevular.Click += btnRandevular_Click;
            btnMusteriler.Click += btnMusteriler_Click;
            btnPersonel.Click += btnPersonel_Click;
            btnIhtiyaclar.Click += btnIhtiyaclar_Click;
            btnRaporlar.Click += btnRaporlar_Click;
            btnExit.Click += btnExit_Click;
            btnContactMessages.Click += btnContactMessages_Click;
            btnConnectionStatus.Click += btnConnectionStatus_Click;
            btnServisler.Click += btnServisler_Click;
        }

        private void LoadDashboard()
        {
            var dashboard = new DashboardForm();
            // Dashboard'daki özel olaylara abone ol
            dashboard.RandevularRequested += (s, e) => btnRandevular_Click(this, EventArgs.Empty);
            dashboard.MusterilerRequested += (s, e) => btnMusteriler_Click(this, EventArgs.Empty);
            dashboard.IhtiyaclarRequested += (s, e) => btnIhtiyaclar_Click(this, EventArgs.Empty);
            dashboard.RaporlarRequested += (s, e) => btnRaporlar_Click(this, EventArgs.Empty);

            ShowForm(dashboard, btnAnaEkran);
        }

        private void ShowForm(Form formToShow, Button activatedButton)
        {
            // Mevcut formu kapat (varsa)
            if (_currentForm != null && !_currentForm.IsDisposed)
            {
                _currentForm.Close();
                _currentForm.Dispose(); // Kaynakları serbest bırak
            }

            _currentForm = formToShow;
            _currentForm.TopLevel = false; // Child form olarak aç
            _currentForm.FormBorderStyle = FormBorderStyle.None; // Kenarlık yok
            _currentForm.Dock = DockStyle.Fill; // Parent kontrolünü doldur

            panelContent.Controls.Add(_currentForm); // Panelin içine ekle
            _currentForm.Show(); // Formu göster
            _currentForm.BringToFront(); // En öne getir

            ActivateButton(activatedButton); // Buton rengini güncelle
            UpdateTitle(_currentForm); // Başlığı güncelle
        }

        // Ana ekran butonu tıklaması
        private void btnAnaEkran_Click(object sender, EventArgs e)
        {
            LoadDashboard(); // Dashboard'u yeniden yükle
        }

        // Randevular butonu tıklaması
        private void btnRandevular_Click(object sender, EventArgs e)
        {
            ShowForm(new RandevularForm(), btnRandevular);
        }

        // Müşteriler butonu tıklaması
        private void btnMusteriler_Click(object sender, EventArgs e)
        {
            ShowForm(new MusterilerForm(), btnMusteriler);
        }

        // Personel butonu tıklaması
        private void btnPersonel_Click(object sender, EventArgs e)
        {
            ShowForm(new PersonelForm(), btnPersonel);
        }

        // İhtiyaçlar butonu tıklaması
        private void btnIhtiyaclar_Click(object sender, EventArgs e)
        {
            ShowForm(new IhtiyaclarForm(), btnIhtiyaclar);
        }

        // Raporlar butonu tıklaması
        private void btnRaporlar_Click(object sender, EventArgs e)
        {
            ShowForm(new RaporlamaForm(), btnRaporlar);
        }

        // İletişim Mesajları butonu tıklaması
        private void btnContactMessages_Click(object sender, EventArgs e)
        {
            ShowForm(new ContactMessagesForm(), btnContactMessages);
        }

        // Çıkış butonu tıklaması
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                // Mevcut formu güvenli şekilde kapat
                if (_currentForm != null && !_currentForm.IsDisposed)
                {
                    _currentForm.Close();
                    _currentForm.Dispose();
                }

                // ConnectionManager'ı dispose et
                ConnectionManager.Instance.Dispose();

                // Tüm timer'ları durdur
                Application.DoEvents();

                // Uygulamayı kapat
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Çıkış sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void btnConnectionStatus_Click(object sender, EventArgs e)
        {
            var connectionForm = new ConnectionStatusForm();
            connectionForm.ShowDialog();
        }

        private void btnServisler_Click(object sender, EventArgs e)
        {
            ShowForm(new ServislerForm(), btnServisler);
        }

        private void ActivateButton(Button btn)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.FromArgb(40, 40, 60); // Eski rengi
                activeButton.ForeColor = SystemColors.ControlLightLight; // Eski metin rengi
            }

            activeButton = btn;
            activeButton.BackColor = Color.FromArgb(52, 152, 219); // Vurgulu renk
            activeButton.ForeColor = Color.White; // Vurgulu metin rengi
        }

        private void UpdateTitle(Form form)
        {
            string title = "Berber Randevu Sistemi";

            if (form is DashboardForm)
                title += " - Gösterge Paneli";
            else if (form is RandevularForm)
                title += " - Randevular";
            else if (form is MusterilerForm)
                title += " - Müşteriler";
            else if (form is PersonelForm)
                title += " - Personel";
            else if (form is IhtiyaclarForm)
                title += " - İhtiyaçlar";
            else if (form is RaporlamaForm)
                title += " - Raporlar";
            else if (form is ContactMessagesForm)
                title += " - İletişim Mesajları";
            else if (form is ServislerForm)
                title += " - Servisler";

            lblTitle.Text = title;
        }

        private void HighlightActiveButton(Button activeButton)
        {
            // Tüm butonları normal renge çevir
            btnAnaEkran.BackColor = Color.FromArgb(40, 40, 60);
            btnRandevular.BackColor = Color.FromArgb(40, 40, 60);
            btnMusteriler.BackColor = Color.FromArgb(40, 40, 60);
            btnPersonel.BackColor = Color.FromArgb(40, 40, 60);
            btnIhtiyaclar.BackColor = Color.FromArgb(40, 40, 60);
            btnRaporlar.BackColor = Color.FromArgb(40, 40, 60);
            btnContactMessages.BackColor = Color.FromArgb(40, 40, 60);
            btnConnectionStatus.BackColor = Color.FromArgb(40, 40, 60);

            // Aktif butonu vurgula
            activeButton.BackColor = Color.FromArgb(52, 152, 219);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Mevcut formu güvenli şekilde kapat
                if (_currentForm != null && !_currentForm.IsDisposed)
                {
                    _currentForm.Close();
                    _currentForm.Dispose();
                }

                // ConnectionManager'ı dispose et
                ConnectionManager.Instance.Dispose();

                // Tüm timer'ları durdur
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                // Hata durumunda sessizce devam et
                Console.WriteLine($"Form kapatma hatası: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }
    }
}