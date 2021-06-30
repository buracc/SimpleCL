using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Pet;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Network;
using SimpleCL.Services.Login;
using SimpleCL.Ui.Components;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    public partial class Gui : Form
    {
        private const ushort GatewayPort = 15779;

        private readonly ToolTip _toolTip = new ToolTip();

        public Gui()
        {
            base.DoubleBuffered = true;
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

                Text = "SimpleCL (" + local.Uid + ")";

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
        }

        public bool DebugGateway()
        {
            return debugGwCheckbox.Checked;
        }

        public bool DebugAgent()
        {
            return debugAgCheckbox.Checked;
        }

        public void AddMinimapMarker(Entity entity)
        {
            MapControl marker = new MapControl();
            _toolTip.SetToolTip(marker, entity.ToString());

            switch (entity)
            {
                case TalkNpc talkNpc:
                    marker.Image = Properties.Resources.mm_sign_npc;
                    break;

                case Monster monster:
                    marker.Image = Properties.Resources.mm_sign_monster;
                    break;

                case LocalPlayer localPlayer:
                    marker.Image = Properties.Resources.mm_sign_character;
                    _toolTip.SetToolTip(marker, "We are here");
                    break;

                case Player player:
                    marker.Image = Properties.Resources.mm_sign_otherplayer;
                    break;

                case PickPet pickPet:
                case AttackPet attackPet:
                case FellowPet fellowPet:
                case Horse horse:
                    marker.Image = Properties.Resources.mm_sign_animal;
                    break;

                case Teleport teleporter:
                    marker.Image = Properties.Resources.xy_gate;
                    if (teleporter.Links.IsNotEmpty())
                    {
                        ContextMenuStrip teleportMenu = new ContextMenuStrip();
                        foreach (TeleportLink teleportLink in teleporter.Links)
                        {
                            ToolStripMenuItem menuitem = new ToolStripMenuItem
                            {
                                Text = teleportLink.Name,
                                Name = teleportLink.DestinationId.ToString(),
                                Tag = teleporter
                            };
                            
                            menuitem.Click += (sender, args) => teleportLink.Teleport(teleporter);
                            
                            teleportMenu.Items.Add(
                                menuitem
                            );
                        }

                        marker.ContextMenuStrip = teleportMenu;
                    }

                    break;
            }

            if (marker.Image != null)
            {
                marker.Size = marker.Image.Size;

                Point location = minimap.GetPoint(entity.WorldPoint);
                location.X -= marker.Image.Size.Width / 2;
                location.Y -= marker.Image.Size.Height / 2;
                marker.Location = location;

                marker.Tag = entity;

                minimap.AddMarker(entity.Uid, marker);
            }
        }

        public void RefreshMap()
        {
            try
            {
                if (!mapPanel.Visible)
                {
                    return;
                }

                minimap.SetView(LocalPlayer.Get.WorldPoint);

                if (!minimap.Markers.ContainsKey(LocalPlayer.Get.Uid))
                {
                    return;
                }

                var marker = minimap.Markers[LocalPlayer.Get.Uid];
                var image = Properties.Resources.mm_sign_character;
                var rotated = new Bitmap(image.Width, image.Height);
                var graphics = Graphics.FromImage(rotated);
                graphics.TranslateTransform((float) rotated.Width / 2, (float) rotated.Height / 2);
                graphics.RotateTransform(-LocalPlayer.Get.GetAngleDegrees());
                graphics.TranslateTransform(-(float) rotated.Width / 2, -(float) rotated.Height / 2);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, new Point(0, 0));
                graphics.Dispose();

                marker.InvokeLater(() => { marker.Image = rotated; });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ClearMarkers()
        {
            minimap.ClearMarkers();
        }

        public void RefreshMarkers()
        {
            minimap.UpdateMarkerLocations();
        }

        public void RemoveMinimapMarker(uint uid)
        {
            minimap.RemoveMarker(uid);
        }
    }
}