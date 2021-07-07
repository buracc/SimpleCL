using System;
using System.Windows.Forms;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    public partial class StallWindow : Form
    {
        private readonly Player _player;
        
        public StallWindow(Player player)
        {
            _player = player;
            
            this.InvokeLater(() =>
            {
                InitializeComponent();
                
                stallItemsDataGridView.DataSource = _player.Stall.Items;
                base.Text = _player.Stall.Title;
                stallDescriptionRtb.Text = _player.Stall.Description;

                stallOwnerBox.Text = _player.Name;
                stallStatusBox.DataBindings.Add("Text", player.Stall, "Status", false,
                    DataSourceUpdateMode.OnPropertyChanged);

                InitStallItemEvents();
                CenterToScreen();
                BringToFront();
            });
        }

        private void InitStallItemEvents()
        {
            var purchaseMenu = new ContextMenuStrip();
            var purchaseMenuitem = new ToolStripMenuItem
            {
                Text = "Purchase"
            };
            purchaseMenuitem.Click += (_, _) =>
            {
                var currentCell = stallItemsDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }
                
                var currRow = stallItemsDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = (StallItem) currRow.DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                selectedItem.Purchase();
            };

            purchaseMenu.Items.Add(purchaseMenuitem);
            
            stallItemsDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }
                
                stallItemsDataGridView.CurrentCell = stallItemsDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = purchaseMenu;
            };
        }

        private void ExitStallClick(object sender, EventArgs e)
        {
            _player.Stall.Leave();
        }
    }
}