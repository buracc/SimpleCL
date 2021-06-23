using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Model;
using SimpleCL.Network;
using SimpleCL.Service.Login;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        public Character Character = null;
        
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
            CenterToScreen();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            SilkroadServer selectedServer = serverComboBox.SelectedItem as SilkroadServer;
            if (selectedServer == null)
            {
                return;
            }

            GameDatabase.GetInstance().SelectedServer = selectedServer;
            
            Gateway gw = new Gateway(selectedServer.GatewayIps[new Random().Next(selectedServer.GatewayIps.Length)], GatewayPort);
            gw.RegisterService(new LoginService(usernameBox.Text, passwordBox.Text, selectedServer));
            gw.Debug = true;
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
            if (Character != null)
            {
                hpProgressBar.Value = (int) Character.getHpPercent();
                mpProgressBar.Value = (int) Character.getMpPercent();
                expProgressBar.Value = Character.GetExpPercent();

                Console.WriteLine(Character.GetExpPercent());

                levelLabelValue.Text = Character.Level.ToString();
                spLabelValue.Text = Character.Skillpoints.ToString("N0");
                goldLabelValue.Text = Character.Gold.ToString("N0");
                coordsLabelValue.Text = Character.Coordinates.ToString();
            }
            
            Refresh();
        }
    }
}