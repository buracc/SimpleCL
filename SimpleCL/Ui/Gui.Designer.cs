using System;
using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Ui.Components;

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
            System.Windows.Forms.Label serverLabel;
            System.Windows.Forms.Label usernameLabel;
            System.Windows.Forms.Label passwordLabel;
            System.Windows.Forms.GroupBox buffsGroupBox;
            System.Windows.Forms.GroupBox statisticsBox;
            System.Windows.Forms.Label worldCoordsLabel;
            System.Windows.Forms.Label jobExpLabel;
            System.Windows.Forms.Label jobLevelLabel;
            System.Windows.Forms.Label jobNameLabel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label localCoordsLabel;
            System.Windows.Forms.Label goldLabel;
            System.Windows.Forms.Label spLabel;
            System.Windows.Forms.Label expLabel;
            System.Windows.Forms.Label levelLabel;
            System.Windows.Forms.Label mpLabel;
            System.Windows.Forms.Label hpLabel;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label currWorldLabel;
            System.Windows.Forms.Label currLocalLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.buffsDataGridView = new System.Windows.Forms.DataGridView();
            this.worldCoordsLabelValue = new System.Windows.Forms.Label();
            this.jobExpProgressBar = new SimpleCL.Ui.Components.TextProgressBar();
            this.hpProgressBar = new SimpleCL.Ui.Components.TextProgressBar();
            this.mpProgressBar = new SimpleCL.Ui.Components.TextProgressBar();
            this.expProgressBar = new SimpleCL.Ui.Components.TextProgressBar();
            this.jobLevelLabelValue = new System.Windows.Forms.Label();
            this.jobNameLabelValue = new System.Windows.Forms.Label();
            this.nameLabelValue = new System.Windows.Forms.Label();
            this.localCoordsLabelValue = new System.Windows.Forms.Label();
            this.goldLabelValue = new System.Windows.Forms.Label();
            this.spLabelValue = new System.Windows.Forms.Label();
            this.levelLabelValue = new System.Windows.Forms.Label();
            this.credentialsGroup = new System.Windows.Forms.GroupBox();
            this.proxyPortTextBox = new System.Windows.Forms.TextBox();
            this.proxyIpTextBox = new System.Windows.Forms.TextBox();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homeTab = new System.Windows.Forms.TabPage();
            this.inventoryPage = new System.Windows.Forms.TabPage();
            this.goldAmountLabel = new System.Windows.Forms.Label();
            this.refreshInventoriesButton = new System.Windows.Forms.Button();
            this.inventoryTabControl = new System.Windows.Forms.TabControl();
            this.inventoryInvTab = new System.Windows.Forms.TabPage();
            this.inventoryDataGridView = new System.Windows.Forms.DataGridView();
            this.equipmentInvTab = new System.Windows.Forms.TabPage();
            this.equipmentDataGridView = new System.Windows.Forms.DataGridView();
            this.avatarInvTab = new System.Windows.Forms.TabPage();
            this.avatarDataGridView = new System.Windows.Forms.DataGridView();
            this.jobEquipmentInvTab = new System.Windows.Forms.TabPage();
            this.jobEquipmentDataGridView = new System.Windows.Forms.DataGridView();
            this.chatTab = new System.Windows.Forms.TabPage();
            this.chatBox = new System.Windows.Forms.ListBox();
            this.attackTab = new System.Windows.Forms.TabPage();
            this.attackButton = new System.Windows.Forms.Button();
            this.removeSkillListBox = new System.Windows.Forms.Button();
            this.addSkillListBox = new System.Windows.Forms.Button();
            this.removeEntityButton = new System.Windows.Forms.Button();
            this.addEntityButton = new System.Windows.Forms.Button();
            this.nearEntitiesListBox = new System.Windows.Forms.ListBox();
            this.availSkillsListBox = new System.Windows.Forms.ListBox();
            this.attackEntitiesListBox = new System.Windows.Forms.ListBox();
            this.attackSkillsListBox = new System.Windows.Forms.ListBox();
            this.movementTab = new System.Windows.Forms.TabPage();
            this.mapVisibilityCheckbox = new System.Windows.Forms.CheckBox();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.currWorldLabelValue = new System.Windows.Forms.Label();
            this.currLocalLabelValue = new System.Windows.Forms.Label();
            this.devTab = new System.Windows.Forms.TabPage();
            this.packetLoggerGroupBox = new System.Windows.Forms.GroupBox();
            this.packetlogRtb = new System.Windows.Forms.RichTextBox();
            this.filterPacketTextBox = new System.Windows.Forms.TextBox();
            this.filteredPacketsListBox = new System.Windows.Forms.ListBox();
            this.debugAgCheckbox = new System.Windows.Forms.CheckBox();
            this.debugGwCheckbox = new System.Windows.Forms.CheckBox();
            this.loggerBox = new System.Windows.Forms.ListBox();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripProgressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            this.proxyPasswordBox = new System.Windows.Forms.TextBox();
            this.proxyUsernameBox = new System.Windows.Forms.TextBox();
            serverLabel = new System.Windows.Forms.Label();
            usernameLabel = new System.Windows.Forms.Label();
            passwordLabel = new System.Windows.Forms.Label();
            buffsGroupBox = new System.Windows.Forms.GroupBox();
            statisticsBox = new System.Windows.Forms.GroupBox();
            worldCoordsLabel = new System.Windows.Forms.Label();
            jobExpLabel = new System.Windows.Forms.Label();
            jobLevelLabel = new System.Windows.Forms.Label();
            jobNameLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            localCoordsLabel = new System.Windows.Forms.Label();
            goldLabel = new System.Windows.Forms.Label();
            spLabel = new System.Windows.Forms.Label();
            expLabel = new System.Windows.Forms.Label();
            levelLabel = new System.Windows.Forms.Label();
            mpLabel = new System.Windows.Forms.Label();
            hpLabel = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            currWorldLabel = new System.Windows.Forms.Label();
            currLocalLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            buffsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.buffsDataGridView)).BeginInit();
            statisticsBox.SuspendLayout();
            this.credentialsGroup.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.homeTab.SuspendLayout();
            this.inventoryPage.SuspendLayout();
            this.inventoryTabControl.SuspendLayout();
            this.inventoryInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.inventoryDataGridView)).BeginInit();
            this.equipmentInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.equipmentDataGridView)).BeginInit();
            this.avatarInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.avatarDataGridView)).BeginInit();
            this.jobEquipmentInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.jobEquipmentDataGridView)).BeginInit();
            this.chatTab.SuspendLayout();
            this.attackTab.SuspendLayout();
            this.movementTab.SuspendLayout();
            this.devTab.SuspendLayout();
            this.packetLoggerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            serverLabel.Location = new System.Drawing.Point(6, 77);
            serverLabel.Name = "serverLabel";
            serverLabel.Size = new System.Drawing.Size(57, 17);
            serverLabel.TabIndex = 10;
            serverLabel.Text = "Server";
            // 
            // usernameLabel
            // 
            usernameLabel.Location = new System.Drawing.Point(6, 25);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(57, 17);
            usernameLabel.TabIndex = 1;
            usernameLabel.Text = "Username";
            // 
            // passwordLabel
            // 
            passwordLabel.Location = new System.Drawing.Point(6, 51);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(57, 17);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Password";
            // 
            // buffsGroupBox
            // 
            buffsGroupBox.Controls.Add(this.buffsDataGridView);
            buffsGroupBox.Location = new System.Drawing.Point(6, 164);
            buffsGroupBox.Name = "buffsGroupBox";
            buffsGroupBox.Size = new System.Drawing.Size(262, 211);
            buffsGroupBox.TabIndex = 8;
            buffsGroupBox.TabStop = false;
            buffsGroupBox.Text = "Buffs";
            // 
            // buffsDataGridView
            // 
            this.buffsDataGridView.AllowUserToAddRows = false;
            this.buffsDataGridView.AllowUserToDeleteRows = false;
            this.buffsDataGridView.AllowUserToResizeRows = false;
            this.buffsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.buffsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.buffsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.buffsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.buffsDataGridView.Name = "buffsDataGridView";
            this.buffsDataGridView.ReadOnly = true;
            this.buffsDataGridView.RowHeadersVisible = false;
            this.buffsDataGridView.Size = new System.Drawing.Size(248, 183);
            this.buffsDataGridView.TabIndex = 0;
            // 
            // statisticsBox
            // 
            statisticsBox.Controls.Add(this.worldCoordsLabelValue);
            statisticsBox.Controls.Add(worldCoordsLabel);
            statisticsBox.Controls.Add(this.jobExpProgressBar);
            statisticsBox.Controls.Add(this.hpProgressBar);
            statisticsBox.Controls.Add(this.mpProgressBar);
            statisticsBox.Controls.Add(this.expProgressBar);
            statisticsBox.Controls.Add(jobExpLabel);
            statisticsBox.Controls.Add(this.jobLevelLabelValue);
            statisticsBox.Controls.Add(jobLevelLabel);
            statisticsBox.Controls.Add(this.jobNameLabelValue);
            statisticsBox.Controls.Add(jobNameLabel);
            statisticsBox.Controls.Add(this.nameLabelValue);
            statisticsBox.Controls.Add(nameLabel);
            statisticsBox.Controls.Add(this.localCoordsLabelValue);
            statisticsBox.Controls.Add(this.goldLabelValue);
            statisticsBox.Controls.Add(this.spLabelValue);
            statisticsBox.Controls.Add(this.levelLabelValue);
            statisticsBox.Controls.Add(localCoordsLabel);
            statisticsBox.Controls.Add(goldLabel);
            statisticsBox.Controls.Add(spLabel);
            statisticsBox.Controls.Add(expLabel);
            statisticsBox.Controls.Add(levelLabel);
            statisticsBox.Controls.Add(mpLabel);
            statisticsBox.Controls.Add(hpLabel);
            statisticsBox.Location = new System.Drawing.Point(274, 6);
            statisticsBox.Name = "statisticsBox";
            statisticsBox.Size = new System.Drawing.Size(488, 254);
            statisticsBox.TabIndex = 7;
            statisticsBox.TabStop = false;
            statisticsBox.Text = "Statistics";
            // 
            // worldCoordsLabelValue
            // 
            this.worldCoordsLabelValue.Location = new System.Drawing.Point(90, 224);
            this.worldCoordsLabelValue.Name = "worldCoordsLabelValue";
            this.worldCoordsLabelValue.Size = new System.Drawing.Size(371, 22);
            this.worldCoordsLabelValue.TabIndex = 31;
            this.worldCoordsLabelValue.Text = "-1, -1";
            // 
            // worldCoordsLabel
            // 
            worldCoordsLabel.Location = new System.Drawing.Point(6, 224);
            worldCoordsLabel.Name = "worldCoordsLabel";
            worldCoordsLabel.Size = new System.Drawing.Size(78, 22);
            worldCoordsLabel.TabIndex = 30;
            worldCoordsLabel.Text = "World";
            // 
            // jobExpProgressBar
            // 
            this.jobExpProgressBar.CustomText = "";
            this.jobExpProgressBar.Location = new System.Drawing.Point(90, 134);
            this.jobExpProgressBar.Name = "jobExpProgressBar";
            this.jobExpProgressBar.ProgressColor = System.Drawing.Color.LightGreen;
            this.jobExpProgressBar.Size = new System.Drawing.Size(371, 20);
            this.jobExpProgressBar.TabIndex = 29;
            this.jobExpProgressBar.TextColor = System.Drawing.Color.Black;
            this.jobExpProgressBar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.jobExpProgressBar.VisualMode = SimpleCL.Ui.Components.ProgressBarDisplayMode.CustomText;
            // 
            // hpProgressBar
            // 
            this.hpProgressBar.CustomText = "";
            this.hpProgressBar.Location = new System.Drawing.Point(90, 45);
            this.hpProgressBar.Name = "hpProgressBar";
            this.hpProgressBar.ProgressColor = System.Drawing.Color.LightGreen;
            this.hpProgressBar.Size = new System.Drawing.Size(371, 20);
            this.hpProgressBar.TabIndex = 28;
            this.hpProgressBar.TextColor = System.Drawing.Color.Black;
            this.hpProgressBar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.hpProgressBar.VisualMode = SimpleCL.Ui.Components.ProgressBarDisplayMode.CurrProgress;
            // 
            // mpProgressBar
            // 
            this.mpProgressBar.CustomText = "";
            this.mpProgressBar.Location = new System.Drawing.Point(90, 68);
            this.mpProgressBar.Name = "mpProgressBar";
            this.mpProgressBar.ProgressColor = System.Drawing.Color.LightGreen;
            this.mpProgressBar.Size = new System.Drawing.Size(371, 20);
            this.mpProgressBar.TabIndex = 27;
            this.mpProgressBar.TextColor = System.Drawing.Color.Black;
            this.mpProgressBar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.mpProgressBar.VisualMode = SimpleCL.Ui.Components.ProgressBarDisplayMode.CurrProgress;
            // 
            // expProgressBar
            // 
            this.expProgressBar.CustomText = "";
            this.expProgressBar.Location = new System.Drawing.Point(90, 111);
            this.expProgressBar.Name = "expProgressBar";
            this.expProgressBar.ProgressColor = System.Drawing.Color.LightGreen;
            this.expProgressBar.Size = new System.Drawing.Size(371, 20);
            this.expProgressBar.TabIndex = 26;
            this.expProgressBar.TextColor = System.Drawing.Color.Black;
            this.expProgressBar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.expProgressBar.VisualMode = SimpleCL.Ui.Components.ProgressBarDisplayMode.CustomText;
            // 
            // jobExpLabel
            // 
            jobExpLabel.Location = new System.Drawing.Point(6, 135);
            jobExpLabel.Name = "jobExpLabel";
            jobExpLabel.Size = new System.Drawing.Size(78, 22);
            jobExpLabel.TabIndex = 24;
            jobExpLabel.Text = "Job EXP";
            // 
            // jobLevelLabelValue
            // 
            this.jobLevelLabelValue.Location = new System.Drawing.Point(258, 91);
            this.jobLevelLabelValue.Name = "jobLevelLabelValue";
            this.jobLevelLabelValue.Size = new System.Drawing.Size(78, 22);
            this.jobLevelLabelValue.TabIndex = 23;
            this.jobLevelLabelValue.Text = "-1";
            // 
            // jobLevelLabel
            // 
            jobLevelLabel.Location = new System.Drawing.Point(174, 91);
            jobLevelLabel.Name = "jobLevelLabel";
            jobLevelLabel.Size = new System.Drawing.Size(78, 22);
            jobLevelLabel.TabIndex = 22;
            jobLevelLabel.Text = "Job level";
            // 
            // jobNameLabelValue
            // 
            this.jobNameLabelValue.Location = new System.Drawing.Point(258, 22);
            this.jobNameLabelValue.Name = "jobNameLabelValue";
            this.jobNameLabelValue.Size = new System.Drawing.Size(78, 22);
            this.jobNameLabelValue.TabIndex = 21;
            this.jobNameLabelValue.Text = "-";
            // 
            // jobNameLabel
            // 
            jobNameLabel.Location = new System.Drawing.Point(174, 22);
            jobNameLabel.Name = "jobNameLabel";
            jobNameLabel.Size = new System.Drawing.Size(78, 22);
            jobNameLabel.TabIndex = 20;
            jobNameLabel.Text = "Job alias";
            // 
            // nameLabelValue
            // 
            this.nameLabelValue.Location = new System.Drawing.Point(90, 22);
            this.nameLabelValue.Name = "nameLabelValue";
            this.nameLabelValue.Size = new System.Drawing.Size(78, 22);
            this.nameLabelValue.TabIndex = 19;
            this.nameLabelValue.Text = "-";
            // 
            // nameLabel
            // 
            nameLabel.Location = new System.Drawing.Point(6, 22);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(78, 22);
            nameLabel.TabIndex = 18;
            nameLabel.Text = "Name";
            // 
            // localCoordsLabelValue
            // 
            this.localCoordsLabelValue.Location = new System.Drawing.Point(90, 202);
            this.localCoordsLabelValue.Name = "localCoordsLabelValue";
            this.localCoordsLabelValue.Size = new System.Drawing.Size(371, 22);
            this.localCoordsLabelValue.TabIndex = 17;
            this.localCoordsLabelValue.Text = "-1, -1";
            // 
            // goldLabelValue
            // 
            this.goldLabelValue.Location = new System.Drawing.Point(90, 180);
            this.goldLabelValue.Name = "goldLabelValue";
            this.goldLabelValue.Size = new System.Drawing.Size(78, 22);
            this.goldLabelValue.TabIndex = 16;
            this.goldLabelValue.Text = "-1";
            // 
            // spLabelValue
            // 
            this.spLabelValue.Location = new System.Drawing.Point(90, 158);
            this.spLabelValue.Name = "spLabelValue";
            this.spLabelValue.Size = new System.Drawing.Size(78, 22);
            this.spLabelValue.TabIndex = 13;
            this.spLabelValue.Text = "-1";
            // 
            // levelLabelValue
            // 
            this.levelLabelValue.Location = new System.Drawing.Point(90, 91);
            this.levelLabelValue.Name = "levelLabelValue";
            this.levelLabelValue.Size = new System.Drawing.Size(78, 22);
            this.levelLabelValue.TabIndex = 11;
            this.levelLabelValue.Text = "-1";
            // 
            // localCoordsLabel
            // 
            localCoordsLabel.Location = new System.Drawing.Point(6, 202);
            localCoordsLabel.Name = "localCoordsLabel";
            localCoordsLabel.Size = new System.Drawing.Size(78, 22);
            localCoordsLabel.TabIndex = 8;
            localCoordsLabel.Text = "Local";
            // 
            // goldLabel
            // 
            goldLabel.Location = new System.Drawing.Point(6, 180);
            goldLabel.Name = "goldLabel";
            goldLabel.Size = new System.Drawing.Size(78, 22);
            goldLabel.TabIndex = 7;
            goldLabel.Text = "Gold";
            // 
            // spLabel
            // 
            spLabel.Location = new System.Drawing.Point(6, 158);
            spLabel.Name = "spLabel";
            spLabel.Size = new System.Drawing.Size(78, 22);
            spLabel.TabIndex = 4;
            spLabel.Text = "SP";
            // 
            // expLabel
            // 
            expLabel.Location = new System.Drawing.Point(6, 113);
            expLabel.Name = "expLabel";
            expLabel.Size = new System.Drawing.Size(78, 22);
            expLabel.TabIndex = 3;
            expLabel.Text = "EXP";
            // 
            // levelLabel
            // 
            levelLabel.Location = new System.Drawing.Point(6, 91);
            levelLabel.Name = "levelLabel";
            levelLabel.Size = new System.Drawing.Size(78, 22);
            levelLabel.TabIndex = 2;
            levelLabel.Text = "Level";
            // 
            // mpLabel
            // 
            mpLabel.Location = new System.Drawing.Point(6, 69);
            mpLabel.Name = "mpLabel";
            mpLabel.Size = new System.Drawing.Size(78, 22);
            mpLabel.TabIndex = 1;
            mpLabel.Text = "MP";
            // 
            // hpLabel
            // 
            hpLabel.Location = new System.Drawing.Point(6, 47);
            hpLabel.Name = "hpLabel";
            hpLabel.Size = new System.Drawing.Size(78, 22);
            hpLabel.TabIndex = 0;
            hpLabel.Text = "HP";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(261, 352);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(53, 23);
            label3.TabIndex = 2;
            label3.Text = "Gold:";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(421, 259);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(75, 19);
            label2.TabIndex = 3;
            label2.Text = "Entities";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(201, 259);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 19);
            label1.TabIndex = 2;
            label1.Text = "Skills";
            // 
            // currWorldLabel
            // 
            currWorldLabel.Location = new System.Drawing.Point(291, 358);
            currWorldLabel.Name = "currWorldLabel";
            currWorldLabel.Size = new System.Drawing.Size(39, 23);
            currWorldLabel.TabIndex = 9;
            currWorldLabel.Text = "World";
            // 
            // currLocalLabel
            // 
            currLocalLabel.Location = new System.Drawing.Point(0, 358);
            currLocalLabel.Name = "currLocalLabel";
            currLocalLabel.Size = new System.Drawing.Size(46, 23);
            currLocalLabel.TabIndex = 7;
            currLocalLabel.Text = "Local";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(14, 154);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(142, 15);
            label4.TabIndex = 5;
            label4.Text = "Logged opcodes";
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(6, 104);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(57, 17);
            label5.TabIndex = 12;
            label5.Text = "Proxy";
            // 
            // credentialsGroup
            // 
            this.credentialsGroup.Controls.Add(this.proxyPasswordBox);
            this.credentialsGroup.Controls.Add(label6);
            this.credentialsGroup.Controls.Add(this.proxyUsernameBox);
            this.credentialsGroup.Controls.Add(this.proxyPortTextBox);
            this.credentialsGroup.Controls.Add(label5);
            this.credentialsGroup.Controls.Add(this.proxyIpTextBox);
            this.credentialsGroup.Controls.Add(this.serverComboBox);
            this.credentialsGroup.Controls.Add(serverLabel);
            this.credentialsGroup.Controls.Add(this.loginButton);
            this.credentialsGroup.Controls.Add(usernameLabel);
            this.credentialsGroup.Controls.Add(this.passwordBox);
            this.credentialsGroup.Controls.Add(this.usernameBox);
            this.credentialsGroup.Controls.Add(passwordLabel);
            this.credentialsGroup.Location = new System.Drawing.Point(6, 6);
            this.credentialsGroup.Name = "credentialsGroup";
            this.credentialsGroup.Size = new System.Drawing.Size(262, 157);
            this.credentialsGroup.TabIndex = 6;
            this.credentialsGroup.TabStop = false;
            this.credentialsGroup.Text = "Credentials";
            // 
            // proxyPortTextBox
            // 
            this.proxyPortTextBox.Location = new System.Drawing.Point(197, 101);
            this.proxyPortTextBox.Name = "proxyPortTextBox";
            this.proxyPortTextBox.Size = new System.Drawing.Size(57, 20);
            this.proxyPortTextBox.TabIndex = 13;
            // 
            // proxyIpTextBox
            // 
            this.proxyIpTextBox.Location = new System.Drawing.Point(69, 101);
            this.proxyIpTextBox.Name = "proxyIpTextBox";
            this.proxyIpTextBox.Size = new System.Drawing.Size(122, 20);
            this.proxyIpTextBox.TabIndex = 11;
            // 
            // serverComboBox
            // 
            this.serverComboBox.Location = new System.Drawing.Point(69, 74);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(122, 21);
            this.serverComboBox.TabIndex = 9;
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.homeTab);
            this.tabControl.Controls.Add(this.inventoryPage);
            this.tabControl.Controls.Add(this.chatTab);
            this.tabControl.Controls.Add(this.attackTab);
            this.tabControl.Controls.Add(this.movementTab);
            this.tabControl.Controls.Add(this.devTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(776, 407);
            this.tabControl.TabIndex = 4;
            // 
            // homeTab
            // 
            this.homeTab.Controls.Add(buffsGroupBox);
            this.homeTab.Controls.Add(statisticsBox);
            this.homeTab.Controls.Add(this.credentialsGroup);
            this.homeTab.Location = new System.Drawing.Point(4, 22);
            this.homeTab.Name = "homeTab";
            this.homeTab.Padding = new System.Windows.Forms.Padding(3);
            this.homeTab.Size = new System.Drawing.Size(768, 381);
            this.homeTab.TabIndex = 0;
            this.homeTab.Text = "Home";
            this.homeTab.UseVisualStyleBackColor = true;
            // 
            // inventoryPage
            // 
            this.inventoryPage.Controls.Add(this.goldAmountLabel);
            this.inventoryPage.Controls.Add(label3);
            this.inventoryPage.Controls.Add(this.refreshInventoriesButton);
            this.inventoryPage.Controls.Add(this.inventoryTabControl);
            this.inventoryPage.Location = new System.Drawing.Point(4, 22);
            this.inventoryPage.Name = "inventoryPage";
            this.inventoryPage.Size = new System.Drawing.Size(768, 381);
            this.inventoryPage.TabIndex = 2;
            this.inventoryPage.Text = "Inventory";
            this.inventoryPage.UseVisualStyleBackColor = true;
            // 
            // goldAmountLabel
            // 
            this.goldAmountLabel.Location = new System.Drawing.Point(320, 352);
            this.goldAmountLabel.Name = "goldAmountLabel";
            this.goldAmountLabel.Size = new System.Drawing.Size(160, 23);
            this.goldAmountLabel.TabIndex = 3;
            this.goldAmountLabel.Text = "0";
            // 
            // refreshInventoriesButton
            // 
            this.refreshInventoriesButton.Location = new System.Drawing.Point(3, 352);
            this.refreshInventoriesButton.Name = "refreshInventoriesButton";
            this.refreshInventoriesButton.Size = new System.Drawing.Size(78, 23);
            this.refreshInventoriesButton.TabIndex = 1;
            this.refreshInventoriesButton.Text = "Refresh";
            this.refreshInventoriesButton.UseVisualStyleBackColor = true;
            // 
            // inventoryTabControl
            // 
            this.inventoryTabControl.Controls.Add(this.inventoryInvTab);
            this.inventoryTabControl.Controls.Add(this.equipmentInvTab);
            this.inventoryTabControl.Controls.Add(this.avatarInvTab);
            this.inventoryTabControl.Controls.Add(this.jobEquipmentInvTab);
            this.inventoryTabControl.Location = new System.Drawing.Point(3, 3);
            this.inventoryTabControl.Name = "inventoryTabControl";
            this.inventoryTabControl.SelectedIndex = 0;
            this.inventoryTabControl.Size = new System.Drawing.Size(762, 347);
            this.inventoryTabControl.TabIndex = 0;
            // 
            // inventoryInvTab
            // 
            this.inventoryInvTab.Controls.Add(this.inventoryDataGridView);
            this.inventoryInvTab.Location = new System.Drawing.Point(4, 22);
            this.inventoryInvTab.Name = "inventoryInvTab";
            this.inventoryInvTab.Padding = new System.Windows.Forms.Padding(3);
            this.inventoryInvTab.Size = new System.Drawing.Size(754, 321);
            this.inventoryInvTab.TabIndex = 0;
            this.inventoryInvTab.Text = "Inventory";
            this.inventoryInvTab.UseVisualStyleBackColor = true;
            // 
            // inventoryDataGridView
            // 
            this.inventoryDataGridView.AllowUserToAddRows = false;
            this.inventoryDataGridView.AllowUserToDeleteRows = false;
            this.inventoryDataGridView.AllowUserToResizeColumns = false;
            this.inventoryDataGridView.AllowUserToResizeRows = false;
            this.inventoryDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.inventoryDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.inventoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inventoryDataGridView.Location = new System.Drawing.Point(6, 6);
            this.inventoryDataGridView.MultiSelect = false;
            this.inventoryDataGridView.Name = "inventoryDataGridView";
            this.inventoryDataGridView.ReadOnly = true;
            this.inventoryDataGridView.RowHeadersVisible = false;
            this.inventoryDataGridView.Size = new System.Drawing.Size(742, 308);
            this.inventoryDataGridView.TabIndex = 0;
            // 
            // equipmentInvTab
            // 
            this.equipmentInvTab.Controls.Add(this.equipmentDataGridView);
            this.equipmentInvTab.Location = new System.Drawing.Point(4, 22);
            this.equipmentInvTab.Name = "equipmentInvTab";
            this.equipmentInvTab.Padding = new System.Windows.Forms.Padding(3);
            this.equipmentInvTab.Size = new System.Drawing.Size(754, 321);
            this.equipmentInvTab.TabIndex = 1;
            this.equipmentInvTab.Text = "Equipment";
            this.equipmentInvTab.UseVisualStyleBackColor = true;
            // 
            // equipmentDataGridView
            // 
            this.equipmentDataGridView.AllowUserToAddRows = false;
            this.equipmentDataGridView.AllowUserToDeleteRows = false;
            this.equipmentDataGridView.AllowUserToResizeColumns = false;
            this.equipmentDataGridView.AllowUserToResizeRows = false;
            this.equipmentDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.equipmentDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.equipmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.equipmentDataGridView.Location = new System.Drawing.Point(6, 6);
            this.equipmentDataGridView.MultiSelect = false;
            this.equipmentDataGridView.Name = "equipmentDataGridView";
            this.equipmentDataGridView.ReadOnly = true;
            this.equipmentDataGridView.RowHeadersVisible = false;
            this.equipmentDataGridView.Size = new System.Drawing.Size(742, 308);
            this.equipmentDataGridView.TabIndex = 1;
            // 
            // avatarInvTab
            // 
            this.avatarInvTab.Controls.Add(this.avatarDataGridView);
            this.avatarInvTab.Location = new System.Drawing.Point(4, 22);
            this.avatarInvTab.Name = "avatarInvTab";
            this.avatarInvTab.Size = new System.Drawing.Size(754, 321);
            this.avatarInvTab.TabIndex = 2;
            this.avatarInvTab.Text = "Avatar";
            this.avatarInvTab.UseVisualStyleBackColor = true;
            // 
            // avatarDataGridView
            // 
            this.avatarDataGridView.AllowUserToAddRows = false;
            this.avatarDataGridView.AllowUserToDeleteRows = false;
            this.avatarDataGridView.AllowUserToOrderColumns = true;
            this.avatarDataGridView.AllowUserToResizeColumns = false;
            this.avatarDataGridView.AllowUserToResizeRows = false;
            this.avatarDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.avatarDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.avatarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.avatarDataGridView.Location = new System.Drawing.Point(6, 6);
            this.avatarDataGridView.MultiSelect = false;
            this.avatarDataGridView.Name = "avatarDataGridView";
            this.avatarDataGridView.ReadOnly = true;
            this.avatarDataGridView.RowHeadersVisible = false;
            this.avatarDataGridView.Size = new System.Drawing.Size(742, 308);
            this.avatarDataGridView.TabIndex = 1;
            // 
            // jobEquipmentInvTab
            // 
            this.jobEquipmentInvTab.Controls.Add(this.jobEquipmentDataGridView);
            this.jobEquipmentInvTab.Location = new System.Drawing.Point(4, 22);
            this.jobEquipmentInvTab.Name = "jobEquipmentInvTab";
            this.jobEquipmentInvTab.Size = new System.Drawing.Size(754, 321);
            this.jobEquipmentInvTab.TabIndex = 3;
            this.jobEquipmentInvTab.Text = "Job Equipment";
            this.jobEquipmentInvTab.UseVisualStyleBackColor = true;
            // 
            // jobEquipmentDataGridView
            // 
            this.jobEquipmentDataGridView.AllowUserToAddRows = false;
            this.jobEquipmentDataGridView.AllowUserToDeleteRows = false;
            this.jobEquipmentDataGridView.AllowUserToOrderColumns = true;
            this.jobEquipmentDataGridView.AllowUserToResizeColumns = false;
            this.jobEquipmentDataGridView.AllowUserToResizeRows = false;
            this.jobEquipmentDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.jobEquipmentDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.jobEquipmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobEquipmentDataGridView.Location = new System.Drawing.Point(6, 6);
            this.jobEquipmentDataGridView.MultiSelect = false;
            this.jobEquipmentDataGridView.Name = "jobEquipmentDataGridView";
            this.jobEquipmentDataGridView.ReadOnly = true;
            this.jobEquipmentDataGridView.RowHeadersVisible = false;
            this.jobEquipmentDataGridView.Size = new System.Drawing.Size(742, 308);
            this.jobEquipmentDataGridView.TabIndex = 1;
            // 
            // chatTab
            // 
            this.chatTab.Controls.Add(this.chatBox);
            this.chatTab.Location = new System.Drawing.Point(4, 22);
            this.chatTab.Name = "chatTab";
            this.chatTab.Padding = new System.Windows.Forms.Padding(3);
            this.chatTab.Size = new System.Drawing.Size(768, 381);
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
            this.chatBox.Size = new System.Drawing.Size(765, 342);
            this.chatBox.TabIndex = 0;
            // 
            // attackTab
            // 
            this.attackTab.Controls.Add(this.attackButton);
            this.attackTab.Controls.Add(this.removeSkillListBox);
            this.attackTab.Controls.Add(this.addSkillListBox);
            this.attackTab.Controls.Add(this.removeEntityButton);
            this.attackTab.Controls.Add(this.addEntityButton);
            this.attackTab.Controls.Add(this.nearEntitiesListBox);
            this.attackTab.Controls.Add(this.availSkillsListBox);
            this.attackTab.Controls.Add(label2);
            this.attackTab.Controls.Add(label1);
            this.attackTab.Controls.Add(this.attackEntitiesListBox);
            this.attackTab.Controls.Add(this.attackSkillsListBox);
            this.attackTab.Location = new System.Drawing.Point(4, 22);
            this.attackTab.Name = "attackTab";
            this.attackTab.Size = new System.Drawing.Size(768, 381);
            this.attackTab.TabIndex = 5;
            this.attackTab.Text = "Attack";
            this.attackTab.UseVisualStyleBackColor = true;
            // 
            // attackButton
            // 
            this.attackButton.Location = new System.Drawing.Point(653, 341);
            this.attackButton.Name = "attackButton";
            this.attackButton.Size = new System.Drawing.Size(75, 23);
            this.attackButton.TabIndex = 11;
            this.attackButton.Text = "Attack";
            this.attackButton.UseVisualStyleBackColor = true;
            this.attackButton.Click += new System.EventHandler(this.StartAttack);
            // 
            // removeSkillListBox
            // 
            this.removeSkillListBox.Location = new System.Drawing.Point(167, 194);
            this.removeSkillListBox.Name = "removeSkillListBox";
            this.removeSkillListBox.Size = new System.Drawing.Size(27, 23);
            this.removeSkillListBox.TabIndex = 10;
            this.removeSkillListBox.Text = "-";
            this.removeSkillListBox.UseVisualStyleBackColor = true;
            this.removeSkillListBox.Click += new System.EventHandler(this.RemoveSkill);
            // 
            // addSkillListBox
            // 
            this.addSkillListBox.Location = new System.Drawing.Point(168, 140);
            this.addSkillListBox.Name = "addSkillListBox";
            this.addSkillListBox.Size = new System.Drawing.Size(27, 23);
            this.addSkillListBox.TabIndex = 9;
            this.addSkillListBox.Text = "+";
            this.addSkillListBox.UseVisualStyleBackColor = true;
            this.addSkillListBox.Click += new System.EventHandler(this.AddSkill);
            // 
            // removeEntityButton
            // 
            this.removeEntityButton.Location = new System.Drawing.Point(573, 194);
            this.removeEntityButton.Name = "removeEntityButton";
            this.removeEntityButton.Size = new System.Drawing.Size(27, 23);
            this.removeEntityButton.TabIndex = 8;
            this.removeEntityButton.Text = "-";
            this.removeEntityButton.UseVisualStyleBackColor = true;
            this.removeEntityButton.Click += new System.EventHandler(this.RemoveEntity);
            // 
            // addEntityButton
            // 
            this.addEntityButton.Location = new System.Drawing.Point(574, 140);
            this.addEntityButton.Name = "addEntityButton";
            this.addEntityButton.Size = new System.Drawing.Size(27, 23);
            this.addEntityButton.TabIndex = 7;
            this.addEntityButton.Text = "+";
            this.addEntityButton.UseVisualStyleBackColor = true;
            this.addEntityButton.Click += new System.EventHandler(this.AddEntity);
            // 
            // nearEntitiesListBox
            // 
            this.nearEntitiesListBox.FormattingEnabled = true;
            this.nearEntitiesListBox.Location = new System.Drawing.Point(606, 96);
            this.nearEntitiesListBox.Name = "nearEntitiesListBox";
            this.nearEntitiesListBox.Size = new System.Drawing.Size(147, 160);
            this.nearEntitiesListBox.TabIndex = 5;
            // 
            // availSkillsListBox
            // 
            this.availSkillsListBox.FormattingEnabled = true;
            this.availSkillsListBox.Location = new System.Drawing.Point(15, 96);
            this.availSkillsListBox.Name = "availSkillsListBox";
            this.availSkillsListBox.Size = new System.Drawing.Size(147, 160);
            this.availSkillsListBox.TabIndex = 4;
            // 
            // attackEntitiesListBox
            // 
            this.attackEntitiesListBox.FormattingEnabled = true;
            this.attackEntitiesListBox.Location = new System.Drawing.Point(421, 96);
            this.attackEntitiesListBox.Name = "attackEntitiesListBox";
            this.attackEntitiesListBox.Size = new System.Drawing.Size(147, 160);
            this.attackEntitiesListBox.TabIndex = 1;
            // 
            // attackSkillsListBox
            // 
            this.attackSkillsListBox.FormattingEnabled = true;
            this.attackSkillsListBox.Location = new System.Drawing.Point(201, 96);
            this.attackSkillsListBox.Name = "attackSkillsListBox";
            this.attackSkillsListBox.Size = new System.Drawing.Size(147, 160);
            this.attackSkillsListBox.TabIndex = 0;
            // 
            // movementTab
            // 
            this.movementTab.Controls.Add(this.mapVisibilityCheckbox);
            this.movementTab.Controls.Add(this.mapPanel);
            this.movementTab.Controls.Add(currWorldLabel);
            this.movementTab.Controls.Add(this.currWorldLabelValue);
            this.movementTab.Controls.Add(currLocalLabel);
            this.movementTab.Controls.Add(this.currLocalLabelValue);
            this.movementTab.Location = new System.Drawing.Point(4, 22);
            this.movementTab.Name = "movementTab";
            this.movementTab.Size = new System.Drawing.Size(768, 381);
            this.movementTab.TabIndex = 4;
            this.movementTab.Text = "Movement";
            this.movementTab.UseVisualStyleBackColor = true;
            // 
            // mapVisibilityCheckbox
            // 
            this.mapVisibilityCheckbox.Checked = true;
            this.mapVisibilityCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mapVisibilityCheckbox.Location = new System.Drawing.Point(647, 357);
            this.mapVisibilityCheckbox.Name = "mapVisibilityCheckbox";
            this.mapVisibilityCheckbox.Size = new System.Drawing.Size(104, 16);
            this.mapVisibilityCheckbox.TabIndex = 12;
            this.mapVisibilityCheckbox.Text = "Visible";
            this.mapVisibilityCheckbox.UseVisualStyleBackColor = true;
            // 
            // mapPanel
            // 
            this.mapPanel.Location = new System.Drawing.Point(3, 3);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(762, 352);
            this.mapPanel.TabIndex = 11;
            // 
            // currWorldLabelValue
            // 
            this.currWorldLabelValue.Location = new System.Drawing.Point(336, 358);
            this.currWorldLabelValue.Name = "currWorldLabelValue";
            this.currWorldLabelValue.Size = new System.Drawing.Size(179, 23);
            this.currWorldLabelValue.TabIndex = 8;
            this.currWorldLabelValue.Text = "0, 0";
            // 
            // currLocalLabelValue
            // 
            this.currLocalLabelValue.Location = new System.Drawing.Point(52, 358);
            this.currLocalLabelValue.Name = "currLocalLabelValue";
            this.currLocalLabelValue.Size = new System.Drawing.Size(278, 23);
            this.currLocalLabelValue.TabIndex = 4;
            this.currLocalLabelValue.Text = "0, 0";
            // 
            // devTab
            // 
            this.devTab.Controls.Add(this.packetLoggerGroupBox);
            this.devTab.Controls.Add(label4);
            this.devTab.Controls.Add(this.filterPacketTextBox);
            this.devTab.Controls.Add(this.filteredPacketsListBox);
            this.devTab.Controls.Add(this.debugAgCheckbox);
            this.devTab.Controls.Add(this.debugGwCheckbox);
            this.devTab.Location = new System.Drawing.Point(4, 22);
            this.devTab.Name = "devTab";
            this.devTab.Size = new System.Drawing.Size(768, 381);
            this.devTab.TabIndex = 3;
            this.devTab.Text = "Developer";
            // 
            // packetLoggerGroupBox
            // 
            this.packetLoggerGroupBox.Controls.Add(this.packetlogRtb);
            this.packetLoggerGroupBox.Location = new System.Drawing.Point(194, 3);
            this.packetLoggerGroupBox.Name = "packetLoggerGroupBox";
            this.packetLoggerGroupBox.Size = new System.Drawing.Size(562, 368);
            this.packetLoggerGroupBox.TabIndex = 6;
            this.packetLoggerGroupBox.TabStop = false;
            this.packetLoggerGroupBox.Text = "Packet logger";
            // 
            // packetlogRtb
            // 
            this.packetlogRtb.Location = new System.Drawing.Point(6, 19);
            this.packetlogRtb.Name = "packetlogRtb";
            this.packetlogRtb.ReadOnly = true;
            this.packetlogRtb.Size = new System.Drawing.Size(550, 343);
            this.packetlogRtb.TabIndex = 2;
            this.packetlogRtb.Text = "";
            // 
            // filterPacketTextBox
            // 
            this.filterPacketTextBox.Location = new System.Drawing.Point(14, 172);
            this.filterPacketTextBox.Name = "filterPacketTextBox";
            this.filterPacketTextBox.Size = new System.Drawing.Size(142, 20);
            this.filterPacketTextBox.TabIndex = 4;
            // 
            // filteredPacketsListBox
            // 
            this.filteredPacketsListBox.FormattingEnabled = true;
            this.filteredPacketsListBox.Location = new System.Drawing.Point(14, 198);
            this.filteredPacketsListBox.Name = "filteredPacketsListBox";
            this.filteredPacketsListBox.Size = new System.Drawing.Size(142, 173);
            this.filteredPacketsListBox.TabIndex = 3;
            // 
            // debugAgCheckbox
            // 
            this.debugAgCheckbox.Location = new System.Drawing.Point(14, 43);
            this.debugAgCheckbox.Name = "debugAgCheckbox";
            this.debugAgCheckbox.Size = new System.Drawing.Size(104, 24);
            this.debugAgCheckbox.TabIndex = 1;
            this.debugAgCheckbox.Text = "Debug agent";
            this.debugAgCheckbox.UseVisualStyleBackColor = true;
            // 
            // debugGwCheckbox
            // 
            this.debugGwCheckbox.Location = new System.Drawing.Point(14, 13);
            this.debugGwCheckbox.Name = "debugGwCheckbox";
            this.debugGwCheckbox.Size = new System.Drawing.Size(104, 24);
            this.debugGwCheckbox.TabIndex = 0;
            this.debugGwCheckbox.Text = "Debug gateway";
            this.debugGwCheckbox.UseVisualStyleBackColor = true;
            // 
            // loggerBox
            // 
            this.loggerBox.FormattingEnabled = true;
            this.loggerBox.Location = new System.Drawing.Point(8, 425);
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
            // proxyPasswordBox
            // 
            this.proxyPasswordBox.Location = new System.Drawing.Point(162, 127);
            this.proxyPasswordBox.Name = "proxyPasswordBox";
            this.proxyPasswordBox.Size = new System.Drawing.Size(92, 20);
            this.proxyPasswordBox.TabIndex = 16;
            this.proxyPasswordBox.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(6, 130);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(57, 17);
            label6.TabIndex = 15;
            label6.Text = "Login";
            // 
            // proxyUsernameBox
            // 
            this.proxyUsernameBox.Location = new System.Drawing.Point(69, 127);
            this.proxyUsernameBox.Name = "proxyUsernameBox";
            this.proxyUsernameBox.Size = new System.Drawing.Size(87, 20);
            this.proxyUsernameBox.TabIndex = 14;
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 558);
            this.Controls.Add(this.loggerBox);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Gui";
            this.Text = "SimpleCL";
            buffsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.buffsDataGridView)).EndInit();
            statisticsBox.ResumeLayout(false);
            this.credentialsGroup.ResumeLayout(false);
            this.credentialsGroup.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.homeTab.ResumeLayout(false);
            this.inventoryPage.ResumeLayout(false);
            this.inventoryTabControl.ResumeLayout(false);
            this.inventoryInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.inventoryDataGridView)).EndInit();
            this.equipmentInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.equipmentDataGridView)).EndInit();
            this.avatarInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.avatarDataGridView)).EndInit();
            this.jobEquipmentInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.jobEquipmentDataGridView)).EndInit();
            this.chatTab.ResumeLayout(false);
            this.attackTab.ResumeLayout(false);
            this.movementTab.ResumeLayout(false);
            this.devTab.ResumeLayout(false);
            this.devTab.PerformLayout();
            this.packetLoggerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox proxyPasswordBox;
        private System.Windows.Forms.TextBox proxyUsernameBox;

        private System.Windows.Forms.TextBox proxyIpTextBox;
        private System.Windows.Forms.TextBox proxyPortTextBox;

        private System.Windows.Forms.GroupBox packetLoggerGroupBox;

        private System.Windows.Forms.RichTextBox packetlogRtb;
        private System.Windows.Forms.ListBox filteredPacketsListBox;
        private System.Windows.Forms.TextBox filterPacketTextBox;

        private System.Windows.Forms.CheckBox mapVisibilityCheckbox;

        private System.Windows.Forms.Label goldAmountLabel;

        private System.Windows.Forms.Button refreshInventoriesButton;

        private System.Windows.Forms.DataGridView buffsDataGridView;

        private System.Windows.Forms.Button addEntityButton;
        private System.Windows.Forms.Button removeEntityButton;
        private System.Windows.Forms.Button removeSkillListBox;
        private System.Windows.Forms.Button addSkillListBox;

        private System.Windows.Forms.ListBox nearEntitiesListBox;

        private System.Windows.Forms.Panel mapPanel;


        private System.Windows.Forms.Label currWorldLabelValue;

        private System.Windows.Forms.TabPage movementTab;
        private System.Windows.Forms.Label currLocalLabelValue;

        private System.Windows.Forms.TabPage devTab;
        private System.Windows.Forms.CheckBox debugGwCheckbox;
        private System.Windows.Forms.CheckBox debugAgCheckbox;

        private System.Windows.Forms.Label nameLabelValue;
        private System.Windows.Forms.Label jobNameLabelValue;
        private System.Windows.Forms.Label jobLevelLabelValue;

        private System.Windows.Forms.DataGridView inventoryDataGridView;
        private System.Windows.Forms.DataGridView equipmentDataGridView;
        private System.Windows.Forms.DataGridView avatarDataGridView;
        private System.Windows.Forms.DataGridView jobEquipmentDataGridView;

        private System.Windows.Forms.TabPage inventoryInvTab;
        private System.Windows.Forms.TabPage equipmentInvTab;
        private System.Windows.Forms.TabPage avatarInvTab;
        private System.Windows.Forms.TabPage jobEquipmentInvTab;

        private System.Windows.Forms.TabControl inventoryTabControl;

        private System.Windows.Forms.TabPage inventoryPage;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar2;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;

        private System.Windows.Forms.Label localCoordsLabelValue;
        private System.Windows.Forms.Label worldCoordsLabelValue;
        private System.Windows.Forms.Label spLabelValue;

        private System.Windows.Forms.Label goldLabelValue;

        private System.Windows.Forms.Label levelLabelValue;


        private System.Windows.Forms.TabPage homeTab;

        private System.Windows.Forms.TabPage chatTab;
        private System.Windows.Forms.ListBox chatBox;

        private System.Windows.Forms.ListBox loggerBox;

        private System.Windows.Forms.Button loginButton;

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TabControl tabControl;

        private System.Windows.Forms.TextBox usernameBox;

        private System.Windows.Forms.ComboBox serverComboBox;
        
        private System.Windows.Forms.TabPage attackTab;
        private System.Windows.Forms.ListBox attackSkillsListBox;
        private System.Windows.Forms.ListBox attackEntitiesListBox;
        private System.Windows.Forms.ListBox availSkillsListBox;

        #endregion
        
        private TextProgressBar expProgressBar;
        private TextProgressBar hpProgressBar;
        private TextProgressBar mpProgressBar;
        private TextProgressBar jobExpProgressBar;
        private System.Windows.Forms.Button attackButton;
        private System.Windows.Forms.GroupBox credentialsGroup;
    }
}