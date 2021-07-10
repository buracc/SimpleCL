using System;
using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Skills;
using SimpleCL.Ui.Components;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        private const ushort GatewayPort = 15779;

        private readonly ToolTip _toolTip = new();

        public readonly BindingList<CharacterSkill> SelectedSkills = new();
        public readonly BindingList<ITargetable> SelectedEntities = new();

        private readonly LocalPlayer _localPlayer;

        private bool _dataBound;

        public Gui()
        {
            _localPlayer = LocalPlayer.Get;

            base.DoubleBuffered = true;
            InitializeComponent();

            FormClosed += ExitApplication;

            foreach (var server in SilkroadServer.Values)
            {
                serverComboBox.Items.Add(server);
            }

            serverComboBox.SelectedIndex = 0;
            GameDatabase.Get.SelectedServer = (SilkroadServer) serverComboBox.SelectedItem;

            usernameBox.Text = Credentials.Username;
            passwordBox.Text = Credentials.Password;

            InitAttackTab();
            InitBuffsGrid();
            InitInventories();
            InitMap();

            CenterToScreen();
        }

        public void InitBindings()
        {
            if (_dataBound)
            {
                return;
            }

            this.InvokeLater(() =>
            {
                nameLabelValue.DataBindings.Add("Text", _localPlayer, "Name");
                jobNameLabelValue.DataBindings.Add("Text", _localPlayer, "JobName");

                hpProgressBar.DataBindings.Add("Maximum", _localPlayer, "MaxHp");
                mpProgressBar.DataBindings.Add("Maximum", _localPlayer, "MaxMp");
                hpProgressBar.DataBindings.Add("Value", _localPlayer, "Hp");
                mpProgressBar.DataBindings.Add("Value", _localPlayer, "Mp");

                expProgressBar.DataBindings.Add("Value", _localPlayer, "ExpPercent");
                expProgressBar.DataBindings.Add("CustomText", _localPlayer, "ExpPercentString");
                jobExpProgressBar.DataBindings.Add("Value", _localPlayer, "JobExpPercent");
                jobExpProgressBar.DataBindings.Add("CustomText", _localPlayer, "JobExpPercentString");

                levelLabelValue.DataBindings.Add("Text", _localPlayer, "Level");
                jobLevelLabelValue.DataBindings.Add("Text", _localPlayer, "JobLevel");

                spLabelValue.DataBindings.Add("Text", _localPlayer, "SkillpointsString");
                goldLabelValue.DataBindings.Add("Text", _localPlayer, "GoldString");
                goldAmountLabel.DataBindings.Add("Text", _localPlayer, "GoldString");

                localCoordsLabelValue.DataBindings.Add("Text", _localPlayer, "LocalPoint");
                currLocalLabelValue.DataBindings.Add("Text", _localPlayer, "LocalPoint");

                worldCoordsLabelValue.DataBindings.Add("Text", _localPlayer, "WorldPoint");
                currWorldLabelValue.DataBindings.Add("Text", _localPlayer, "WorldPoint");
            });

            _dataBound = true;
        }

        public void ToggleControls(bool enabled)
        {
            this.InvokeLater(() =>
            {
                credentialsGroup.Enabled = enabled;
            });
        }

        public void Log(string message)
        {
            this.InvokeLater(() =>
                {
                    loggerBox.Items.Add(message);
                    loggerBox.SelectedIndex = loggerBox.Items.Count - 1;
                    loggerBox.SelectedIndex = -1;
                }
            );
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

        public Map GetMap()
        {
            return minimap;
        }

        public void ClearMarkers()
        {
            minimap.ClearMarkers();
        }

        public void ClearTiles()
        {
            minimap.RemoveTiles();
        }
    }
}