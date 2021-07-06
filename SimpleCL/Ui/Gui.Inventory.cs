using System;
using System.Windows.Forms;
using SimpleCL.Models.Items;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        private void InitInventories()
        {
            InitInventory();
            InitEquipment();
            InitJobEquipment();
            InitAvatars();
            refreshInventoriesButton.Click += RefreshInventories;
        }

        // todo: add unequiping equipment
        private void InitEquipment()
        {
            equipmentDataGridView.DataSource = _localPlayer.EquipmentInventory;
        }

        private void InitJobEquipment()
        {
            jobEquipmentDataGridView.DataSource = _localPlayer.JobEquipmentInventory;
        }

        private void InitAvatars()
        {
            avatarDataGridView.DataSource = _localPlayer.AvatarInventory;
        }

        private void InitInventory()
        {
            inventoryDataGridView.DataSource = _localPlayer.Inventory;

            var useItemMenuStrip = new ContextMenuStrip();
            var useMenuItem = new ToolStripMenuItem
            {
                Text = "Use item"
            };
            useMenuItem.Click += (_, _) =>
            {
                var currentCell = inventoryDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = inventoryDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = (InventoryItem) currRow.DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                selectedItem.Use();
            };

            useItemMenuStrip.Items.Add(useMenuItem);

            inventoryDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                inventoryDataGridView.CurrentCell = inventoryDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = useItemMenuStrip;
            };
        }

        private void RefreshInventories(object sender, EventArgs e)
        {
            inventoryDataGridView.Refresh();
            equipmentDataGridView.Refresh();
            avatarDataGridView.Refresh();
            jobEquipmentDataGridView.Refresh();
        }
    }
}