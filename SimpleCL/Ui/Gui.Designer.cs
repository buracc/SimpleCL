using System;
using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Network.Enums;

namespace SimpleCL.Ui
{
    partial class Gui
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
            System.Windows.Forms.GroupBox credentialsGroup;
            this.loginButton = new System.Windows.Forms.Button();
            this.localeComboBox = new System.Windows.Forms.ComboBox();
            this.localeLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.gatewayComboBox = new System.Windows.Forms.ComboBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.gatewayLabel = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.loginTab = new System.Windows.Forms.TabPage();
            this.chatTab = new System.Windows.Forms.TabPage();
            this.chatBox = new System.Windows.Forms.ListBox();
            this.loggerBox = new System.Windows.Forms.ListBox();
            credentialsGroup = new System.Windows.Forms.GroupBox();
            credentialsGroup.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.loginTab.SuspendLayout();
            this.chatTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // credentialsGroup
            // 
            credentialsGroup.Controls.Add(this.loginButton);
            credentialsGroup.Controls.Add(this.localeComboBox);
            credentialsGroup.Controls.Add(this.localeLabel);
            credentialsGroup.Controls.Add(this.usernameLabel);
            credentialsGroup.Controls.Add(this.gatewayComboBox);
            credentialsGroup.Controls.Add(this.passwordBox);
            credentialsGroup.Controls.Add(this.gatewayLabel);
            credentialsGroup.Controls.Add(this.usernameBox);
            credentialsGroup.Controls.Add(this.passwordLabel);
            credentialsGroup.Location = new System.Drawing.Point(6, 6);
            credentialsGroup.Name = "credentialsGroup";
            credentialsGroup.Size = new System.Drawing.Size(262, 131);
            credentialsGroup.TabIndex = 6;
            credentialsGroup.TabStop = false;
            credentialsGroup.Text = "Credentials";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(197, 22);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(57, 73);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginClicked);
            // 
            // localeComboBox
            // 
            this.localeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localeComboBox.FormattingEnabled = true;
            this.localeComboBox.Location = new System.Drawing.Point(69, 101);
            this.localeComboBox.Name = "localeComboBox";
            this.localeComboBox.Size = new System.Drawing.Size(185, 21);
            this.localeComboBox.TabIndex = 7;
            // 
            // localeLabel
            // 
            this.localeLabel.Location = new System.Drawing.Point(6, 104);
            this.localeLabel.Name = "localeLabel";
            this.localeLabel.Size = new System.Drawing.Size(57, 17);
            this.localeLabel.TabIndex = 6;
            this.localeLabel.Text = "Locale";
            // 
            // usernameLabel
            // 
            this.usernameLabel.Location = new System.Drawing.Point(6, 25);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(57, 17);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username";
            // 
            // gatewayComboBox
            // 
            this.gatewayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gatewayComboBox.FormattingEnabled = true;
            this.gatewayComboBox.Location = new System.Drawing.Point(69, 74);
            this.gatewayComboBox.Name = "gatewayComboBox";
            this.gatewayComboBox.Size = new System.Drawing.Size(122, 21);
            this.gatewayComboBox.TabIndex = 5;
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(69, 48);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(122, 20);
            this.passwordBox.TabIndex = 2;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // gatewayLabel
            // 
            this.gatewayLabel.Location = new System.Drawing.Point(6, 78);
            this.gatewayLabel.Name = "gatewayLabel";
            this.gatewayLabel.Size = new System.Drawing.Size(57, 17);
            this.gatewayLabel.TabIndex = 4;
            this.gatewayLabel.Text = "Gateway";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(69, 22);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(122, 20);
            this.usernameBox.TabIndex = 0;
            // 
            // passwordLabel
            // 
            this.passwordLabel.Location = new System.Drawing.Point(6, 51);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 17);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.loginTab);
            this.tabControl.Controls.Add(this.chatTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(776, 298);
            this.tabControl.TabIndex = 4;
            // 
            // loginTab
            // 
            this.loginTab.Controls.Add(credentialsGroup);
            this.loginTab.Location = new System.Drawing.Point(4, 22);
            this.loginTab.Name = "loginTab";
            this.loginTab.Padding = new System.Windows.Forms.Padding(3);
            this.loginTab.Size = new System.Drawing.Size(768, 272);
            this.loginTab.TabIndex = 0;
            this.loginTab.Text = "Login";
            this.loginTab.UseVisualStyleBackColor = true;
            // 
            // chatTab
            // 
            this.chatTab.Controls.Add(this.chatBox);
            this.chatTab.Location = new System.Drawing.Point(4, 22);
            this.chatTab.Name = "chatTab";
            this.chatTab.Padding = new System.Windows.Forms.Padding(3);
            this.chatTab.Size = new System.Drawing.Size(768, 272);
            this.chatTab.TabIndex = 1;
            this.chatTab.Text = "Chat";
            this.chatTab.UseVisualStyleBackColor = true;
            // 
            // chatBox
            // 
            this.chatBox.FormattingEnabled = true;
            this.chatBox.Location = new System.Drawing.Point(0, 1);
            this.chatBox.Name = "chatBox";
            this.chatBox.ScrollAlwaysVisible = true;
            this.chatBox.Size = new System.Drawing.Size(765, 264);
            this.chatBox.TabIndex = 0;
            // 
            // loggerBox
            // 
            this.loggerBox.FormattingEnabled = true;
            this.loggerBox.Location = new System.Drawing.Point(12, 313);
            this.loggerBox.Name = "loggerBox";
            this.loggerBox.ScrollAlwaysVisible = true;
            this.loggerBox.Size = new System.Drawing.Size(776, 121);
            this.loggerBox.TabIndex = 5;
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loggerBox);
            this.Controls.Add(this.tabControl);
            this.Name = "Gui";
            this.Text = "SimpleCL";
            credentialsGroup.ResumeLayout(false);
            credentialsGroup.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.loginTab.ResumeLayout(false);
            this.chatTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabPage chatTab;
        private System.Windows.Forms.ListBox chatBox;

        private System.Windows.Forms.ListBox loggerBox;

        private System.Windows.Forms.ComboBox gatewayComboBox;
        private System.Windows.Forms.ComboBox localeComboBox;
        private System.Windows.Forms.Label localeLabel;
        private System.Windows.Forms.Button loginButton;

        private System.Windows.Forms.Label gatewayLabel;

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage loginTab;
        private System.Windows.Forms.TabPage logTab;
        private System.Windows.Forms.Label passwordLabel;

        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label usernameLabel;

        #endregion
    }
}