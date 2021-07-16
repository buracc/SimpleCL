using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Enums.Events;
using SimpleCL.Models.Items;
using SimpleCL.Models.Items.Consumables;
using SimpleCL.Models.Items.Equipables;
using SimpleCL.Models.Items.JobEquipables;
using SimpleCL.Models.Items.Summons;

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

            refreshInventoriesButton.Click += RefreshClicked;

            inventoryDataGridView.CellFormatting += GridToolTip;
            equipmentDataGridView.CellFormatting += GridToolTip;
            avatarDataGridView.CellFormatting += GridToolTip;
            jobEquipmentDataGridView.CellFormatting += GridToolTip;
        }

        private void GridToolTip(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((DataGridView) sender).Rows[e.RowIndex];

            if (row.DataBoundItem is not InventoryItem inventoryItem)
            {
                return;
            }

            row.Cells[e.ColumnIndex].ToolTipText = $"{inventoryItem} [{inventoryItem.GetType().Name}]";
        }

        private void InitEquipment()
        {
            equipmentDataGridView.DataSource = _localPlayer.EquipmentInventory;

            equipmentDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                equipmentDataGridView.CurrentCell = equipmentDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = GetUnEquipMenu(equipmentDataGridView);
            };
        }

        private void InitJobEquipment()
        {
            jobEquipmentDataGridView.DataSource = _localPlayer.JobEquipmentInventory;

            jobEquipmentDataGridView.CellContextMenuStripNeeded += (sender, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                jobEquipmentDataGridView.CurrentCell =
                    jobEquipmentDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = GetUnEquipMenu(jobEquipmentDataGridView);
            };
        }

        private void InitAvatars()
        {
            avatarDataGridView.DataSource = _localPlayer.AvatarInventory;

            avatarDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                avatarDataGridView.CurrentCell = avatarDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = GetUnEquipMenu(avatarDataGridView);
            };
        }

        private void InitInventory()
        {
            inventoryDataGridView.DataSource = _localPlayer.Inventory;

            inventoryDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }

                var currCell = inventoryDataGridView.CurrentCell =
                    inventoryDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];

                switch (currCell.OwningRow.DataBoundItem)
                {
                    case JobEquipment:
                    case Avatar:
                    case Equipment:
                        args.ContextMenuStrip = GetEquipItemMenu();
                        return;
                    case Summon:
                    case Consumable:
                        args.ContextMenuStrip = GetUseItemMenu();
                        break;
                }
            };
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            RefreshInventories();
        }

        public void RefreshInventories()
        {
            inventoryDataGridView.Refresh();
            equipmentDataGridView.Refresh();
            avatarDataGridView.Refresh();
            jobEquipmentDataGridView.Refresh();
        }

        private ContextMenuStrip GetUseItemMenu()
        {
            var moveMenuitem = new ToolStripMenuItem("Move");
            moveMenuitem.Click += (_, _) =>
            {
                var currentCell = inventoryDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = inventoryDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = currRow.DataBoundItem;
                if (selectedItem is not InventoryItem inventoryItem)
                {
                    return;
                }

                var quantityInput = Microsoft.VisualBasic.Interaction.InputBox("Enter amount (0 for all)", "Move item");
                var slot = Microsoft.VisualBasic.Interaction.InputBox("Enter slot", "Move item");

                try
                {
                    var quantity = ushort.Parse(quantityInput);
                    inventoryItem.Move(inventoryItem.Slot, byte.Parse(slot),
                        quantity: quantity == 0 ? inventoryItem.Quantity : quantity);
                }
                catch
                {
                    // Ignored
                }
            };

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
                switch (selectedItem)
                {
                    case Consumable consumable:
                        consumable.Use();
                        break;
                    case Summon summon:
                        summon.Call();
                        break;
                }
            };

            useItemMenuStrip.Items.Add(useMenuItem);
            useItemMenuStrip.Items.Add(moveMenuitem);
            return useItemMenuStrip;
        }

        private ContextMenuStrip GetEquipItemMenu()
        {
            var moveMenuitem = new ToolStripMenuItem("Move");
            moveMenuitem.Click += (_, _) =>
            {
                var currentCell = inventoryDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = inventoryDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = currRow.DataBoundItem;
                if (selectedItem is not InventoryItem inventoryItem)
                {
                    return;
                }

                var input = Microsoft.VisualBasic.Interaction.InputBox("Enter slot", "Move item");

                try
                {
                    inventoryItem.Move(inventoryItem.Slot, byte.Parse(input), quantity: inventoryItem.Quantity);
                }
                catch
                {
                    // Ignored
                }
            };

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

                switch (selectedItem)
                {
                    case JobEquipment jobEquipment:
                        jobEquipment.Equip();
                        break;
                    case Avatar avatar:
                        avatar.Equip();
                        break;
                    case Equipment equipment:
                        equipment.Equip();
                        break;
                }
            };

            equipMenuStrip.Items.Add(equipMenuItem);
            equipMenuStrip.Items.Add(moveMenuitem);
            return equipMenuStrip;
        }

        public ContextMenuStrip GetUnEquipMenu(DataGridView grid)
        {
            var unequipMenu = new ContextMenuStrip();
            var unequipMenuItem = new ToolStripMenuItem
            {
                Text = "Unequip"
            };
            unequipMenuItem.Click += (_, _) =>
            {
                var currentCell = grid.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = grid.Rows[currentCell.RowIndex];
                var selectedItem = currRow.DataBoundItem;
                switch (selectedItem)
                {
                    case JobEquipment jobEquipment:
                        jobEquipment.UnEquip();
                        break;
                    case Avatar avatar:
                        avatar.UnEquip();
                        break;
                    case Equipment equipment:
                        equipment.UnEquip();
                        break;
                    case Consumable cons:
                        cons.Move(cons.Slot, 0x0D);
                        break;
                }
            };

            unequipMenu.Items.Add(unequipMenuItem);

            var moveMenuitem = new ToolStripMenuItem("Move");
            moveMenuitem.Click += (_, _) =>
            {
                var currentCell = grid.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }

                var currRow = grid.Rows[currentCell.RowIndex];
                var selectedItem = currRow.DataBoundItem;
                if (selectedItem is not InventoryItem inventoryItem)
                {
                    return;
                }

                var input = Microsoft.VisualBasic.Interaction.InputBox("Enter slot", "Move item");

                try
                {
                    var action = selectedItem switch
                    {
                        JobEquipment job => _localPlayer.Inventory.Contains(job)
                            ? InventoryAction.InventoryToJob
                            : InventoryAction.JobToInventory,
                        Avatar avatar => _localPlayer.Inventory.Contains(avatar)
                            ? InventoryAction.InventoryToAvatar
                            : InventoryAction.AvatarToInventory,
                        _ => InventoryAction.InventoryToInventory
                    };

                    inventoryItem.Move(inventoryItem.Slot, byte.Parse(input), action, inventoryItem.Quantity);
                }
                catch
                {
                    // Ignored
                }
            };

            unequipMenu.Items.Add(moveMenuitem);
            return unequipMenu;
        }
    }
}