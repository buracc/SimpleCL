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
            this.label1 = new System.Windows.Forms.Label();
            this.stallOwnerBox = new System.Windows.Forms.TextBox();
            this.stallStatusBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.exitStallButton = new System.Windows.Forms.Button();
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
            this.stallItemsDataGridView.Size = new System.Drawing.Size(383, 257);
            this.stallItemsDataGridView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stallItemsDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 282);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stall items";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stallDescriptionRtb);
            this.groupBox2.Location = new System.Drawing.Point(12, 342);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 63);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Description";
            // 
            // stallDescriptionRtb
            // 
            this.stallDescriptionRtb.Location = new System.Drawing.Point(6, 19);
            this.stallDescriptionRtb.Name = "stallDescriptionRtb";
            this.stallDescriptionRtb.Size = new System.Drawing.Size(299, 38);
            this.stallDescriptionRtb.TabIndex = 0;
            this.stallDescriptionRtb.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Owner";
            // 
            // stallOwnerBox
            // 
            this.stallOwnerBox.Location = new System.Drawing.Point(83, 9);
            this.stallOwnerBox.Name = "stallOwnerBox";
            this.stallOwnerBox.ReadOnly = true;
            this.stallOwnerBox.Size = new System.Drawing.Size(100, 20);
            this.stallOwnerBox.TabIndex = 1;
            // 
            // stallStatusBox
            // 
            this.stallStatusBox.Location = new System.Drawing.Point(293, 9);
            this.stallStatusBox.Name = "stallStatusBox";
            this.stallStatusBox.ReadOnly = true;
            this.stallStatusBox.Size = new System.Drawing.Size(100, 20);
            this.stallStatusBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(222, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Status";
            // 
            // exitStallButton
            // 
            this.exitStallButton.Location = new System.Drawing.Point(332, 347);
            this.exitStallButton.Name = "exitStallButton";
            this.exitStallButton.Size = new System.Drawing.Size(75, 58);
            this.exitStallButton.TabIndex = 4;
            this.exitStallButton.Text = "Exit";
            this.exitStallButton.UseVisualStyleBackColor = true;
            this.exitStallButton.Click += new System.EventHandler(this.ExitStallClick);
            // 
            // StallWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(420, 417);
            this.ControlBox = false;
            this.Controls.Add(this.exitStallButton);
            this.Controls.Add(this.stallStatusBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stallOwnerBox);
            this.Name = "StallWindow";
            this.Text = "StallWindow";
            ((System.ComponentModel.ISupportInitialize) (this.stallItemsDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button exitStallButton;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox stallOwnerBox;
        private System.Windows.Forms.TextBox stallStatusBox;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox stallDescriptionRtb;

        private System.Windows.Forms.DataGridView stallItemsDataGridView;

        #endregion
    }
}