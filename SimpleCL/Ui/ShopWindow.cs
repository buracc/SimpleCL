using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Items;

namespace SimpleCL.Ui
{
    public partial class ShopWindow : Form
    {
        private readonly Shop _shop;
        private bool ClosedByUser { get; set; }

        public ShopWindow(Shop shop)
        {
            _shop = shop;
            InitializeComponent();
            
            InitShopItemEvents();

            shopDataGridView.DataSource = shop.Items;
            foreach (DataGridViewColumn column in shopDataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            
            CenterToScreen();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112 && m.WParam.ToInt32() == 0xF060)
            {
                ClosedByUser = true;
            }

            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!ClosedByUser)
            {
                return;
            }

            e.Cancel = true;
            _shop.Close();
            ClosedByUser = false;
        }
        
        private void InitShopItemEvents()
        {
            var purchaseMenu = new ContextMenuStrip();
            var purchaseMenuitem = new ToolStripMenuItem
            {
                Text = "Purchase"
            };
            
            purchaseMenuitem.Click += (_, _) =>
            {
                var currentCell = shopDataGridView.CurrentCell;
                if (currentCell == null)
                {
                    return;
                }
                
                var currRow = shopDataGridView.Rows[currentCell.RowIndex];
                var selectedItem = (ShopItem) currRow.DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                var input = Microsoft.VisualBasic.Interaction.InputBox("Enter amount to buy", "Purchase");

                try
                {
                    selectedItem.Purchase(ushort.Parse(input));
                }
                catch
                {
                    // Ignored
                }
            };

            purchaseMenu.Items.Add(purchaseMenuitem);
            
            shopDataGridView.CellContextMenuStripNeeded += (_, args) =>
            {
                if (args.RowIndex <= -1 || args.ColumnIndex <= -1)
                {
                    return;
                }
                
                shopDataGridView.CurrentCell = shopDataGridView.Rows[args.RowIndex].Cells[args.ColumnIndex];
                args.ContextMenuStrip = purchaseMenu;
            };
        }
    }
}