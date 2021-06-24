using System;
using System.Globalization;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Model.Character;
using SimpleCL.Network;
using SimpleCL.Service.Login;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        private const ushort GatewayPort = 15779;
        
        public Gui()
        {
            InitializeComponent();

            FormClosed += ExitApplication;

            foreach (SilkroadServer server in SilkroadServer.Values)
            {
                serverComboBox.Items.Add(server);
            }

            serverComboBox.SelectedIndex = 0;

            usernameBox.Text = Credentials.Username;
            passwordBox.Text = Credentials.Password;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            
            inventoryDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            equipmentDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            avatarDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            jobEquipmentDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            CenterToScreen();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            SilkroadServer selectedServer = serverComboBox.SelectedItem as SilkroadServer;
            if (selectedServer == null)
            {
                return;
            }

            GameDatabase.Get.SelectedServer = selectedServer;
            
            Gateway gw = new Gateway(selectedServer.GatewayIps[new Random().Next(selectedServer.GatewayIps.Length)], GatewayPort);
            gw.RegisterService(new LoginService(usernameBox.Text, passwordBox.Text, selectedServer));
            // gw.Debug = true;
            gw.Start();
            
            ToggleControls(false);
        }

        public void ToggleControls(bool enabled)
        {
            usernameBox.Enabled = enabled;
            passwordBox.Enabled = enabled;
            loginButton.Enabled = enabled;
            serverComboBox.Enabled = enabled;
        }

        public void Log(string message)
        {
            loggerBox.Items.Add(message);
            loggerBox.SelectedIndex = loggerBox.Items.Count - 1;
            loggerBox.SelectedIndex = -1;
        }

        public void AddChatMessage(string message)
        {
            chatBox.Items.Add(message);
            chatBox.SelectedIndex = chatBox.Items.Count - 1;
            chatBox.SelectedIndex = -1;
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void RefreshGui()
        {
            LocalPlayer local = LocalPlayer.Get;
            if (local != null)
            {
                nameLabelValue.Text = local.Name;
                jobNameLabelValue.Text = local.JobName;
                
                hpProgressBar.Maximum = (int) local.Hp;
                mpProgressBar.Maximum = (int) local.Mp;
                expProgressBar.Value = (int) local.GetExpPercent();
                expProgressBar.CustomText = local.GetExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);
                jobExpProgressBar.Value = (int) local.GetJobExpPercent();
                jobExpProgressBar.CustomText = local.GetJobExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);

                levelLabelValue.Text = local.Level.ToString();
                jobLevelLabelValue.Text = local.JobLevel.ToString();
                spLabelValue.Text = local.Skillpoints.ToString("N0");
                goldLabelValue.Text = local.Gold.ToString("N0");
                coordsLabelValue.Text = local.LocalPoint.ToString();

                inventoryDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                inventoryDataGridView.DataSource = local.Inventories["inventory"];
                equipmentDataGridView.DataSource = local.Inventories["equipment"];
                avatarDataGridView.DataSource = local.Inventories["avatar"];
                jobEquipmentDataGridView.DataSource = local.Inventories["jobEquipment"];
            }
            
            Refresh();
        }
    }
}