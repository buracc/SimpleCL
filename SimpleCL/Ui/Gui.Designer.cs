using System;
using System.ComponentModel;
using System.Windows.Forms;

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
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homeTab = new System.Windows.Forms.TabPage();
            this.statisticsBox = new System.Windows.Forms.GroupBox();
            this.coordsLabelValue = new System.Windows.Forms.Label();
            this.goldLabelValue = new System.Windows.Forms.Label();
            this.spLabelValue = new System.Windows.Forms.Label();
            this.expProgressBar = new System.Windows.Forms.ProgressBar();
            this.levelLabelValue = new System.Windows.Forms.Label();
            this.mpProgressBar = new System.Windows.Forms.ProgressBar();
            this.hpProgressBar = new System.Windows.Forms.ProgressBar();
            this.coordsLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.spLabel = new System.Windows.Forms.Label();
            this.expLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.mpLabel = new System.Windows.Forms.Label();
            this.hpLabel = new System.Windows.Forms.Label();
            this.chatTab = new System.Windows.Forms.TabPage();
            this.chatBox = new System.Windows.Forms.ListBox();
            this.loggerBox = new System.Windows.Forms.ListBox();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripProgressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            credentialsGroup = new System.Windows.Forms.GroupBox();
            credentialsGroup.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.homeTab.SuspendLayout();
            this.statisticsBox.SuspendLayout();
            this.chatTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // credentialsGroup
            // 
            credentialsGroup.Controls.Add(this.serverComboBox);
            credentialsGroup.Controls.Add(this.serverLabel);
            credentialsGroup.Controls.Add(this.loginButton);
            credentialsGroup.Controls.Add(this.usernameLabel);
            credentialsGroup.Controls.Add(this.passwordBox);
            credentialsGroup.Controls.Add(this.usernameBox);
            credentialsGroup.Controls.Add(this.passwordLabel);
            credentialsGroup.Location = new System.Drawing.Point(6, 6);
            credentialsGroup.Name = "credentialsGroup";
            credentialsGroup.Size = new System.Drawing.Size(262, 131);
            credentialsGroup.TabIndex = 6;
            credentialsGroup.TabStop = false;
            credentialsGroup.Text = "Credentials";
            // 
            // serverComboBox
            // 
            this.serverComboBox.Location = new System.Drawing.Point(69, 74);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(122, 20);
            this.serverComboBox.TabIndex = 9;
            // 
            // serverLabel
            // 
            this.serverLabel.Location = new System.Drawing.Point(6, 77);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(57, 17);
            this.serverLabel.TabIndex = 10;
            this.serverLabel.Text = "Server";
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
            // usernameLabel
            // 
            this.usernameLabel.Location = new System.Drawing.Point(6, 25);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(57, 17);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(69, 48);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(122, 20);
            this.passwordBox.TabIndex = 2;
            this.passwordBox.UseSystemPasswordChar = true;
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
            this.tabControl.Controls.Add(this.homeTab);
            this.tabControl.Controls.Add(this.chatTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(776, 298);
            this.tabControl.TabIndex = 4;
            // 
            // homeTab
            // 
            this.homeTab.Controls.Add(this.statisticsBox);
            this.homeTab.Controls.Add(credentialsGroup);
            this.homeTab.Location = new System.Drawing.Point(4, 22);
            this.homeTab.Name = "homeTab";
            this.homeTab.Padding = new System.Windows.Forms.Padding(3);
            this.homeTab.Size = new System.Drawing.Size(768, 272);
            this.homeTab.TabIndex = 0;
            this.homeTab.Text = "Home";
            this.homeTab.UseVisualStyleBackColor = true;
            // 
            // statisticsBox
            // 
            this.statisticsBox.Controls.Add(this.coordsLabelValue);
            this.statisticsBox.Controls.Add(this.goldLabelValue);
            this.statisticsBox.Controls.Add(this.spLabelValue);
            this.statisticsBox.Controls.Add(this.expProgressBar);
            this.statisticsBox.Controls.Add(this.levelLabelValue);
            this.statisticsBox.Controls.Add(this.mpProgressBar);
            this.statisticsBox.Controls.Add(this.hpProgressBar);
            this.statisticsBox.Controls.Add(this.coordsLabel);
            this.statisticsBox.Controls.Add(this.goldLabel);
            this.statisticsBox.Controls.Add(this.spLabel);
            this.statisticsBox.Controls.Add(this.expLabel);
            this.statisticsBox.Controls.Add(this.levelLabel);
            this.statisticsBox.Controls.Add(this.mpLabel);
            this.statisticsBox.Controls.Add(this.hpLabel);
            this.statisticsBox.Location = new System.Drawing.Point(274, 6);
            this.statisticsBox.Name = "statisticsBox";
            this.statisticsBox.Size = new System.Drawing.Size(488, 260);
            this.statisticsBox.TabIndex = 7;
            this.statisticsBox.TabStop = false;
            this.statisticsBox.Text = "Statistics";
            // 
            // coordsLabelValue
            // 
            this.coordsLabelValue.Location = new System.Drawing.Point(94, 155);
            this.coordsLabelValue.Name = "coordsLabelValue";
            this.coordsLabelValue.Size = new System.Drawing.Size(78, 22);
            this.coordsLabelValue.TabIndex = 17;
            this.coordsLabelValue.Text = "-1, -1";
            // 
            // goldLabelValue
            // 
            this.goldLabelValue.Location = new System.Drawing.Point(94, 133);
            this.goldLabelValue.Name = "goldLabelValue";
            this.goldLabelValue.Size = new System.Drawing.Size(78, 22);
            this.goldLabelValue.TabIndex = 16;
            this.goldLabelValue.Text = "-1";
            // 
            // spLabelValue
            // 
            this.spLabelValue.Location = new System.Drawing.Point(94, 111);
            this.spLabelValue.Name = "spLabelValue";
            this.spLabelValue.Size = new System.Drawing.Size(78, 22);
            this.spLabelValue.TabIndex = 13;
            this.spLabelValue.Text = "-1";
            // 
            // expProgressBar
            // 
            this.expProgressBar.Location = new System.Drawing.Point(94, 88);
            this.expProgressBar.Name = "expProgressBar";
            this.expProgressBar.Size = new System.Drawing.Size(371, 20);
            this.expProgressBar.Step = 1;
            this.expProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.expProgressBar.TabIndex = 12;
            // 
            // levelLabelValue
            // 
            this.levelLabelValue.Location = new System.Drawing.Point(94, 66);
            this.levelLabelValue.Name = "levelLabelValue";
            this.levelLabelValue.Size = new System.Drawing.Size(78, 22);
            this.levelLabelValue.TabIndex = 11;
            this.levelLabelValue.Text = "-1";
            // 
            // mpProgressBar
            // 
            this.mpProgressBar.Location = new System.Drawing.Point(94, 44);
            this.mpProgressBar.Name = "mpProgressBar";
            this.mpProgressBar.Size = new System.Drawing.Size(371, 20);
            this.mpProgressBar.Step = 1;
            this.mpProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.mpProgressBar.TabIndex = 10;
            // 
            // hpProgressBar
            // 
            this.hpProgressBar.Location = new System.Drawing.Point(94, 22);
            this.hpProgressBar.Name = "hpProgressBar";
            this.hpProgressBar.Size = new System.Drawing.Size(371, 20);
            this.hpProgressBar.Step = 1;
            this.hpProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.hpProgressBar.TabIndex = 9;
            // 
            // coordsLabel
            // 
            this.coordsLabel.Location = new System.Drawing.Point(10, 155);
            this.coordsLabel.Name = "coordsLabel";
            this.coordsLabel.Size = new System.Drawing.Size(78, 22);
            this.coordsLabel.TabIndex = 8;
            this.coordsLabel.Text = "Coordinates";
            // 
            // goldLabel
            // 
            this.goldLabel.Location = new System.Drawing.Point(10, 133);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(78, 22);
            this.goldLabel.TabIndex = 7;
            this.goldLabel.Text = "Gold";
            // 
            // spLabel
            // 
            this.spLabel.Location = new System.Drawing.Point(10, 111);
            this.spLabel.Name = "spLabel";
            this.spLabel.Size = new System.Drawing.Size(78, 22);
            this.spLabel.TabIndex = 4;
            this.spLabel.Text = "SP";
            // 
            // expLabel
            // 
            this.expLabel.Location = new System.Drawing.Point(10, 88);
            this.expLabel.Name = "expLabel";
            this.expLabel.Size = new System.Drawing.Size(78, 22);
            this.expLabel.TabIndex = 3;
            this.expLabel.Text = "EXP";
            // 
            // levelLabel
            // 
            this.levelLabel.Location = new System.Drawing.Point(10, 66);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(78, 22);
            this.levelLabel.TabIndex = 2;
            this.levelLabel.Text = "Level";
            // 
            // mpLabel
            // 
            this.mpLabel.Location = new System.Drawing.Point(10, 44);
            this.mpLabel.Name = "mpLabel";
            this.mpLabel.Size = new System.Drawing.Size(78, 22);
            this.mpLabel.TabIndex = 1;
            this.mpLabel.Text = "MP";
            // 
            // hpLabel
            // 
            this.hpLabel.Location = new System.Drawing.Point(10, 22);
            this.hpLabel.Name = "hpLabel";
            this.hpLabel.Size = new System.Drawing.Size(78, 22);
            this.hpLabel.TabIndex = 0;
            this.hpLabel.Text = "HP";
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
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 15);
            // 
            // toolStripProgressBar2
            // 
            this.toolStripProgressBar2.Name = "toolStripProgressBar2";
            this.toolStripProgressBar2.Size = new System.Drawing.Size(100, 15);
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loggerBox);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.Name = "Gui";
            this.Text = "SimpleCL";
            credentialsGroup.ResumeLayout(false);
            credentialsGroup.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.homeTab.ResumeLayout(false);
            this.statisticsBox.ResumeLayout(false);
            this.chatTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar2;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;

        private System.Windows.Forms.Label coordsLabel;
        private System.Windows.Forms.Label coordsLabelValue;
        private System.Windows.Forms.Label spLabelValue;

        private System.Windows.Forms.Label goldLabelValue;

        private System.Windows.Forms.ProgressBar hpProgressBar;

        private System.Windows.Forms.ProgressBar mpProgressBar;

        private System.Windows.Forms.ProgressBar expProgressBar;

        private System.Windows.Forms.Label levelLabelValue;

        private System.Windows.Forms.Label hpLabel;
        private System.Windows.Forms.Label mpLabel;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label expLabel;
        private System.Windows.Forms.Label spLabel;
        private System.Windows.Forms.Label goldLabel;


        private System.Windows.Forms.TabPage homeTab;
        private System.Windows.Forms.GroupBox statisticsBox;

        private System.Windows.Forms.TabPage chatTab;
        private System.Windows.Forms.ListBox chatBox;

        private System.Windows.Forms.ListBox loggerBox;

        private System.Windows.Forms.Button loginButton;

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Label passwordLabel;

        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label usernameLabel;

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.ComboBox serverComboBox;

        #endregion
    }
}