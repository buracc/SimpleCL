using System.Windows.Forms;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;

namespace SimpleCL.Ui
{
    public partial class StallWindow : Form
    {
        private readonly Player _player;
        
        public StallWindow(Player player)
        {
            InitializeComponent();
            
            _player = player;
            stallItemsDataGridView.DataSource = _player.Stall.Items;
            base.Text = "[" + _player.Name + "] " + _player.Stall.Title;
            stallDescriptionRtb.Text = _player.Stall.Description;

            FormClosed += (_, _) =>
            {
                _player.Stall.Leave();
            };
            
            InitStallItemEvents();
            CenterToScreen();
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
                var selectedBuff = (StallItem) currRow.DataBoundItem;
                if (selectedBuff == null)
                {
                    return;
                }

                selectedBuff.Purchase();
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
    }
}