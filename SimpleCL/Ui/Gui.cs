using System;
using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Skills;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        private const ushort GatewayPort = 15779;

        private readonly ToolTip _toolTip = new();

        public readonly BindingList<CharacterSkill> SelectedSkills = new();
        public readonly BindingList<ITargetable> SelectedEntities = new();

        private readonly LocalPlayer _localPlayer;

        public Gui()
        {
            base.DoubleBuffered = true;
            InitializeComponent();

            FormClosed += ExitApplication;

            _localPlayer = LocalPlayer.Get;

            foreach (var server in SilkroadServer.Values)
            {
                serverComboBox.Items.Add(server);
            }

            serverComboBox.SelectedIndex = 0;

            usernameBox.Text = Credentials.Username;
            passwordBox.Text = Credentials.Password;

            InitAttackTab();
            InitBuffsGrid();
            InitInventories();
            InitMapTimers();
            
            CenterToScreen();
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

        private void ExitApplication(object sender, EventArgs e)
        {
            GameDatabase.Get.CacheData();

            Application.Exit();
            Environment.Exit(0);
        }

        public bool DebugGateway()
        {
            return debugGwCheckbox.Checked;
        }

        public bool DebugAgent()
        {
            return debugAgCheckbox.Checked;
        }
    }
}