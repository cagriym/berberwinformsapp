namespace berberdenemeson
{
    partial class MusterilerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvMusteriler;
        private System.Windows.Forms.Button btnYeniMusteri;
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
        private System.Windows.Forms.Panel panelMain;

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
            this.lblBaslik = new System.Windows.Forms.Label();
            this.panelArama = new System.Windows.Forms.Panel();
            this.lblArama = new System.Windows.Forms.Label();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.panelButonlar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnYeniMusteri = new System.Windows.Forms.Button();
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.dgvMusteriler = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button(); // Refresh button
            this.btnAdd = new System.Windows.Forms.Button(); // Add button
            this.btnSearch = new System.Windows.Forms.Button(); // Search button
            this.txtSearch = new System.Windows.Forms.TextBox(); // Search textbox
            this._statusLabel = new System.Windows.Forms.Label(); // Status label
            this.panelMain = new System.Windows.Forms.Panel();

            this.panelArama.SuspendLayout();
            this.panelButonlar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            //
            // lblBaslik
            //
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblBaslik.Location = new System.Drawing.Point(20, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(760, 40);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Müşteri Yönetimi";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelArama
            //
            this.panelArama.Controls.Add(this.lblArama);
            this.panelArama.Controls.Add(this.txtArama);
            this.panelArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArama.Location = new System.Drawing.Point(20, 60);
            this.panelArama.Name = "panelArama";
            this.panelArama.Size = new System.Drawing.Size(760, 0); // Bu panel artık kullanılmayacak, Search paneli var
            this.panelArama.TabIndex = 1;
            this.panelArama.Visible = false; // Gizle
            //
            // lblArama
            //
            this.lblArama.AutoSize = true;
            this.lblArama.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblArama.Location = new System.Drawing.Point(0, 7);
            this.lblArama.Name = "lblArama";
            this.lblArama.Size = new System.Drawing.Size(50, 19);
            this.lblArama.TabIndex = 0;
            this.lblArama.Text = "Arama:";
            //
            // txtArama
            //
            this.txtArama.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtArama.Location = new System.Drawing.Point(56, 4);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(200, 25);
            this.txtArama.TabIndex = 1;
            //
            // panelButonlar
            //
            this.panelButonlar.Controls.Add(this.btnYeniMusteri);
            this.panelButonlar.Controls.Add(this.btnDuzenle);
            this.panelButonlar.Controls.Add(this.btnSil);
            this.panelButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButonlar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButonlar.Location = new System.Drawing.Point(20, 545);
            this.panelButonlar.Name = "panelButonlar";
            this.panelButonlar.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelButonlar.Size = new System.Drawing.Size(760, 55);
            this.panelButonlar.TabIndex = 2;
            //
            // btnYeniMusteri
            //
            this.btnYeniMusteri.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnYeniMusteri.FlatAppearance.BorderSize = 0;
            this.btnYeniMusteri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniMusteri.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYeniMusteri.ForeColor = System.Drawing.Color.White;
            this.btnYeniMusteri.Location = new System.Drawing.Point(620, 13);
            this.btnYeniMusteri.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnYeniMusteri.Name = "btnYeniMusteri";
            this.btnYeniMusteri.Size = new System.Drawing.Size(130, 35);
            this.btnYeniMusteri.TabIndex = 0;
            this.btnYeniMusteri.Text = "➕ Yeni Müşteri";
            this.btnYeniMusteri.UseVisualStyleBackColor = false;
            //
            // btnDuzenle
            //
            this.btnDuzenle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnDuzenle.FlatAppearance.BorderSize = 0;
            this.btnDuzenle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuzenle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDuzenle.ForeColor = System.Drawing.Color.White;
            this.btnDuzenle.Location = new System.Drawing.Point(480, 13);
            this.btnDuzenle.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(130, 35);
            this.btnDuzenle.TabIndex = 1;
            this.btnDuzenle.Text = "✏️ Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = false;
            //
            // btnSil
            //
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(340, 13);
            this.btnSil.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(130, 35);
            this.btnSil.TabIndex = 2;
            this.btnSil.Text = "🗑️ Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            //
            // dgvMusteriler
            //
            this.dgvMusteriler.AllowUserToAddRows = false;
            this.dgvMusteriler.AllowUserToDeleteRows = false;
            this.dgvMusteriler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMusteriler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMusteriler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMusteriler.Location = new System.Drawing.Point(0, 0);
            this.dgvMusteriler.Name = "dgvMusteriler";
            this.dgvMusteriler.ReadOnly = true;
            this.dgvMusteriler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMusteriler.Size = new System.Drawing.Size(800, 450);
            this.dgvMusteriler.TabIndex = 0;
            this.dgvMusteriler.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgvMusteriler.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.dgvMusteriler.RowTemplate.Height = 36;
            //
            // panelSearch
            //
            this.panelSearch.Controls.Add(this.btnRefresh);
            this.panelSearch.Controls.Add(this.btnAdd);
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this._statusLabel);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(20, 60);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(760, 65);
            this.panelSearch.TabIndex = 4;
            //
            // txtSearch
            //
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(0, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 25);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "Müşteri ara..."; // Placeholder text
            this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText; // Placeholder color
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(310, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "🔍 Ara";
            this.btnSearch.UseVisualStyleBackColor = false;
            //
            // btnAdd
            //
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(420, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "➕ Ekle";
            this.btnAdd.UseVisualStyleBackColor = false;
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(530, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            //
            // _statusLabel
            //
            this._statusLabel.AutoSize = true;
            this._statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._statusLabel.Location = new System.Drawing.Point(640, 25);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(40, 15);
            this._statusLabel.TabIndex = 4;
            this._statusLabel.Text = "Hazır";
            this._statusLabel.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            //
            // panelMain
            //
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Name = "panelMain";
            this.panelMain.TabIndex = 100;
            this.panelMain.Controls.Add(this.dgvMusteriler);
            this.panelMain.Controls.Add(this.panelSearch);
            this.panelMain.Controls.Add(this.lblBaslik);
            //
            // MusterilerForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButonlar);
            this.Name = "MusterilerForm";
            this.Text = "Müşteriler";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.panelArama.ResumeLayout(false);
            this.panelArama.PerformLayout();
            this.panelButonlar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion
    }
}