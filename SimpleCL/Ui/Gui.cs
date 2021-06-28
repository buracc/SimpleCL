using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Model.Character;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity;
using SimpleCL.Model.Entity.Pet;
using SimpleCL.Network;
using SimpleCL.Service.Login;
using SimpleCL.Ui.Comp;
using SimpleCL.Util.Extension;

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
            if (!(serverComboBox.SelectedItem is SilkroadServer selectedServer))
            {
                return;
            }

            GameDatabase.Get.SelectedServer = selectedServer;

            Gateway gw = new Gateway(selectedServer.GatewayIps[new Random().Next(selectedServer.GatewayIps.Length)],
                GatewayPort);
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
            GameDatabase.Get.CacheData();

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

                Name = Name + "(" + local.Uid + ")";

                hpProgressBar.Maximum = (int) local.MaxHp;
                mpProgressBar.Maximum = (int) local.MaxMp;
                hpProgressBar.Value = (int) local.MaxHp;
                mpProgressBar.Value = (int) local.MaxMp;

                expProgressBar.Value = (int) local.GetExpPercent();
                expProgressBar.CustomText = local.GetExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);
                jobExpProgressBar.Value = (int) local.GetJobExpPercent();
                jobExpProgressBar.CustomText =
                    local.GetJobExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);

                levelLabelValue.Text = local.Level.ToString();
                jobLevelLabelValue.Text = local.JobLevel.ToString();
                spLabelValue.Text = local.Skillpoints.ToString("N0");
                goldLabelValue.Text = local.Gold.ToString("N0");

                var worldPoint = WorldPoint.FromLocal(local.LocalPoint);
                localCoordsLabelValue.Text = local.LocalPoint.ToString();
                worldCoordsLabelValue.Text = worldPoint.ToString();
                currLocalLabelValue.Text = local.LocalPoint.ToString();
                currWorldLabelValue.Text = worldPoint.ToString();

                inventoryDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                inventoryDataGridView.DataSource = local.Inventories["inventory"];
                equipmentDataGridView.DataSource = local.Inventories["equipment"];
                avatarDataGridView.DataSource = local.Inventories["avatar"];
                jobEquipmentDataGridView.DataSource = local.Inventories["jobEquipment"];
            }

            Refresh();
        }

        public bool DebugGateway()
        {
            return debugGwCheckbox.Checked;
        }

        public bool DebugAgent()
        {
            return debugAgCheckbox.Checked;
        }

        public void AddMinimapEntity(uint uid, Entity entity)
        {
            MapControl marker = new MapControl();
            switch (entity)
            {
                case TalkNpc talkNpc:
                    marker.Image = Properties.Resources.mm_sign_npc;
                    break;

                case Monster monster:
                    marker.Image = Properties.Resources.mm_sign_monster;
                    break;

                case Player player:
                    if (uid == LocalPlayer.Get.Uid)
                    {
                        marker.Image = Properties.Resources.mm_sign_character;
                    }
                    else
                    {
                        marker.Image = Properties.Resources.mm_sign_otherplayer;
                    }

                    break;
                case PickPet pickPet:
                case AttackPet attackPet:
                case FellowPet fellowPet:
                case Horse horse:
                    marker.Image = Properties.Resources.mm_sign_animal;
                    break;
                case Teleport teleport:
                    marker.Image = Properties.Resources.xy_gate;
                    break;
            }

            if (marker.Image != null)
            {
                marker.Size = marker.Image.Size;

                Point location = map1.GetPoint(entity.WorldPoint);
                location.X -= marker.Image.Size.Width / 2;
                location.Y -= marker.Image.Size.Height / 2;
                marker.Location = location;

                marker.Tag = entity;
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(marker, marker.Tag.ToString());
                
                map1.InvokeLater(() => { map1.AddMarker(uid, marker); });
            }
        }

        public void RemoveMinimapEntity(uint uid)
        {
            map1.InvokeLater(() =>
            {
                try
                {
                    map1.RemoveMarker(uid);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public void RefreshMap()
        {
            map1.InvokeLater(() => { map1.SetView(LocalPlayer.Get.WorldPoint); });
        }
    }
}