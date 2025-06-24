using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace berberdenemeson
{
    public partial class ConnectionStatusForm : Form
    {
        private readonly ConnectionManager _connectionManager;

        public ConnectionStatusForm()
        {
            InitializeComponent();
            _connectionManager = ConnectionManager.Instance;
            
            // Event handler'ları ekle
            btnCheckConnection.Click += btnCheckConnection_Click;
            btnSync.Click += btnSync_Click;
            this.Load += ConnectionStatusForm_Load;
        }

        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            var isOnline = _connectionManager.CheckConnectionAsync();
            UpdateStatus(isOnline);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            btnSync.Enabled = false;
            btnSync.Text = "Senkronize Ediliyor...";
            
            Task.Run(async () =>
            {
                try
                {
                    await _connectionManager.SyncPendingOperationsAsync();
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnSync.Enabled = true;
                        btnSync.Text = "Senkronize Et";
                        MessageBox.Show("Senkronizasyon tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnSync.Enabled = true;
                        btnSync.Text = "Senkronize Et";
                        MessageBox.Show($"Senkronizasyon hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            });
        }

        private void UpdateStatus(bool isOnline = true)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => UpdateStatus(isOnline)));
                return;
            }

            lblStatus.Text = isOnline ? "Çevrimiçi" : "Çevrimdışı";
            lblStatus.ForeColor = isOnline ? Color.Green : Color.Red;
            btnSync.Enabled = isOnline;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void ConnectionStatusForm_Load(object sender, EventArgs e)
        {
            var isOnline = _connectionManager.CheckConnectionAsync();
            UpdateStatus(isOnline);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
} 