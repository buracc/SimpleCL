using System;
using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Skills;
using Timer = System.Timers.Timer;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        private const ushort GatewayPort = 15779;

        private readonly ToolTip _toolTip = new();

        public readonly BindingList<CharacterSkill> SelectedSkills = new();
        public readonly BindingList<ITargetable> SelectedEntities = new();

        public Gui()
        {
            base.DoubleBuffered = true;
            InitializeComponent();

            FormClosed += ExitApplication;

            var local = LocalPlayer.Get;

            var timer = new Timer(100);
            timer.Elapsed += (_, _) =>
            {
                if (local.Uid == 0)
                {
                    return;
                }

                RefreshMap();
            };
            timer.Start();

            foreach (var server in SilkroadServer.Values)
            {
                serverComboBox.Items.Add(server);
            }

            serverComboBox.SelectedIndex = 0;

            usernameBox.Text = Credentials.Username;
            passwordBox.Text = Credentials.Password;

            availSkillsListBox.DataSource = local.Skills;
            attackSkillsListBox.DataSource = SelectedSkills;
            attackEntitiesListBox.DataSource = SelectedEntities;
            nearEntitiesListBox.DataSource = Entities.TargetableEntities;

            buffsDataGridView.DataSource = local.Buffs;
            InitBuffsGrid();

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