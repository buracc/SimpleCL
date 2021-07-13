using System;
using System.Windows.Forms;
using SimpleCL.Models.Items;
using SimpleCL.Models.Items.Consumables;
using SimpleCL.Models.Items.Equipables;

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
            
            var moveItemMenuStrip = new ContextMenuStrip();
            var unequipMenuItem = new ToolStripMenuItem
            {
                Text = "Unequip"
            };
            unequipMenuItem.Click += (_, _) =>
            {
                var currentCell = equipmentDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = equipmentDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = (Equipment) currRow.DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                selectedItem.UnEquip();
            };

            moveItemMenuStrip.Items.Add(unequipMenuItem);

            equipmentDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                equipmentDataGridView.CurrentCell = equipmentDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = moveItemMenuStrip;
            };
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
                var selectedItem = currRow.DataBoundItem;
                if (selectedItem is not Consumable consumable)
                {
                    return;
                }

                consumable.Use();
            };

            useItemMenuStrip.Items.Add(useMenuItem);
            
            var equipMenuStrip = new ContextMenuStrip();
            var equipMenuItem = new ToolStripMenuItem
            {
                Text = "Equip"
            };
            equipMenuItem.Click += (_, _) =>
            {
                var currentCell = inventoryDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = inventoryDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = currRow.DataBoundItem;
                if (selectedItem is not Equipment equipment)
                {
                    return;
                }

                equipment.Equip();
            };

            equipMenuStrip.Items.Add(equipMenuItem);

            inventoryDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                var currCell = inventoryDataGridView.CurrentCell = inventoryDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                if (currCell.OwningRow.DataBoundItem is Equipment)
                {
                    args.ContextMenuStrip = equipMenuStrip;
                    return;
                }

                if (currCell.OwningRow.DataBoundItem is Consumable)
                {
                    args.ContextMenuStrip = useItemMenuStrip;
                }
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