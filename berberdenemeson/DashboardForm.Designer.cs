using System.Windows.Forms;
using System.Drawing;

namespace berberdenemeson
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private Panel panelHeader;
        private Label lblDashboardTitle;

        // Statistics Cards (Updated names to match CS file usage)
        private Panel panelStats;
        private Panel panelStat1;
        private Panel panelStat2;
        private Panel panelStat3;
        private Panel panelStat4;
        private Label lblAppointmentCount; // Mapped to lblTotalAppointments in Designer
        private Label lblCustomerCount;   // Mapped to lblTotalCustomers in Designer
        private Label lblPersonnelCount;  // Mapped to lblTotalPersonnel in Designer
        private Label lblNeedsCount;      // Mapped to lblTodayAppointments in Designer
        private Label lblLastUpdate; // Son Güncelleme tarihi etiketi
        private Label lblConnectionStatus; // Bağlantı durumu etiketi

        // Financial Panel
        private Panel panelFinancial;
        private Label lblTotalRevenue; // Mapped to lblMonthlyRevenue in Designer
        private Label lblTotalCost; // New label for total cost
        private Label lblNetProfit; // New label for net profit/loss

        // Quick Actions
        private Panel panelQuickActions;
        private Button btnQuickAddAppointment;
        private Button btnQuickAddCustomer;
        private Button btnViewAllAppointments;
        private Button btnViewAllCustomers;
        private Label lblQuickActionsTitle;

        // Recent Appointments
        private Panel panelRecentAppointments;
        private DataGridView dgvRecentAppointments; // Renamed from dgvRecentAppointmentsGrid
        private Label lblRecentAppointmentsTitle;

        // Bottom Container
        private Panel panelBottomContainer;
        private Panel panelLeftBottom;
        private Panel panelRightBottom;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.panelStats = new System.Windows.Forms.Panel();
            this.panelStat4 = new System.Windows.Forms.Panel();
            this.lblNeedsCount = new System.Windows.Forms.Label();
            this.lblTodayAppointments = new System.Windows.Forms.Label();
            this.panelStat3 = new System.Windows.Forms.Panel();
            this.lblPersonnelCount = new System.Windows.Forms.Label();
            this.lblPersonnelTitle = new System.Windows.Forms.Label();
            this.panelStat2 = new System.Windows.Forms.Panel();
            this.lblCustomerCount = new System.Windows.Forms.Label();
            this.lblCustomersTitle = new System.Windows.Forms.Label();
            this.panelStat1 = new System.Windows.Forms.Panel();
            this.lblAppointmentCount = new System.Windows.Forms.Label();
            this.lblAppointmentsTitle = new System.Windows.Forms.Label();
            this.panelFinancial = new System.Windows.Forms.Panel();
            this.lblNetProfit = new System.Windows.Forms.Label();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.panelQuickActions = new System.Windows.Forms.Panel();
            this.lblQuickActionsTitle = new System.Windows.Forms.Label();
            this.btnQuickAddAppointment = new System.Windows.Forms.Button();
            this.btnQuickAddCustomer = new System.Windows.Forms.Button();
            this.btnViewAllAppointments = new System.Windows.Forms.Button();
            this.btnViewAllCustomers = new System.Windows.Forms.Button();
            this.panelRecentAppointments = new System.Windows.Forms.Panel();
            this.dgvRecentAppointments = new System.Windows.Forms.DataGridView();
            this.lblRecentAppointmentsTitle = new System.Windows.Forms.Label();
            this.panelBottomContainer = new System.Windows.Forms.Panel();
            this.panelRightBottom = new System.Windows.Forms.Panel();
            this.panelLeftBottom = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelStat4.SuspendLayout();
            this.panelStat3.SuspendLayout();
            this.panelStat2.SuspendLayout();
            this.panelStat1.SuspendLayout();
            this.panelFinancial.SuspendLayout();
            this.panelQuickActions.SuspendLayout();
            this.panelRecentAppointments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentAppointments)).BeginInit();
            this.panelBottomContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelHeader.Controls.Add(this.lblConnectionStatus);
            this.panelHeader.Controls.Add(this.lblLastUpdate);
            this.panelHeader.Controls.Add(this.lblDashboardTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(998, 61);
            this.panelHeader.TabIndex = 0;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.lblConnectionStatus.Location = new System.Drawing.Point(771, 9);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(148, 15);
            this.lblConnectionStatus.TabIndex = 1;
            this.lblConnectionStatus.Text = "Bağlantı kontrol ediliyor...";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLastUpdate.ForeColor = System.Drawing.Color.White;
            this.lblLastUpdate.Location = new System.Drawing.Point(771, 26);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(118, 15);
            this.lblLastUpdate.TabIndex = 2;
            this.lblLastUpdate.Text = "Son Güncelleme: Yok";
            // 
            // lblDashboardTitle
            // 
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.White;
            this.lblDashboardTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Padding = new System.Windows.Forms.Padding(0, 20, 0, 10);
            this.lblDashboardTitle.Size = new System.Drawing.Size(270, 67);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "Genel Durum Paneli";
            this.lblDashboardTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelStats
            // 
            this.panelStats.Controls.Add(this.panelStat4);
            this.panelStats.Controls.Add(this.panelStat3);
            this.panelStats.Controls.Add(this.panelStat2);
            this.panelStats.Controls.Add(this.panelStat1);
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStats.Location = new System.Drawing.Point(0, 61);
            this.panelStats.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panelStats.Name = "panelStats";
            this.panelStats.Padding = new System.Windows.Forms.Padding(40, 10, 40, 10);
            this.panelStats.Size = new System.Drawing.Size(998, 130);
            this.panelStats.TabIndex = 1;
            // 
            // panelStat4
            // 
            this.panelStat4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.panelStat4.Controls.Add(this.lblNeedsCount);
            this.panelStat4.Controls.Add(this.lblTodayAppointments);
            this.panelStat4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStat4.Location = new System.Drawing.Point(683, 10);
            this.panelStat4.Name = "panelStat4";
            this.panelStat4.Padding = new System.Windows.Forms.Padding(9);
            this.panelStat4.Size = new System.Drawing.Size(206, 110);
            this.panelStat4.TabIndex = 3;
            this.panelStat4.Paint += new System.Windows.Forms.PaintEventHandler(this.panelStat4_Paint);
            // 
            // lblNeedsCount
            // 
            this.lblNeedsCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblNeedsCount.ForeColor = System.Drawing.Color.White;
            this.lblNeedsCount.Location = new System.Drawing.Point(13, 43);
            this.lblNeedsCount.Name = "lblNeedsCount";
            this.lblNeedsCount.Size = new System.Drawing.Size(189, 43);
            this.lblNeedsCount.TabIndex = 1;
            this.lblNeedsCount.Text = "0";
            this.lblNeedsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTodayAppointments
            // 
            this.lblTodayAppointments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodayAppointments.ForeColor = System.Drawing.Color.White;
            this.lblTodayAppointments.Location = new System.Drawing.Point(13, 9);
            this.lblTodayAppointments.Name = "lblTodayAppointments";
            this.lblTodayAppointments.Size = new System.Drawing.Size(189, 22);
            this.lblTodayAppointments.TabIndex = 0;
            this.lblTodayAppointments.Text = "Toplam İhtiyaçlar";
            this.lblTodayAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelStat3
            // 
            this.panelStat3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.panelStat3.Controls.Add(this.lblPersonnelCount);
            this.panelStat3.Controls.Add(this.lblPersonnelTitle);
            this.panelStat3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStat3.Location = new System.Drawing.Point(477, 10);
            this.panelStat3.Name = "panelStat3";
            this.panelStat3.Padding = new System.Windows.Forms.Padding(9);
            this.panelStat3.Size = new System.Drawing.Size(206, 110);
            this.panelStat3.TabIndex = 2;
            // 
            // lblPersonnelCount
            // 
            this.lblPersonnelCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblPersonnelCount.ForeColor = System.Drawing.Color.White;
            this.lblPersonnelCount.Location = new System.Drawing.Point(9, 43);
            this.lblPersonnelCount.Name = "lblPersonnelCount";
            this.lblPersonnelCount.Size = new System.Drawing.Size(189, 43);
            this.lblPersonnelCount.TabIndex = 1;
            this.lblPersonnelCount.Text = "0";
            this.lblPersonnelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPersonnelTitle
            // 
            this.lblPersonnelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPersonnelTitle.ForeColor = System.Drawing.Color.White;
            this.lblPersonnelTitle.Location = new System.Drawing.Point(9, 9);
            this.lblPersonnelTitle.Name = "lblPersonnelTitle";
            this.lblPersonnelTitle.Size = new System.Drawing.Size(189, 22);
            this.lblPersonnelTitle.TabIndex = 0;
            this.lblPersonnelTitle.Text = "Toplam Personel";
            this.lblPersonnelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelStat2
            // 
            this.panelStat2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panelStat2.Controls.Add(this.lblCustomerCount);
            this.panelStat2.Controls.Add(this.lblCustomersTitle);
            this.panelStat2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStat2.Location = new System.Drawing.Point(271, 10);
            this.panelStat2.Name = "panelStat2";
            this.panelStat2.Padding = new System.Windows.Forms.Padding(9);
            this.panelStat2.Size = new System.Drawing.Size(206, 110);
            this.panelStat2.TabIndex = 1;
            // 
            // lblCustomerCount
            // 
            this.lblCustomerCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCount.ForeColor = System.Drawing.Color.White;
            this.lblCustomerCount.Location = new System.Drawing.Point(21, 43);
            this.lblCustomerCount.Name = "lblCustomerCount";
            this.lblCustomerCount.Size = new System.Drawing.Size(189, 43);
            this.lblCustomerCount.TabIndex = 1;
            this.lblCustomerCount.Text = "0";
            this.lblCustomerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomersTitle
            // 
            this.lblCustomersTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCustomersTitle.ForeColor = System.Drawing.Color.White;
            this.lblCustomersTitle.Location = new System.Drawing.Point(9, 9);
            this.lblCustomersTitle.Name = "lblCustomersTitle";
            this.lblCustomersTitle.Size = new System.Drawing.Size(189, 22);
            this.lblCustomersTitle.TabIndex = 0;
            this.lblCustomersTitle.Text = "Toplam Müşteri";
            this.lblCustomersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelStat1
            // 
            this.panelStat1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelStat1.Controls.Add(this.lblAppointmentCount);
            this.panelStat1.Controls.Add(this.lblAppointmentsTitle);
            this.panelStat1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStat1.Location = new System.Drawing.Point(40, 10);
            this.panelStat1.Name = "panelStat1";
            this.panelStat1.Padding = new System.Windows.Forms.Padding(9);
            this.panelStat1.Size = new System.Drawing.Size(231, 110);
            this.panelStat1.TabIndex = 0;
            // 
            // lblAppointmentCount
            // 
            this.lblAppointmentCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblAppointmentCount.ForeColor = System.Drawing.Color.White;
            this.lblAppointmentCount.Location = new System.Drawing.Point(9, 43);
            this.lblAppointmentCount.Name = "lblAppointmentCount";
            this.lblAppointmentCount.Size = new System.Drawing.Size(214, 43);
            this.lblAppointmentCount.TabIndex = 1;
            this.lblAppointmentCount.Text = "0";
            this.lblAppointmentCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAppointmentsTitle
            // 
            this.lblAppointmentsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAppointmentsTitle.ForeColor = System.Drawing.Color.White;
            this.lblAppointmentsTitle.Location = new System.Drawing.Point(9, 9);
            this.lblAppointmentsTitle.Name = "lblAppointmentsTitle";
            this.lblAppointmentsTitle.Size = new System.Drawing.Size(214, 22);
            this.lblAppointmentsTitle.TabIndex = 0;
            this.lblAppointmentsTitle.Text = "Toplam Randevu";
            this.lblAppointmentsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFinancial
            // 
            this.panelFinancial.BackColor = System.Drawing.Color.White;
            this.panelFinancial.Controls.Add(this.lblNetProfit);
            this.panelFinancial.Controls.Add(this.lblTotalCost);
            this.panelFinancial.Controls.Add(this.lblTotalRevenue);
            this.panelFinancial.Controls.Add(this.lblRevenueTitle);
            this.panelFinancial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFinancial.Location = new System.Drawing.Point(0, 191);
            this.panelFinancial.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panelFinancial.Name = "panelFinancial";
            this.panelFinancial.Padding = new System.Windows.Forms.Padding(20);
            this.panelFinancial.Size = new System.Drawing.Size(998, 379);
            this.panelFinancial.TabIndex = 2;
            // 
            // lblNetProfit
            // 
            this.lblNetProfit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNetProfit.ForeColor = System.Drawing.Color.Green;
            this.lblNetProfit.Location = new System.Drawing.Point(23, 147);
            this.lblNetProfit.Name = "lblNetProfit";
            this.lblNetProfit.Size = new System.Drawing.Size(326, 17);
            this.lblNetProfit.TabIndex = 3;
            this.lblNetProfit.Text = "Net Kar/Zarar: 0,00 TL";
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalCost.ForeColor = System.Drawing.Color.Red;
            this.lblTotalCost.Location = new System.Drawing.Point(23, 130);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(326, 17);
            this.lblTotalCost.TabIndex = 2;
            this.lblTotalCost.Text = "Toplam Maliyet: 0,00 TL";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.lblTotalRevenue.Location = new System.Drawing.Point(23, 100);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(326, 28);
            this.lblTotalRevenue.TabIndex = 1;
            this.lblTotalRevenue.Text = "0,00 TL";
            this.lblTotalRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.AutoSize = true;
            this.lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRevenueTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblRevenueTitle.Location = new System.Drawing.Point(23, 74);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(127, 21);
            this.lblRevenueTitle.TabIndex = 0;
            this.lblRevenueTitle.Text = "Toplam Gelirler";
            // 
            // panelQuickActions
            // 
            this.panelQuickActions.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelQuickActions.Controls.Add(this.lblQuickActionsTitle);
            this.panelQuickActions.Controls.Add(this.btnQuickAddAppointment);
            this.panelQuickActions.Controls.Add(this.btnQuickAddCustomer);
            this.panelQuickActions.Controls.Add(this.btnViewAllAppointments);
            this.panelQuickActions.Controls.Add(this.btnViewAllCustomers);
            this.panelQuickActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelQuickActions.Location = new System.Drawing.Point(0, 191);
            this.panelQuickActions.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.panelQuickActions.Name = "panelQuickActions";
            this.panelQuickActions.Padding = new System.Windows.Forms.Padding(20);
            this.panelQuickActions.Size = new System.Drawing.Size(998, 100);
            this.panelQuickActions.TabIndex = 3;
            // 
            // lblQuickActionsTitle
            // 
            this.lblQuickActionsTitle.AutoSize = true;
            this.lblQuickActionsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblQuickActionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblQuickActionsTitle.Location = new System.Drawing.Point(9, 9);
            this.lblQuickActionsTitle.Name = "lblQuickActionsTitle";
            this.lblQuickActionsTitle.Size = new System.Drawing.Size(110, 21);
            this.lblQuickActionsTitle.TabIndex = 0;
            this.lblQuickActionsTitle.Text = "Hızlı İşlemler";
            // 
            // btnQuickAddAppointment
            // 
            this.btnQuickAddAppointment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnQuickAddAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickAddAppointment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickAddAppointment.ForeColor = System.Drawing.Color.White;
            this.btnQuickAddAppointment.Location = new System.Drawing.Point(9, 35);
            this.btnQuickAddAppointment.Name = "btnQuickAddAppointment";
            this.btnQuickAddAppointment.Size = new System.Drawing.Size(129, 26);
            this.btnQuickAddAppointment.TabIndex = 1;
            this.btnQuickAddAppointment.Text = "Yeni Randevu";
            this.btnQuickAddAppointment.UseVisualStyleBackColor = false;
            this.btnQuickAddAppointment.Click += new System.EventHandler(this.btnGoToRandevular_Click);
            // 
            // btnQuickAddCustomer
            // 
            this.btnQuickAddCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnQuickAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickAddCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickAddCustomer.ForeColor = System.Drawing.Color.White;
            this.btnQuickAddCustomer.Location = new System.Drawing.Point(146, 35);
            this.btnQuickAddCustomer.Name = "btnQuickAddCustomer";
            this.btnQuickAddCustomer.Size = new System.Drawing.Size(129, 26);
            this.btnQuickAddCustomer.TabIndex = 2;
            this.btnQuickAddCustomer.Text = "Yeni Müşteri";
            this.btnQuickAddCustomer.UseVisualStyleBackColor = false;
            this.btnQuickAddCustomer.Click += new System.EventHandler(this.btnGoToMusteriler_Click);
            // 
            // btnViewAllAppointments
            // 
            this.btnViewAllAppointments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnViewAllAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllAppointments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewAllAppointments.ForeColor = System.Drawing.Color.White;
            this.btnViewAllAppointments.Location = new System.Drawing.Point(9, 69);
            this.btnViewAllAppointments.Name = "btnViewAllAppointments";
            this.btnViewAllAppointments.Size = new System.Drawing.Size(129, 26);
            this.btnViewAllAppointments.TabIndex = 3;
            this.btnViewAllAppointments.Text = "Tüm Randevular";
            this.btnViewAllAppointments.UseVisualStyleBackColor = false;
            this.btnViewAllAppointments.Click += new System.EventHandler(this.btnGoToRandevular_Click);
            // 
            // btnViewAllCustomers
            // 
            this.btnViewAllCustomers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnViewAllCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllCustomers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewAllCustomers.ForeColor = System.Drawing.Color.White;
            this.btnViewAllCustomers.Location = new System.Drawing.Point(146, 69);
            this.btnViewAllCustomers.Name = "btnViewAllCustomers";
            this.btnViewAllCustomers.Size = new System.Drawing.Size(129, 26);
            this.btnViewAllCustomers.TabIndex = 4;
            this.btnViewAllCustomers.Text = "Tüm Müşteriler";
            this.btnViewAllCustomers.UseVisualStyleBackColor = false;
            this.btnViewAllCustomers.Click += new System.EventHandler(this.btnGoToMusteriler_Click);
            // 
            // panelRecentAppointments
            // 
            this.panelRecentAppointments.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelRecentAppointments.Controls.Add(this.dgvRecentAppointments);
            this.panelRecentAppointments.Controls.Add(this.lblRecentAppointmentsTitle);
            this.panelRecentAppointments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecentAppointments.Location = new System.Drawing.Point(0, 291);
            this.panelRecentAppointments.Name = "panelRecentAppointments";
            this.panelRecentAppointments.Padding = new System.Windows.Forms.Padding(20, 10, 20, 20);
            this.panelRecentAppointments.Size = new System.Drawing.Size(998, 279);
            this.panelRecentAppointments.TabIndex = 4;
            // 
            // dgvRecentAppointments
            // 
            this.dgvRecentAppointments.AllowUserToAddRows = false;
            this.dgvRecentAppointments.AllowUserToDeleteRows = false;
            this.dgvRecentAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecentAppointments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecentAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 14F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecentAppointments.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecentAppointments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentAppointments.Location = new System.Drawing.Point(20, 40);
            this.dgvRecentAppointments.Name = "dgvRecentAppointments";
            this.dgvRecentAppointments.ReadOnly = true;
            this.dgvRecentAppointments.RowTemplate.Height = 36;
            this.dgvRecentAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentAppointments.Size = new System.Drawing.Size(958, 219);
            this.dgvRecentAppointments.TabIndex = 1;
            // 
            // lblRecentAppointmentsTitle
            // 
            this.lblRecentAppointmentsTitle.AutoSize = true;
            this.lblRecentAppointmentsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecentAppointmentsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentAppointmentsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblRecentAppointmentsTitle.Location = new System.Drawing.Point(20, 10);
            this.lblRecentAppointmentsTitle.Name = "lblRecentAppointmentsTitle";
            this.lblRecentAppointmentsTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.lblRecentAppointmentsTitle.Size = new System.Drawing.Size(130, 30);
            this.lblRecentAppointmentsTitle.TabIndex = 0;
            this.lblRecentAppointmentsTitle.Text = "Son Randevular";
            // 
            // panelBottomContainer
            // 
            this.panelBottomContainer.Controls.Add(this.panelRightBottom);
            this.panelBottomContainer.Controls.Add(this.panelLeftBottom);
            this.panelBottomContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomContainer.Location = new System.Drawing.Point(0, 0);
            this.panelBottomContainer.Name = "panelBottomContainer";
            this.panelBottomContainer.Size = new System.Drawing.Size(998, 570);
            this.panelBottomContainer.TabIndex = 5;
            // 
            // panelRightBottom
            // 
            this.panelRightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightBottom.Location = new System.Drawing.Point(343, 0);
            this.panelRightBottom.Name = "panelRightBottom";
            this.panelRightBottom.Size = new System.Drawing.Size(655, 570);
            this.panelRightBottom.TabIndex = 1;
            // 
            // panelLeftBottom
            // 
            this.panelLeftBottom.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeftBottom.Location = new System.Drawing.Point(0, 0);
            this.panelLeftBottom.Name = "panelLeftBottom";
            this.panelLeftBottom.Size = new System.Drawing.Size(343, 570);
            this.panelLeftBottom.TabIndex = 0;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(998, 570);
            this.Controls.Add(this.panelRecentAppointments);
            this.Controls.Add(this.panelQuickActions);
            this.Controls.Add(this.panelFinancial);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelBottomContainer);
            this.Name = "DashboardForm";
            this.Text = "Genel Durum";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelStat4.ResumeLayout(false);
            this.panelStat3.ResumeLayout(false);
            this.panelStat2.ResumeLayout(false);
            this.panelStat1.ResumeLayout(false);
            this.panelFinancial.ResumeLayout(false);
            this.panelFinancial.PerformLayout();
            this.panelQuickActions.ResumeLayout(false);
            this.panelQuickActions.PerformLayout();
            this.panelRecentAppointments.ResumeLayout(false);
            this.panelRecentAppointments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentAppointments)).EndInit();
            this.panelBottomContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
