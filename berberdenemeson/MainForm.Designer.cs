using System.Windows.Forms;

namespace berberdenemeson
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelSidebar;
        private Panel panelHeader;
        private Panel panelContent;
        private Button btnAnaEkran;
        private Button btnRandevular;
        private Button btnMusteriler;
        private Button btnPersonel;
        private Button btnIhtiyaclar;
        private Button btnExit;
        private Button btnRaporlar;
        private Label lblTitle;
        private Button btnContactMessages;
        private Button btnConnectionStatus;
        private Button btnServisler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnConnectionStatus = new System.Windows.Forms.Button();
            this.btnContactMessages = new System.Windows.Forms.Button();
            this.btnIhtiyaclar = new System.Windows.Forms.Button();
            this.btnPersonel = new System.Windows.Forms.Button();
            this.btnMusteriler = new System.Windows.Forms.Button();
            this.btnRandevular = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.btnAnaEkran = new System.Windows.Forms.Button();
            this.btnRaporlar = new System.Windows.Forms.Button();
            this.btnServisler = new System.Windows.Forms.Button();
            this.panelSidebar.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.panelSidebar.Controls.Add(this.btnExit);
            this.panelSidebar.Controls.Add(this.btnConnectionStatus);
            this.panelSidebar.Controls.Add(this.btnContactMessages);
            this.panelSidebar.Controls.Add(this.btnServisler);
            this.panelSidebar.Controls.Add(this.btnIhtiyaclar);
            this.panelSidebar.Controls.Add(this.btnPersonel);
            this.panelSidebar.Controls.Add(this.btnMusteriler);
            this.panelSidebar.Controls.Add(this.btnRandevular);
            this.panelSidebar.Controls.Add(this.btnAnaEkran);
            this.panelSidebar.Controls.Add(this.btnRaporlar);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Width = 180;
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.TabIndex = 0;
            // 
            // btnAnaEkran
            // 
            this.btnAnaEkran.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAnaEkran.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnaEkran.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnaEkran.ForeColor = System.Drawing.Color.White;
            this.btnAnaEkran.Text = "🏠 Ana Ekran";
            this.btnAnaEkran.Height = 60;
            this.btnAnaEkran.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            // 
            // btnRandevular
            // 
            this.btnRandevular.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRandevular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandevular.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRandevular.ForeColor = System.Drawing.Color.White;
            this.btnRandevular.Text = "📅 Randevular";
            this.btnRandevular.Height = 50;
            // 
            // btnMusteriler
            // 
            this.btnMusteriler.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMusteriler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMusteriler.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMusteriler.ForeColor = System.Drawing.Color.White;
            this.btnMusteriler.Text = "👥 Müşteriler";
            this.btnMusteriler.Height = 50;
            // 
            // btnPersonel
            // 
            this.btnPersonel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPersonel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPersonel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPersonel.ForeColor = System.Drawing.Color.White;
            this.btnPersonel.Text = "👨‍💼 Personel";
            this.btnPersonel.Height = 50;
            // 
            // btnIhtiyaclar
            // 
            this.btnIhtiyaclar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnIhtiyaclar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIhtiyaclar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnIhtiyaclar.ForeColor = System.Drawing.Color.White;
            this.btnIhtiyaclar.Text = "📋 İhtiyaçlar";
            this.btnIhtiyaclar.Height = 50;
            // 
            // btnRaporlar
            // 
            this.btnRaporlar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRaporlar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRaporlar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRaporlar.ForeColor = System.Drawing.Color.White;
            this.btnRaporlar.Text = "📊 Raporlar";
            this.btnRaporlar.Height = 50;
            // 
            // btnContactMessages
            // 
            this.btnContactMessages.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnContactMessages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContactMessages.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnContactMessages.ForeColor = System.Drawing.Color.White;
            this.btnContactMessages.Text = "✉️ İletişim Mesajları";
            this.btnContactMessages.Height = 50;
            this.btnContactMessages.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            // 
            // btnConnectionStatus
            // 
            this.btnConnectionStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConnectionStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.btnConnectionStatus.Text = "🌐 Bağlantı Durumu";
            this.btnConnectionStatus.Height = 50;
            this.btnConnectionStatus.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            // 
            // btnServisler
            // 
            this.btnServisler.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnServisler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServisler.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnServisler.ForeColor = System.Drawing.Color.White;
            this.btnServisler.Text = "🛠️ Servisler";
            this.btnServisler.Height = 50;
            this.btnServisler.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnServisler.TabIndex = 8;
            this.btnServisler.Name = "btnServisler";
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Text = "🚪 Çıkış";
            this.btnExit.Height = 50;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 60;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Berber Randevu Sistemi - Dashboard";
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = false;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Padding = new Padding(10, 0, 0, 0);
            // 
            // panelContent
            // 
            this.panelContent.Dock = DockStyle.Fill;
            this.panelContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelContent.TabIndex = 2;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Berber Randevu Sistemi";
            this.WindowState = FormWindowState.Maximized;
            this.panelSidebar.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
