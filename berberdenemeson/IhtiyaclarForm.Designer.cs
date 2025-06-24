// IhtiyaclarForm.Designer.cs
namespace berberdenemeson
{
    partial class IhtiyaclarForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvIhtiyaclar;
        private System.Windows.Forms.Button btnYeniIhtiyac;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.Label lblArama;
        private System.Windows.Forms.Panel panelArama;
        private System.Windows.Forms.FlowLayoutPanel panelButonlar;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label _statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.panelArama = new System.Windows.Forms.Panel();
            this.lblArama = new System.Windows.Forms.Label();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.panelButonlar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnYeniIhtiyac = new System.Windows.Forms.Button();
            this.dgvIhtiyaclar = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this._statusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIhtiyaclar)).BeginInit();
            this.panelArama.SuspendLayout();
            this.SuspendLayout();

            // lblBaslik
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.Location = new System.Drawing.Point(0, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(800, 70);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "İhtiyaç Yönetimi";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBaslik.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;

            // panelArama
            this.panelArama.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelArama.Controls.Add(this.lblArama);
            this.panelArama.Controls.Add(this.txtArama);
            this.panelArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArama.Location = new System.Drawing.Point(0, 70);
            this.panelArama.Name = "panelArama";
            this.panelArama.Size = new System.Drawing.Size(800, 60);
            this.panelArama.TabIndex = 1;

            // lblArama
            this.lblArama.AutoSize = true;
            this.lblArama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblArama.Location = new System.Drawing.Point(20, 20);
            this.lblArama.Name = "lblArama";
            this.lblArama.Size = new System.Drawing.Size(80, 15);
            this.lblArama.TabIndex = 0;
            this.lblArama.Text = "İhtiyaç Ara:";

            // txtArama
            this.txtArama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtArama.Location = new System.Drawing.Point(110, 15);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(300, 23);
            this.txtArama.TabIndex = 1;

            // panelButonlar
            this.panelButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButonlar.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelButonlar.WrapContents = false;
            this.panelButonlar.Height = 70;
            this.panelButonlar.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.panelButonlar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelButonlar.Controls.Add(this.btnYeniIhtiyac);
            this.panelButonlar.Controls.Add(this.btnDuzenle);
            this.panelButonlar.Controls.Add(this.btnSil);
            this.panelButonlar.Controls.Add(this._statusLabel);

            // btnYeniIhtiyac
            this.btnYeniIhtiyac.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnYeniIhtiyac.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniIhtiyac.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnYeniIhtiyac.ForeColor = System.Drawing.Color.White;
            this.btnYeniIhtiyac.Location = new System.Drawing.Point(20, 10);
            this.btnYeniIhtiyac.Name = "btnYeniIhtiyac";
            this.btnYeniIhtiyac.Size = new System.Drawing.Size(160, 45);
            this.btnYeniIhtiyac.TabIndex = 0;
            this.btnYeniIhtiyac.Text = "Yeni İhtiyaç";
            this.btnYeniIhtiyac.UseVisualStyleBackColor = false;
            this.btnYeniIhtiyac.Margin = new System.Windows.Forms.Padding(10);

            // btnDuzenle
            this.btnDuzenle.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnDuzenle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuzenle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnDuzenle.ForeColor = System.Drawing.Color.White;
            this.btnDuzenle.Location = new System.Drawing.Point(130, 10);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(140, 45);
            this.btnDuzenle.TabIndex = 1;
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = false;
            this.btnDuzenle.Margin = new System.Windows.Forms.Padding(10);

            // btnSil
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(240, 10);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(120, 45);
            this.btnSil.TabIndex = 2;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Margin = new System.Windows.Forms.Padding(10);

            // dgvIhtiyaclar
            this.dgvIhtiyaclar.AllowUserToAddRows = false;
            this.dgvIhtiyaclar.AllowUserToDeleteRows = false;
            this.dgvIhtiyaclar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIhtiyaclar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIhtiyaclar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIhtiyaclar.Location = new System.Drawing.Point(0, 0);
            this.dgvIhtiyaclar.Name = "dgvIhtiyaclar";
            this.dgvIhtiyaclar.ReadOnly = true;
            this.dgvIhtiyaclar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIhtiyaclar.Size = new System.Drawing.Size(800, 450);
            this.dgvIhtiyaclar.TabIndex = 0;
            this.dgvIhtiyaclar.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgvIhtiyaclar.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.dgvIhtiyaclar.RowTemplate.Height = 36;

            // panelSearch
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Height = 80;
            this.panelSearch.Padding = new System.Windows.Forms.Padding(20);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(20, 20, 20, 10);

            // txtSearch
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.Width = 300;
            this.txtSearch.Height = 35;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.txtSearch.Text = "İhtiyaç ara...";

            // btnSearch
            this.btnSearch.Text = "🔍 Ara";
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Width = 100;
            this.btnSearch.Height = 35;
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);

            // btnAdd
            this.btnAdd.Text = "➕ Yeni İhtiyaç";
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Width = 150;
            this.btnAdd.Height = 35;
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);

            // btnRefresh
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Width = 100;
            this.btnRefresh.Height = 35;

            // _statusLabel
            this._statusLabel.AutoSize = true;
            this._statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._statusLabel.Location = new System.Drawing.Point(500, 15);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(200, 15);
            this._statusLabel.TabIndex = 3;
            this._statusLabel.Text = "Hazır";
            this._statusLabel.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);

            // IhtiyaclarForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.dgvIhtiyaclar);
            this.Controls.Add(this.panelButonlar);
            this.Controls.Add(this.panelArama);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.lblBaslik);
            this.Name = "IhtiyaclarForm";
            this.Text = "İhtiyaçlar";
            ((System.ComponentModel.ISupportInitialize)(this.dgvIhtiyaclar)).EndInit();
            this.panelArama.ResumeLayout(false);
            this.panelArama.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
