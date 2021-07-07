using System.ComponentModel;

namespace SimpleCL.Ui
{
    partial class StallWindow
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
            this.stallItemsDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stallDescriptionRtb = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize) (this.stallItemsDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // stallItemsDataGridView
            // 
            this.stallItemsDataGridView.AllowUserToAddRows = false;
            this.stallItemsDataGridView.AllowUserToDeleteRows = false;
            this.stallItemsDataGridView.AllowUserToResizeColumns = false;
            this.stallItemsDataGridView.AllowUserToResizeRows = false;
            this.stallItemsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.stallItemsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stallItemsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.stallItemsDataGridView.MultiSelect = false;
            this.stallItemsDataGridView.Name = "stallItemsDataGridView";
            this.stallItemsDataGridView.ReadOnly = true;
            this.stallItemsDataGridView.Size = new System.Drawing.Size(383, 262);
            this.stallItemsDataGridView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stallItemsDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 287);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stall items";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stallDescriptionRtb);
            this.groupBox2.Location = new System.Drawing.Point(12, 305);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 63);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Description";
            // 
            // stallDescriptionRtb
            // 
            this.stallDescriptionRtb.Location = new System.Drawing.Point(6, 19);
            this.stallDescriptionRtb.Name = "stallDescriptionRtb";
            this.stallDescriptionRtb.Size = new System.Drawing.Size(383, 38);
            this.stallDescriptionRtb.TabIndex = 0;
            this.stallDescriptionRtb.Text = "";
            // 
            // StallWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(420, 380);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "StallWindow";
            this.Text = "StallWindow";
            ((System.ComponentModel.ISupportInitialize) (this.stallItemsDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox stallDescriptionRtb;

        private System.Windows.Forms.DataGridView stallItemsDataGridView;

        private System.Windows.Forms.DataGridView dataGridView1;

        #endregion
    }
}