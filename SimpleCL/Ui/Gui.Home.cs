using System;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Skills;
using SimpleCL.Network;
using SimpleCL.Services.Login;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        private void InitHome()
        {
            // proxyIpTextBox.Text = Credentials.ProxyIp;
            // proxyPortTextBox.Text = Credentials.ProxyPort.ToString();
            // proxyUsernameBox.Text = Credentials.ProxyUsername;
            // proxyPasswordBox.Text = Credentials.ProxyPassword;
            
            InitBuffsGrid();
        }
        
        private void LoginClicked(object sender, EventArgs e)
        {
            if (serverComboBox.SelectedItem is not SilkroadServer selectedServer)
            {
                return;
            }

            GameDatabase.Get.SelectedServer = selectedServer;

            var proxyIp = proxyIpTextBox.TextLength == 0 ? null : proxyIpTextBox.Text;
            var proxyPort = proxyPortTextBox.TextLength == 0 ? 0 : int.Parse(proxyPortTextBox.Text);
            var proxyUser = proxyUsernameBox.TextLength == 0 ? null : proxyUsernameBox.Text;
            var proxyPass = proxyPasswordBox.TextLength == 0 ? null : proxyPasswordBox.Text;

            var gw = new Gateway(selectedServer.GatewayIps[new Random().Next(selectedServer.GatewayIps.Length)],
                GatewayPort, proxyIp, proxyPort, proxyUser, proxyPass, clientCheckbox.Checked, selectedServer.Locale);
            gw.RegisterService(new LoginService(usernameBox.Text, passwordBox.Text, selectedServer, gw));
            if (!clientCheckbox.Checked)
            {
                gw.Start();
            }
            
            ToggleControls(false);
        }

        private void InitBuffsGrid()
        {
            buffsDataGridView.DataSource = _localPlayer.Buffs;
            
            var deleteMenuStrip = new ContextMenuStrip();
            var deleteMenuItem = new ToolStripMenuItem
            {
                Text = "Cancel buff"
            };
            deleteMenuItem.Click += (_, _) =>
            {
                var currentCell = buffsDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }
                
                var currRow = buffsDataGridView.Rows[currentCell.RowIndex];
                var selectedBuff = (Buff) currRow.DataBoundItem;
                if (selectedBuff == null)
                {
                    return;
                }

                selectedBuff.Cancel();
            };

            deleteMenuStrip.Items.Add(deleteMenuItem);
            
            buffsDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }
                
                buffsDataGridView.CurrentCell = buffsDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = deleteMenuStrip;
            };
        }
    }
}