using System.ComponentModel;

namespace SimpleCL.Ui
{
    partial class CharacterSelect
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
            this.characterListDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize) (this.characterListDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // characterListDataGridView
            // 
            this.characterListDataGridView.AllowUserToAddRows = false;
            this.characterListDataGridView.AllowUserToDeleteRows = false;
            this.characterListDataGridView.AllowUserToResizeColumns = false;
            this.characterListDataGridView.AllowUserToResizeRows = false;
            this.characterListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.characterListDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.characterListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.characterListDataGridView.Location = new System.Drawing.Point(12, 12);
            this.characterListDataGridView.MultiSelect = false;
            this.characterListDataGridView.Name = "characterListDataGridView";
            this.characterListDataGridView.ReadOnly = true;
            this.characterListDataGridView.Size = new System.Drawing.Size(322, 206);
            this.characterListDataGridView.TabIndex = 1;
            // 
            // CharacterSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(346, 226);
            this.Controls.Add(this.characterListDataGridView);
            this.Name = "CharacterSelect";
            this.Text = "CharacterSelect";
            ((System.ComponentModel.ISupportInitialize) (this.characterListDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView characterListDataGridView;

        private System.Windows.Forms.DataGridView serverlistDataGridView;

        private System.Windows.Forms.DataGridView dataGridView1;

        #endregion
    }
}