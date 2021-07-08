using System.ComponentModel;

namespace SimpleCL.Ui
{
    partial class ShopWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shopDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize) (this.shopDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // shopDataGridView
            // 
            this.shopDataGridView.AllowUserToAddRows = false;
            this.shopDataGridView.AllowUserToDeleteRows = false;
            this.shopDataGridView.AllowUserToResizeColumns = false;
            this.shopDataGridView.AllowUserToResizeRows = false;
            this.shopDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.shopDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.shopDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.shopDataGridView.Location = new System.Drawing.Point(12, 12);
            this.shopDataGridView.Name = "shopDataGridView";
            this.shopDataGridView.ReadOnly = true;
            this.shopDataGridView.RowHeadersVisible = false;
            this.shopDataGridView.Size = new System.Drawing.Size(430, 289);
            this.shopDataGridView.TabIndex = 0;
            // 
            // ShopWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(454, 313);
            this.Controls.Add(this.shopDataGridView);
            this.Name = "ShopWindow";
            this.Text = "ShopWindow";
            ((System.ComponentModel.ISupportInitialize) (this.shopDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView shopDataGridView;

        #endregion
    }
}