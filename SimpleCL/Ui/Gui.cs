using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SimpleCL.Model.Game;
using SimpleCL.Network;
using SimpleCL.Network.Enums;
using SimpleCL.Service.Login;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        public Character Character = null;
        
        private readonly List<string> _ggGateways = new List<string>
        {
            "94.199.103.68",
            "94.199.103.69",
            "94.199.103.70"
        };

        private const ushort GgPort = 15779;
        
        public Gui()
        {
            InitializeComponent();

            FormClosed += ExitApplication;

            foreach (string ip in _ggGateways)
            {
                gatewayComboBox.Items.Add(ip);
            }

            gatewayComboBox.SelectedIndex = new Random().Next(gatewayComboBox.Items.Count);

            foreach (Locale locale in Enum.GetValues(typeof(Locale)))
            {
                localeComboBox.Items.Add(locale);
            }

            localeComboBox.SelectedItem = Locale.SRO_TR_Official_GameGami;

            usernameBox.Text = Credentials.Username;
            passwordBox.Text = Credentials.Password;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            Gateway gw = new Gateway(gatewayComboBox.SelectedItem as string, GgPort);
            gw.RegisterService(new LoginService(usernameBox.Text, passwordBox.Text, (Locale) localeComboBox.SelectedItem));
            gw.Debug = true;
            gw.Start();
            
            ToggleControls(false);
        }

        public void ToggleControls(bool enabled)
        {
            usernameBox.Enabled = enabled;
            passwordBox.Enabled = enabled;
            loginButton.Enabled = enabled;
            gatewayComboBox.Enabled = enabled;
            localeComboBox.Enabled = enabled;
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