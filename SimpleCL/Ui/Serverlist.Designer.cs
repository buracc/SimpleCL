using System.ComponentModel;

namespace SimpleCL.Ui
{
    partial class Serverlist
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
            this.serverlistDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize) (this.serverlistDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // serverlistDataGridView
            // 
            this.serverlistDataGridView.AllowUserToAddRows = false;
            this.serverlistDataGridView.AllowUserToDeleteRows = false;
            this.serverlistDataGridView.AllowUserToResizeColumns = false;
            this.serverlistDataGridView.AllowUserToResizeRows = false;
            this.serverlistDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.serverlistDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.serverlistDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serverlistDataGridView.Location = new System.Drawing.Point(12, 12);
            this.serverlistDataGridView.MultiSelect = false;
            this.serverlistDataGridView.Name = "serverlistDataGridView";
            this.serverlistDataGridView.ReadOnly = true;
            this.serverlistDataGridView.Size = new System.Drawing.Size(322, 206);
            this.serverlistDataGridView.TabIndex = 1;
            // 
            // Serverlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(346, 226);
            this.Controls.Add(this.serverlistDataGridView);
            this.Name = "Serverlist";
            this.Text = "Serverlist";
            ((System.ComponentModel.ISupportInitialize) (this.serverlistDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView serverlistDataGridView;

        private System.Windows.Forms.DataGridView dataGridView1;

        #endregion
    }
}