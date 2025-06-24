namespace berberdenemeson
{
    partial class RandevularForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvRandevular;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnTamamla;
        private System.Windows.Forms.DateTimePicker dtpAramaTarihi;
        private System.Windows.Forms.Label lblAramaTarihi;
        private System.Windows.Forms.Panel panelArama;
        private System.Windows.Forms.FlowLayoutPanel panelButonlar;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSearchAdd; // Add button (btnYeniRandevu/btnAdd yerine bu kullanılıyor gibi)
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.panelArama = new System.Windows.Forms.Panel();
            this.dtpAramaTarihi = new System.Windows.Forms.DateTimePicker();
            this.lblAramaTarihi = new System.Windows.Forms.Label();
            this.panelButonlar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnTamamla = new System.Windows.Forms.Button();
            this.dgvRandevular = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this._statusLabel = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearchAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelArama.SuspendLayout();
            this.panelButonlar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBaslik
            // 
            this.lblBaslik.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(17, 17);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(652, 61);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Randevular";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelArama
            // 
            this.panelArama.Controls.Add(this.dtpAramaTarihi);
            this.panelArama.Controls.Add(this.lblAramaTarihi);
            this.panelArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArama.Location = new System.Drawing.Point(17, 113);
            this.panelArama.Name = "panelArama";
            this.panelArama.Size = new System.Drawing.Size(652, 35);
            this.panelArama.TabIndex = 2;
            // 
            // dtpAramaTarihi
            // 
            this.dtpAramaTarihi.Checked = false;
            this.dtpAramaTarihi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpAramaTarihi.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAramaTarihi.Location = new System.Drawing.Point(111, 7);
            this.dtpAramaTarihi.Name = "dtpAramaTarihi";
            this.dtpAramaTarihi.ShowCheckBox = true;
            this.dtpAramaTarihi.Size = new System.Drawing.Size(103, 25);
            this.dtpAramaTarihi.TabIndex = 1;
            // 
            // lblAramaTarihi
            // 
            this.lblAramaTarihi.AutoSize = true;
            this.lblAramaTarihi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAramaTarihi.Location = new System.Drawing.Point(9, 10);
            this.lblAramaTarihi.Name = "lblAramaTarihi";
            this.lblAramaTarihi.Size = new System.Drawing.Size(84, 19);
            this.lblAramaTarihi.TabIndex = 0;
            this.lblAramaTarihi.Text = "Tarih Arama:";
            // 
            // panelButonlar
            // 
            this.panelButonlar.Controls.Add(this.btnDuzenle);
            this.panelButonlar.Controls.Add(this.btnSil);
            this.panelButonlar.Controls.Add(this.btnTamamla);
            this.panelButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButonlar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButonlar.Location = new System.Drawing.Point(17, 455);
            this.panelButonlar.Name = "panelButonlar";
            this.panelButonlar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.panelButonlar.Size = new System.Drawing.Size(652, 48);
            this.panelButonlar.TabIndex = 4;
            // 
            // btnDuzenle
            // 
            this.btnDuzenle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnDuzenle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuzenle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDuzenle.ForeColor = System.Drawing.Color.White;
            this.btnDuzenle.Location = new System.Drawing.Point(563, 3);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(86, 30);
            this.btnDuzenle.TabIndex = 0;
            this.btnDuzenle.Text = "✏️ Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = false;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(471, 3);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(86, 30);
            this.btnSil.TabIndex = 1;
            this.btnSil.Text = "🗑️ Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            // 
            // btnTamamla
            // 
            this.btnTamamla.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnTamamla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTamamla.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnTamamla.ForeColor = System.Drawing.Color.White;
            this.btnTamamla.Location = new System.Drawing.Point(379, 3);
            this.btnTamamla.Name = "btnTamamla";
            this.btnTamamla.Size = new System.Drawing.Size(86, 30);
            this.btnTamamla.TabIndex = 2;
            this.btnTamamla.Text = "✅ Tamamla";
            this.btnTamamla.UseVisualStyleBackColor = false;
            // 
            // dgvRandevular
            // 
            this.dgvRandevular.AllowUserToAddRows = false;
            this.dgvRandevular.AllowUserToDeleteRows = false;
            this.dgvRandevular.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRandevular.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRandevular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 14F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRandevular.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRandevular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRandevular.Location = new System.Drawing.Point(17, 148);
            this.dgvRandevular.Name = "dgvRandevular";
            this.dgvRandevular.ReadOnly = true;
            this.dgvRandevular.RowTemplate.Height = 36;
            this.dgvRandevular.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRandevular.Size = new System.Drawing.Size(652, 307);
            this.dgvRandevular.TabIndex = 0;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this._statusLabel);
            this.panelSearch.Controls.Add(this.btnRefresh);
            this.panelSearch.Controls.Add(this.btnSearchAdd);
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(17, 78);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelSearch.Size = new System.Drawing.Size(652, 35);
            this.panelSearch.TabIndex = 1;
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoSize = true;
            this._statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this._statusLabel.Location = new System.Drawing.Point(480, 11);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(34, 15);
            this._statusLabel.TabIndex = 4;
            this._statusLabel.Text = "Hazır";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(403, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(69, 26);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnSearchAdd
            // 
            this.btnSearchAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnSearchAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchAdd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSearchAdd.ForeColor = System.Drawing.Color.White;
            this.btnSearchAdd.Location = new System.Drawing.Point(309, 4);
            this.btnSearchAdd.Name = "btnSearchAdd";
            this.btnSearchAdd.Size = new System.Drawing.Size(86, 26);
            this.btnSearchAdd.TabIndex = 2;
            this.btnSearchAdd.Text = "➕ Yeni Randevu";
            this.btnSearchAdd.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(231, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 26);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "🔍 Ara";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(9, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(215, 25);
            this.txtSearch.TabIndex = 0;
            // 
            // RandevularForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(686, 520);
            this.Controls.Add(this.dgvRandevular);
            this.Controls.Add(this.panelButonlar);
            this.Controls.Add(this.panelArama);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.lblBaslik);
            this.Name = "RandevularForm";
            this.Padding = new System.Windows.Forms.Padding(17, 17, 17, 17);
            this.Text = "Randevular";
            this.panelArama.ResumeLayout(false);
            this.panelArama.PerformLayout();
            this.panelButonlar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion
    }
}
