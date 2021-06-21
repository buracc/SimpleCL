using System.ComponentModel;

namespace SimpleCL.Ui
{
    partial class PasscodeEnter
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
            this.passcodeLabel = new System.Windows.Forms.Label();
            this.passcodeBox = new System.Windows.Forms.TextBox();
            this.submitPasscode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passcodeLabel
            // 
            this.passcodeLabel.Location = new System.Drawing.Point(12, 9);
            this.passcodeLabel.Name = "passcodeLabel";
            this.passcodeLabel.Size = new System.Drawing.Size(192, 17);
            this.passcodeLabel.TabIndex = 0;
            this.passcodeLabel.Text = "Secondary Passcode";
            // 
            // passcodeBox
            // 
            this.passcodeBox.Location = new System.Drawing.Point(12, 29);
            this.passcodeBox.Name = "passcodeBox";
            this.passcodeBox.Size = new System.Drawing.Size(182, 20);
            this.passcodeBox.TabIndex = 1;
            this.passcodeBox.UseSystemPasswordChar = true;
            // 
            // submitPasscode
            // 
            this.submitPasscode.Location = new System.Drawing.Point(200, 29);
            this.submitPasscode.Name = "submitPasscode";
            this.submitPasscode.Size = new System.Drawing.Size(71, 20);
            this.submitPasscode.TabIndex = 2;
            this.submitPasscode.Text = "Ok";
            this.submitPasscode.UseVisualStyleBackColor = true;
            // 
            // PasscodeEnter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(320, 60);
            this.Controls.Add(this.submitPasscode);
            this.Controls.Add(this.passcodeBox);
            this.Controls.Add(this.passcodeLabel);
            this.MaximizeBox = false;
            this.Name = "PasscodeEnter";
            this.Text = "Enter passcode";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button submitPasscode;

        private System.Windows.Forms.Label passcodeLabel;
        private System.Windows.Forms.TextBox passcodeBox;

        #endregion
    }
}