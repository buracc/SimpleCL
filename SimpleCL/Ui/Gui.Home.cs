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
        private void LoginClicked(object sender, EventArgs e)
        {
            if (serverComboBox.SelectedItem is not SilkroadServer selectedServer)
            {
                return;
            }

            GameDatabase.Get.SelectedServer = selectedServer;

            var gw = new Gateway(selectedServer.GatewayIps[new Random().Next(selectedServer.GatewayIps.Length)],
                GatewayPort);
            gw.RegisterService(new LoginService(usernameBox.Text, passwordBox.Text, selectedServer));
            gw.Start();

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