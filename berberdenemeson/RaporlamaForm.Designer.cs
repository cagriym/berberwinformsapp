namespace berberdenemeson
{
    partial class RaporlamaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel panelOzet;
        private System.Windows.Forms.Label lblToplamRandevu; // Ekleniyor
        private System.Windows.Forms.Label lblToplamMusteri; // Ekleniyor
        private System.Windows.Forms.Label lblToplamPersonel; // Ekleniyor
        private System.Windows.Forms.Label lblToplamIhtiyac; // Ekleniyor
        private System.Windows.Forms.Label lblToplamUcret; // Ekleniyor
        private System.Windows.Forms.Label lblToplamIhtiyacMaliyet; // Ekleniyor
        private System.Windows.Forms.Label lblNetKarZarar; // Ekleniyor

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabRandevular;
        private System.Windows.Forms.TabPage tabMusteriler;
        private System.Windows.Forms.TabPage tabPersonel;
        private System.Windows.Forms.TabPage tabIhtiyaclar; // İhtiyaçlar sekmesi de ekleniyor

        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.DateTimePicker dtpBaslangic;
        private System.Windows.Forms.DateTimePicker dtpBitis;
        private System.Windows.Forms.Label lblTarihAraligi;
        private System.Windows.Forms.Button btnFiltrele;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Button btnExport; // Dışa aktar butonu

        // DataGridView'ler her TabPage için
        private System.Windows.Forms.DataGridView dgvRandevuRapor;
        private System.Windows.Forms.DataGridView dgvMusteriRapor;
        private System.Windows.Forms.DataGridView dgvPersonelRapor;
        private System.Windows.Forms.DataGridView dgvIhtiyacRapor;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.panelOzet = new System.Windows.Forms.Panel();
            this.lblToplamRandevu = new System.Windows.Forms.Label();
            this.lblToplamMusteri = new System.Windows.Forms.Label();
            this.lblToplamPersonel = new System.Windows.Forms.Label();
            this.lblToplamIhtiyac = new System.Windows.Forms.Label();
            this.lblToplamUcret = new System.Windows.Forms.Label();
            this.lblToplamIhtiyacMaliyet = new System.Windows.Forms.Label();
            this.lblNetKarZarar = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabRandevular = new System.Windows.Forms.TabPage();
            this.dgvRandevuRapor = new System.Windows.Forms.DataGridView();
            this.tabMusteriler = new System.Windows.Forms.TabPage();
            this.dgvMusteriRapor = new System.Windows.Forms.DataGridView();
            this.tabPersonel = new System.Windows.Forms.TabPage();
            this.dgvPersonelRapor = new System.Windows.Forms.DataGridView();
            this.tabIhtiyaclar = new System.Windows.Forms.TabPage();
            this.dgvIhtiyacRapor = new System.Windows.Forms.DataGridView();
            this.btnYenile = new System.Windows.Forms.Button();
            this.dtpBaslangic = new System.Windows.Forms.DateTimePicker();
            this.dtpBitis = new System.Windows.Forms.DateTimePicker();
            this.lblTarihAraligi = new System.Windows.Forms.Label();
            this.btnFiltrele = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.panelOzet.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabRandevular.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevuRapor)).BeginInit();
            this.tabMusteriler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriRapor)).BeginInit();
            this.tabPersonel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonelRapor)).BeginInit();
            this.tabIhtiyaclar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIhtiyacRapor)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBaslik
            // 
            this.lblBaslik.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(0, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(1029, 61);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Raporlama ve Analiz";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelOzet
            // 
            this.panelOzet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelOzet.Controls.Add(this.btnExport);
            this.panelOzet.Controls.Add(this.btnYenile);
            this.panelOzet.Controls.Add(this.lblTarihAraligi);
            this.panelOzet.Controls.Add(this.btnFiltrele);
            this.panelOzet.Controls.Add(this.dtpBaslangic);
            this.panelOzet.Controls.Add(this.dtpBitis);
            this.panelOzet.Controls.Add(this.lblToplamRandevu);
            this.panelOzet.Controls.Add(this.lblToplamMusteri);
            this.panelOzet.Controls.Add(this.lblToplamPersonel);
            this.panelOzet.Controls.Add(this.lblToplamIhtiyac);
            this.panelOzet.Controls.Add(this.lblToplamUcret);
            this.panelOzet.Controls.Add(this.lblToplamIhtiyacMaliyet);
            this.panelOzet.Controls.Add(this.lblNetKarZarar);
            this.panelOzet.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOzet.Location = new System.Drawing.Point(0, 61);
            this.panelOzet.Name = "panelOzet";
            this.panelOzet.Size = new System.Drawing.Size(1029, 104);
            this.panelOzet.TabIndex = 1;
            // 
            // lblToplamRandevu
            // 
            this.lblToplamRandevu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamRandevu.Location = new System.Drawing.Point(9, 9);
            this.lblToplamRandevu.Name = "lblToplamRandevu";
            this.lblToplamRandevu.Size = new System.Drawing.Size(189, 22);
            this.lblToplamRandevu.TabIndex = 0;
            this.lblToplamRandevu.Text = "Toplam Randevu: 0";
            // 
            // lblToplamMusteri
            // 
            this.lblToplamMusteri.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamMusteri.Location = new System.Drawing.Point(206, 9);
            this.lblToplamMusteri.Name = "lblToplamMusteri";
            this.lblToplamMusteri.Size = new System.Drawing.Size(189, 22);
            this.lblToplamMusteri.TabIndex = 1;
            this.lblToplamMusteri.Text = "Toplam Müşteri: 0";
            // 
            // lblToplamPersonel
            // 
            this.lblToplamPersonel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamPersonel.Location = new System.Drawing.Point(403, 9);
            this.lblToplamPersonel.Name = "lblToplamPersonel";
            this.lblToplamPersonel.Size = new System.Drawing.Size(189, 22);
            this.lblToplamPersonel.TabIndex = 2;
            this.lblToplamPersonel.Text = "Toplam Personel: 0";
            // 
            // lblToplamIhtiyac
            // 
            this.lblToplamIhtiyac.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamIhtiyac.Location = new System.Drawing.Point(600, 9);
            this.lblToplamIhtiyac.Name = "lblToplamIhtiyac";
            this.lblToplamIhtiyac.Size = new System.Drawing.Size(189, 22);
            this.lblToplamIhtiyac.TabIndex = 3;
            this.lblToplamIhtiyac.Text = "Toplam İhtiyaç: 0";
            // 
            // lblToplamUcret
            // 
            this.lblToplamUcret.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamUcret.Location = new System.Drawing.Point(9, 39);
            this.lblToplamUcret.Name = "lblToplamUcret";
            this.lblToplamUcret.Size = new System.Drawing.Size(189, 22);
            this.lblToplamUcret.TabIndex = 4;
            this.lblToplamUcret.Text = "Toplam Gelir: 0,00 TL";
            // 
            // lblToplamIhtiyacMaliyet
            // 
            this.lblToplamIhtiyacMaliyet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblToplamIhtiyacMaliyet.Location = new System.Drawing.Point(206, 39);
            this.lblToplamIhtiyacMaliyet.Name = "lblToplamIhtiyacMaliyet";
            this.lblToplamIhtiyacMaliyet.Size = new System.Drawing.Size(189, 22);
            this.lblToplamIhtiyacMaliyet.TabIndex = 5;
            this.lblToplamIhtiyacMaliyet.Text = "Toplam Maliyet: 0,00 TL";
            // 
            // lblNetKarZarar
            // 
            this.lblNetKarZarar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNetKarZarar.Location = new System.Drawing.Point(403, 39);
            this.lblNetKarZarar.Name = "lblNetKarZarar";
            this.lblNetKarZarar.Size = new System.Drawing.Size(189, 22);
            this.lblNetKarZarar.TabIndex = 6;
            this.lblNetKarZarar.Text = "Net Kar/Zarar: 0,00 TL";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabRandevular);
            this.tabControl.Controls.Add(this.tabMusteriler);
            this.tabControl.Controls.Add(this.tabPersonel);
            this.tabControl.Controls.Add(this.tabIhtiyaclar);
            this.tabControl.Location = new System.Drawing.Point(0, 165);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1029, 355);
            this.tabControl.TabIndex = 2;
            // 
            // tabRandevular
            // 
            this.tabRandevular.Controls.Add(this.dgvRandevuRapor);
            this.tabRandevular.Location = new System.Drawing.Point(4, 22);
            this.tabRandevular.Name = "tabRandevular";
            this.tabRandevular.Padding = new System.Windows.Forms.Padding(3);
            this.tabRandevular.Size = new System.Drawing.Size(1021, 329);
            this.tabRandevular.TabIndex = 0;
            this.tabRandevular.Text = "Randevular";
            this.tabRandevular.UseVisualStyleBackColor = true;
            // 
            // dgvRandevuRapor
            // 
            this.dgvRandevuRapor.AllowUserToAddRows = false;
            this.dgvRandevuRapor.AllowUserToDeleteRows = false;
            this.dgvRandevuRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRandevuRapor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRandevuRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 14F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRandevuRapor.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRandevuRapor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRandevuRapor.Location = new System.Drawing.Point(3, 3);
            this.dgvRandevuRapor.Name = "dgvRandevuRapor";
            this.dgvRandevuRapor.ReadOnly = true;
            this.dgvRandevuRapor.RowTemplate.Height = 36;
            this.dgvRandevuRapor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRandevuRapor.Size = new System.Drawing.Size(1015, 323);
            this.dgvRandevuRapor.TabIndex = 0;
            // 
            // tabMusteriler
            // 
            this.tabMusteriler.Controls.Add(this.dgvMusteriRapor);
            this.tabMusteriler.Location = new System.Drawing.Point(4, 22);
            this.tabMusteriler.Name = "tabMusteriler";
            this.tabMusteriler.Padding = new System.Windows.Forms.Padding(3);
            this.tabMusteriler.Size = new System.Drawing.Size(1021, 329);
            this.tabMusteriler.TabIndex = 1;
            this.tabMusteriler.Text = "Müşteriler";
            this.tabMusteriler.UseVisualStyleBackColor = true;
            // 
            // dgvMusteriRapor
            // 
            this.dgvMusteriRapor.AllowUserToAddRows = false;
            this.dgvMusteriRapor.AllowUserToDeleteRows = false;
            this.dgvMusteriRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMusteriRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMusteriRapor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMusteriRapor.Location = new System.Drawing.Point(3, 3);
            this.dgvMusteriRapor.Name = "dgvMusteriRapor";
            this.dgvMusteriRapor.ReadOnly = true;
            this.dgvMusteriRapor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMusteriRapor.Size = new System.Drawing.Size(1015, 323);
            this.dgvMusteriRapor.TabIndex = 0;
            // 
            // tabPersonel
            // 
            this.tabPersonel.Controls.Add(this.dgvPersonelRapor);
            this.tabPersonel.Location = new System.Drawing.Point(4, 22);
            this.tabPersonel.Name = "tabPersonel";
            this.tabPersonel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPersonel.Size = new System.Drawing.Size(1021, 329);
            this.tabPersonel.TabIndex = 2;
            this.tabPersonel.Text = "Personel";
            this.tabPersonel.UseVisualStyleBackColor = true;
            // 
            // dgvPersonelRapor
            // 
            this.dgvPersonelRapor.AllowUserToAddRows = false;
            this.dgvPersonelRapor.AllowUserToDeleteRows = false;
            this.dgvPersonelRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersonelRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPersonelRapor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersonelRapor.Location = new System.Drawing.Point(3, 3);
            this.dgvPersonelRapor.Name = "dgvPersonelRapor";
            this.dgvPersonelRapor.ReadOnly = true;
            this.dgvPersonelRapor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonelRapor.Size = new System.Drawing.Size(1015, 323);
            this.dgvPersonelRapor.TabIndex = 0;
            // 
            // tabIhtiyaclar
            // 
            this.tabIhtiyaclar.Controls.Add(this.dgvIhtiyacRapor);
            this.tabIhtiyaclar.Location = new System.Drawing.Point(4, 22);
            this.tabIhtiyaclar.Name = "tabIhtiyaclar";
            this.tabIhtiyaclar.Padding = new System.Windows.Forms.Padding(3);
            this.tabIhtiyaclar.Size = new System.Drawing.Size(1021, 329);
            this.tabIhtiyaclar.TabIndex = 3;
            this.tabIhtiyaclar.Text = "İhtiyaçlar";
            this.tabIhtiyaclar.UseVisualStyleBackColor = true;
            // 
            // dgvIhtiyacRapor
            // 
            this.dgvIhtiyacRapor.AllowUserToAddRows = false;
            this.dgvIhtiyacRapor.AllowUserToDeleteRows = false;
            this.dgvIhtiyacRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIhtiyacRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIhtiyacRapor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIhtiyacRapor.Location = new System.Drawing.Point(3, 3);
            this.dgvIhtiyacRapor.Name = "dgvIhtiyacRapor";
            this.dgvIhtiyacRapor.ReadOnly = true;
            this.dgvIhtiyacRapor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIhtiyacRapor.Size = new System.Drawing.Size(1015, 323);
            this.dgvIhtiyacRapor.TabIndex = 0;
            // 
            // btnYenile
            // 
            this.btnYenile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnYenile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYenile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnYenile.ForeColor = System.Drawing.Color.White;
            this.btnYenile.Location = new System.Drawing.Point(890, 35);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(86, 30);
            this.btnYenile.TabIndex = 3;
            this.btnYenile.Text = "🔄 Yenile";
            this.btnYenile.UseVisualStyleBackColor = false;
            // 
            // dtpBaslangic
            // 
            this.dtpBaslangic.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpBaslangic.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBaslangic.Location = new System.Drawing.Point(210, 73);
            this.dtpBaslangic.Name = "dtpBaslangic";
            this.dtpBaslangic.Size = new System.Drawing.Size(103, 25);
            this.dtpBaslangic.TabIndex = 4;
            // 
            // dtpBitis
            // 
            this.dtpBitis.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpBitis.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBitis.Location = new System.Drawing.Point(321, 74);
            this.dtpBitis.Name = "dtpBitis";
            this.dtpBitis.Size = new System.Drawing.Size(103, 25);
            this.dtpBitis.TabIndex = 5;
            // 
            // lblTarihAraligi
            // 
            this.lblTarihAraligi.AutoSize = true;
            this.lblTarihAraligi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTarihAraligi.Location = new System.Drawing.Point(109, 74);
            this.lblTarihAraligi.Name = "lblTarihAraligi";
            this.lblTarihAraligi.Size = new System.Drawing.Size(95, 19);
            this.lblTarihAraligi.TabIndex = 6;
            this.lblTarihAraligi.Text = "Tarih Aralığı:";
            // 
            // btnFiltrele
            // 
            this.btnFiltrele.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnFiltrele.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrele.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnFiltrele.ForeColor = System.Drawing.Color.White;
            this.btnFiltrele.Location = new System.Drawing.Point(430, 74);
            this.btnFiltrele.Name = "btnFiltrele";
            this.btnFiltrele.Size = new System.Drawing.Size(86, 30);
            this.btnFiltrele.TabIndex = 7;
            this.btnFiltrele.Text = "Apply Filter";
            this.btnFiltrele.UseVisualStyleBackColor = false;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.DarkGray;
            this.lblConnectionStatus.Location = new System.Drawing.Point(855, 65);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(148, 15);
            this.lblConnectionStatus.TabIndex = 99;
            this.lblConnectionStatus.Text = "Bağlantı kontrol ediliyor...";
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(522, 74);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(154, 30);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "📄 Raporu Dışa Aktar";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // RaporlamaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1029, 520);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelOzet);
            this.Controls.Add(this.lblBaslik);
            this.Name = "RaporlamaForm";
            this.Text = "Raporlama ve Analiz";
            this.panelOzet.ResumeLayout(false);
            this.panelOzet.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabRandevular.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevuRapor)).EndInit();
            this.tabMusteriler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriRapor)).EndInit();
            this.tabPersonel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonelRapor)).EndInit();
            this.tabIhtiyaclar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIhtiyacRapor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
