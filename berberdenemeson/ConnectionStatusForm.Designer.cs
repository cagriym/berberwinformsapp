namespace berberdenemeson
{
    partial class ConnectionStatusForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCheckConnection;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label titleLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Padding = new System.Windows.Forms.Padding(20);
            this.panel.Controls.Add(this.titleLabel);
            this.panel.Controls.Add(this.lblStatus);
            this.panel.Controls.Add(this.btnCheckConnection);
            this.panel.Controls.Add(this.btnSync);
            this.panel.Controls.Add(this.closeButton);
            // 
            // titleLabel
            // 
            this.titleLabel.Text = "Bağlantı Durumu";
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.AutoSize = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(20, 60);
            this.lblStatus.AutoSize = true;
            this.lblStatus.Text = "Durum kontrol ediliyor...";
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Text = "Bağlantıyı Kontrol Et";
            this.btnCheckConnection.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCheckConnection.ForeColor = System.Drawing.Color.White;
            this.btnCheckConnection.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnCheckConnection.Location = new System.Drawing.Point(20, 100);
            this.btnCheckConnection.Size = new System.Drawing.Size(150, 35);
            this.btnCheckConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckConnection.FlatAppearance.BorderSize = 0;
            this.btnCheckConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnSync
            // 
            this.btnSync.Text = "Senkronize Et";
            this.btnSync.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSync.ForeColor = System.Drawing.Color.White;
            this.btnSync.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnSync.Location = new System.Drawing.Point(190, 100);
            this.btnSync.Size = new System.Drawing.Size(150, 35);
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.FlatAppearance.BorderSize = 0;
            this.btnSync.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // closeButton
            // 
            this.closeButton.Text = "Kapat";
            this.closeButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.closeButton.Location = new System.Drawing.Point(280, 160);
            this.closeButton.Size = new System.Drawing.Size(80, 35);
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ConnectionStatusForm
            // 
            this.Text = "Bağlantı Durumu";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.panel);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
    }
} 