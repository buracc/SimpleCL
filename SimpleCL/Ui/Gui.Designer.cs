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
            this.worldCoordsLabelValue = new System.Windows.Forms.Label();
            this.worldCoordsLabel = new System.Windows.Forms.Label();
            this.jobExpProgressBar = new global::SimpleCL.Ui.Components.TextProgressBar();
            this.hpProgressBar = new global::SimpleCL.Ui.Components.TextProgressBar();
            this.mpProgressBar = new global::SimpleCL.Ui.Components.TextProgressBar();
            this.expProgressBar = new global::SimpleCL.Ui.Components.TextProgressBar();
            this.jobExpLabel = new System.Windows.Forms.Label();
            this.jobLevelLabelValue = new System.Windows.Forms.Label();
            this.jobLevelLabel = new System.Windows.Forms.Label();
            this.jobNameLabelValue = new System.Windows.Forms.Label();
            this.jobNameLabel = new System.Windows.Forms.Label();
            this.nameLabelValue = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.localCoordsLabelValue = new System.Windows.Forms.Label();
            this.goldLabelValue = new System.Windows.Forms.Label();
            this.spLabelValue = new System.Windows.Forms.Label();
            this.levelLabelValue = new System.Windows.Forms.Label();
            this.localCoordsLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.spLabel = new System.Windows.Forms.Label();
            this.expLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.mpLabel = new System.Windows.Forms.Label();
            this.hpLabel = new System.Windows.Forms.Label();
            this.inventoryPage = new System.Windows.Forms.TabPage();
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
            this.removeSkillListBox = new System.Windows.Forms.Button();
            this.addSkillListBox = new System.Windows.Forms.Button();
            this.removeEntityButton = new System.Windows.Forms.Button();
            this.addEntityButton = new System.Windows.Forms.Button();
            this.refreshEntitiesButton = new System.Windows.Forms.Button();
            this.nearEntitiesListBox = new System.Windows.Forms.ListBox();
            this.availSkillsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.attackEntitiesListBox = new System.Windows.Forms.ListBox();
            this.attackSkillsListBox = new System.Windows.Forms.ListBox();
            this.movementTab = new System.Windows.Forms.TabPage();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.minimap = new global::SimpleCL.Ui.Components.Map();
            this.currWorldLabel = new System.Windows.Forms.Label();
            this.currWorldLabelValue = new System.Windows.Forms.Label();
            this.currLocalLabel = new System.Windows.Forms.Label();
            this.currLocalLabelValue = new System.Windows.Forms.Label();
            this.devTab = new System.Windows.Forms.TabPage();
            this.debugAgCheckbox = new System.Windows.Forms.CheckBox();
            this.debugGwCheckbox = new System.Windows.Forms.CheckBox();
            this.loggerBox = new System.Windows.Forms.ListBox();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripProgressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            this.attackButton = new System.Windows.Forms.Button();
            credentialsGroup = new System.Windows.Forms.GroupBox();
            credentialsGroup.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.homeTab.SuspendLayout();
            this.statisticsBox.SuspendLayout();
            this.inventoryPage.SuspendLayout();
            this.inventoryTabControl.SuspendLayout();
            this.inventoryInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDataGridView)).BeginInit();
            this.equipmentInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.equipmentDataGridView)).BeginInit();
            this.avatarInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatarDataGridView)).BeginInit();
            this.jobEquipmentInvTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobEquipmentDataGridView)).BeginInit();
            this.chatTab.SuspendLayout();
            this.attackTab.SuspendLayout();
            this.movementTab.SuspendLayout();
            this.mapPanel.SuspendLayout();
            this.devTab.SuspendLayout();
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
            credentialsGroup.Size = new System.Drawing.Size(262, 113);
            credentialsGroup.TabIndex = 6;
            credentialsGroup.TabStop = false;
            credentialsGroup.Text = "Credentials";
            // 
            // serverComboBox
            // 
            this.serverComboBox.Location = new System.Drawing.Point(69, 74);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(122, 21);
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
            this.homeTab.Controls.Add(this.statisticsBox);
            this.homeTab.Controls.Add(credentialsGroup);
            this.homeTab.Location = new System.Drawing.Point(4, 22);
            this.homeTab.Name = "homeTab";
            this.homeTab.Padding = new System.Windows.Forms.Padding(3);
            this.homeTab.Size = new System.Drawing.Size(768, 381);
            this.homeTab.TabIndex = 0;
            this.homeTab.Text = "Home";
            this.homeTab.UseVisualStyleBackColor = true;
            // 
            // statisticsBox
            // 
            this.statisticsBox.Controls.Add(this.worldCoordsLabelValue);
            this.statisticsBox.Controls.Add(this.worldCoordsLabel);
            this.statisticsBox.Controls.Add(this.jobExpProgressBar);
            this.statisticsBox.Controls.Add(this.hpProgressBar);
            this.statisticsBox.Controls.Add(this.mpProgressBar);
            this.statisticsBox.Controls.Add(this.expProgressBar);
            this.statisticsBox.Controls.Add(this.jobExpLabel);
            this.statisticsBox.Controls.Add(this.jobLevelLabelValue);
            this.statisticsBox.Controls.Add(this.jobLevelLabel);
            this.statisticsBox.Controls.Add(this.jobNameLabelValue);
            this.statisticsBox.Controls.Add(this.jobNameLabel);
            this.statisticsBox.Controls.Add(this.nameLabelValue);
            this.statisticsBox.Controls.Add(this.nameLabel);
            this.statisticsBox.Controls.Add(this.localCoordsLabelValue);
            this.statisticsBox.Controls.Add(this.goldLabelValue);
            this.statisticsBox.Controls.Add(this.spLabelValue);
            this.statisticsBox.Controls.Add(this.levelLabelValue);
            this.statisticsBox.Controls.Add(this.localCoordsLabel);
            this.statisticsBox.Controls.Add(this.goldLabel);
            this.statisticsBox.Controls.Add(this.spLabel);
            this.statisticsBox.Controls.Add(this.expLabel);
            this.statisticsBox.Controls.Add(this.levelLabel);
            this.statisticsBox.Controls.Add(this.mpLabel);
            this.statisticsBox.Controls.Add(this.hpLabel);
            this.statisticsBox.Location = new System.Drawing.Point(274, 6);
            this.statisticsBox.Name = "statisticsBox";
            this.statisticsBox.Size = new System.Drawing.Size(488, 254);
            this.statisticsBox.TabIndex = 7;
            this.statisticsBox.TabStop = false;
            this.statisticsBox.Text = "Statistics";
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
            this.worldCoordsLabel.Location = new System.Drawing.Point(6, 224);
            this.worldCoordsLabel.Name = "worldCoordsLabel";
            this.worldCoordsLabel.Size = new System.Drawing.Size(78, 22);
            this.worldCoordsLabel.TabIndex = 30;
            this.worldCoordsLabel.Text = "World";
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
            this.jobExpProgressBar.VisualMode = global::SimpleCL.Ui.Components.ProgressBarDisplayMode.CustomText;
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
            this.hpProgressBar.VisualMode = global::SimpleCL.Ui.Components.ProgressBarDisplayMode.CurrProgress;
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
            this.mpProgressBar.VisualMode = global::SimpleCL.Ui.Components.ProgressBarDisplayMode.CurrProgress;
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
            this.expProgressBar.VisualMode = global::SimpleCL.Ui.Components.ProgressBarDisplayMode.CustomText;
            // 
            // jobExpLabel
            // 
            this.jobExpLabel.Location = new System.Drawing.Point(6, 135);
            this.jobExpLabel.Name = "jobExpLabel";
            this.jobExpLabel.Size = new System.Drawing.Size(78, 22);
            this.jobExpLabel.TabIndex = 24;
            this.jobExpLabel.Text = "Job EXP";
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
            this.jobLevelLabel.Location = new System.Drawing.Point(174, 91);
            this.jobLevelLabel.Name = "jobLevelLabel";
            this.jobLevelLabel.Size = new System.Drawing.Size(78, 22);
            this.jobLevelLabel.TabIndex = 22;
            this.jobLevelLabel.Text = "Job level";
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
            this.jobNameLabel.Location = new System.Drawing.Point(174, 22);
            this.jobNameLabel.Name = "jobNameLabel";
            this.jobNameLabel.Size = new System.Drawing.Size(78, 22);
            this.jobNameLabel.TabIndex = 20;
            this.jobNameLabel.Text = "Job alias";
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
            this.nameLabel.Location = new System.Drawing.Point(6, 22);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(78, 22);
            this.nameLabel.TabIndex = 18;
            this.nameLabel.Text = "Name";
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
            this.localCoordsLabel.Location = new System.Drawing.Point(6, 202);
            this.localCoordsLabel.Name = "localCoordsLabel";
            this.localCoordsLabel.Size = new System.Drawing.Size(78, 22);
            this.localCoordsLabel.TabIndex = 8;
            this.localCoordsLabel.Text = "Local";
            // 
            // goldLabel
            // 
            this.goldLabel.Location = new System.Drawing.Point(6, 180);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(78, 22);
            this.goldLabel.TabIndex = 7;
            this.goldLabel.Text = "Gold";
            // 
            // spLabel
            // 
            this.spLabel.Location = new System.Drawing.Point(6, 158);
            this.spLabel.Name = "spLabel";
            this.spLabel.Size = new System.Drawing.Size(78, 22);
            this.spLabel.TabIndex = 4;
            this.spLabel.Text = "SP";
            // 
            // expLabel
            // 
            this.expLabel.Location = new System.Drawing.Point(6, 113);
            this.expLabel.Name = "expLabel";
            this.expLabel.Size = new System.Drawing.Size(78, 22);
            this.expLabel.TabIndex = 3;
            this.expLabel.Text = "EXP";
            // 
            // levelLabel
            // 
            this.levelLabel.Location = new System.Drawing.Point(6, 91);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(78, 22);
            this.levelLabel.TabIndex = 2;
            this.levelLabel.Text = "Level";
            // 
            // mpLabel
            // 
            this.mpLabel.Location = new System.Drawing.Point(6, 69);
            this.mpLabel.Name = "mpLabel";
            this.mpLabel.Size = new System.Drawing.Size(78, 22);
            this.mpLabel.TabIndex = 1;
            this.mpLabel.Text = "MP";
            // 
            // hpLabel
            // 
            this.hpLabel.Location = new System.Drawing.Point(6, 47);
            this.hpLabel.Name = "hpLabel";
            this.hpLabel.Size = new System.Drawing.Size(78, 22);
            this.hpLabel.TabIndex = 0;
            this.hpLabel.Text = "HP";
            // 
            // inventoryPage
            // 
            this.inventoryPage.Controls.Add(this.inventoryTabControl);
            this.inventoryPage.Location = new System.Drawing.Point(4, 22);
            this.inventoryPage.Name = "inventoryPage";
            this.inventoryPage.Size = new System.Drawing.Size(768, 381);
            this.inventoryPage.TabIndex = 2;
            this.inventoryPage.Text = "Inventory";
            this.inventoryPage.UseVisualStyleBackColor = true;
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
            this.inventoryTabControl.Size = new System.Drawing.Size(762, 375);
            this.inventoryTabControl.TabIndex = 0;
            // 
            // inventoryInvTab
            // 
            this.inventoryInvTab.Controls.Add(this.inventoryDataGridView);
            this.inventoryInvTab.Location = new System.Drawing.Point(4, 22);
            this.inventoryInvTab.Name = "inventoryInvTab";
            this.inventoryInvTab.Padding = new System.Windows.Forms.Padding(3);
            this.inventoryInvTab.Size = new System.Drawing.Size(754, 349);
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
            this.inventoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inventoryDataGridView.Location = new System.Drawing.Point(0, 0);
            this.inventoryDataGridView.MultiSelect = false;
            this.inventoryDataGridView.Name = "inventoryDataGridView";
            this.inventoryDataGridView.ReadOnly = true;
            this.inventoryDataGridView.Size = new System.Drawing.Size(754, 349);
            this.inventoryDataGridView.TabIndex = 0;
            // 
            // equipmentInvTab
            // 
            this.equipmentInvTab.Controls.Add(this.equipmentDataGridView);
            this.equipmentInvTab.Location = new System.Drawing.Point(4, 22);
            this.equipmentInvTab.Name = "equipmentInvTab";
            this.equipmentInvTab.Padding = new System.Windows.Forms.Padding(3);
            this.equipmentInvTab.Size = new System.Drawing.Size(754, 349);
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
            this.equipmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.equipmentDataGridView.Location = new System.Drawing.Point(0, 0);
            this.equipmentDataGridView.MultiSelect = false;
            this.equipmentDataGridView.Name = "equipmentDataGridView";
            this.equipmentDataGridView.ReadOnly = true;
            this.equipmentDataGridView.Size = new System.Drawing.Size(754, 349);
            this.equipmentDataGridView.TabIndex = 1;
            // 
            // avatarInvTab
            // 
            this.avatarInvTab.Controls.Add(this.avatarDataGridView);
            this.avatarInvTab.Location = new System.Drawing.Point(4, 22);
            this.avatarInvTab.Name = "avatarInvTab";
            this.avatarInvTab.Size = new System.Drawing.Size(754, 349);
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
            this.avatarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.avatarDataGridView.Location = new System.Drawing.Point(0, 0);
            this.avatarDataGridView.MultiSelect = false;
            this.avatarDataGridView.Name = "avatarDataGridView";
            this.avatarDataGridView.ReadOnly = true;
            this.avatarDataGridView.Size = new System.Drawing.Size(754, 349);
            this.avatarDataGridView.TabIndex = 1;
            // 
            // jobEquipmentInvTab
            // 
            this.jobEquipmentInvTab.Controls.Add(this.jobEquipmentDataGridView);
            this.jobEquipmentInvTab.Location = new System.Drawing.Point(4, 22);
            this.jobEquipmentInvTab.Name = "jobEquipmentInvTab";
            this.jobEquipmentInvTab.Size = new System.Drawing.Size(754, 349);
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
            this.jobEquipmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobEquipmentDataGridView.Location = new System.Drawing.Point(0, 0);
            this.jobEquipmentDataGridView.MultiSelect = false;
            this.jobEquipmentDataGridView.Name = "jobEquipmentDataGridView";
            this.jobEquipmentDataGridView.ReadOnly = true;
            this.jobEquipmentDataGridView.Size = new System.Drawing.Size(754, 349);
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
            this.attackTab.Controls.Add(this.refreshEntitiesButton);
            this.attackTab.Controls.Add(this.nearEntitiesListBox);
            this.attackTab.Controls.Add(this.availSkillsListBox);
            this.attackTab.Controls.Add(this.label2);
            this.attackTab.Controls.Add(this.label1);
            this.attackTab.Controls.Add(this.attackEntitiesListBox);
            this.attackTab.Controls.Add(this.attackSkillsListBox);
            this.attackTab.Location = new System.Drawing.Point(4, 22);
            this.attackTab.Name = "attackTab";
            this.attackTab.Size = new System.Drawing.Size(768, 381);
            this.attackTab.TabIndex = 5;
            this.attackTab.Text = "Attack";
            this.attackTab.UseVisualStyleBackColor = true;
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
            // refreshEntitiesButton
            // 
            this.refreshEntitiesButton.Location = new System.Drawing.Point(606, 259);
            this.refreshEntitiesButton.Name = "refreshEntitiesButton";
            this.refreshEntitiesButton.Size = new System.Drawing.Size(75, 23);
            this.refreshEntitiesButton.TabIndex = 6;
            this.refreshEntitiesButton.Text = "Refresh";
            this.refreshEntitiesButton.UseVisualStyleBackColor = true;
            this.refreshEntitiesButton.Click += new System.EventHandler(this.RefreshEntities);
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
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(421, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Entities";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(201, 259);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Skills";
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
            this.movementTab.Controls.Add(this.mapPanel);
            this.movementTab.Controls.Add(this.currWorldLabel);
            this.movementTab.Controls.Add(this.currWorldLabelValue);
            this.movementTab.Controls.Add(this.currLocalLabel);
            this.movementTab.Controls.Add(this.currLocalLabelValue);
            this.movementTab.Location = new System.Drawing.Point(4, 22);
            this.movementTab.Name = "movementTab";
            this.movementTab.Size = new System.Drawing.Size(768, 381);
            this.movementTab.TabIndex = 4;
            this.movementTab.Text = "Movement";
            this.movementTab.UseVisualStyleBackColor = true;
            // 
            // mapPanel
            // 
            this.mapPanel.Controls.Add(this.minimap);
            this.mapPanel.Location = new System.Drawing.Point(3, 3);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(762, 352);
            this.mapPanel.TabIndex = 11;
            // 
            // minimap
            // 
            this.minimap.Location = new System.Drawing.Point(-31, -214);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(800, 800);
            this.minimap.TabIndex = 10;
            this.minimap.Zoom = ((byte)(1));
            // 
            // currWorldLabel
            // 
            this.currWorldLabel.Location = new System.Drawing.Point(291, 358);
            this.currWorldLabel.Name = "currWorldLabel";
            this.currWorldLabel.Size = new System.Drawing.Size(39, 23);
            this.currWorldLabel.TabIndex = 9;
            this.currWorldLabel.Text = "World";
            // 
            // currWorldLabelValue
            // 
            this.currWorldLabelValue.Location = new System.Drawing.Point(336, 358);
            this.currWorldLabelValue.Name = "currWorldLabelValue";
            this.currWorldLabelValue.Size = new System.Drawing.Size(278, 23);
            this.currWorldLabelValue.TabIndex = 8;
            this.currWorldLabelValue.Text = "0, 0";
            // 
            // currLocalLabel
            // 
            this.currLocalLabel.Location = new System.Drawing.Point(0, 358);
            this.currLocalLabel.Name = "currLocalLabel";
            this.currLocalLabel.Size = new System.Drawing.Size(46, 23);
            this.currLocalLabel.TabIndex = 7;
            this.currLocalLabel.Text = "Local";
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
            this.devTab.Controls.Add(this.debugAgCheckbox);
            this.devTab.Controls.Add(this.debugGwCheckbox);
            this.devTab.Location = new System.Drawing.Point(4, 22);
            this.devTab.Name = "devTab";
            this.devTab.Size = new System.Drawing.Size(768, 381);
            this.devTab.TabIndex = 3;
            this.devTab.Text = "Developer";
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
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 558);
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
            this.inventoryPage.ResumeLayout(false);
            this.inventoryTabControl.ResumeLayout(false);
            this.inventoryInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDataGridView)).EndInit();
            this.equipmentInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.equipmentDataGridView)).EndInit();
            this.avatarInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.avatarDataGridView)).EndInit();
            this.jobEquipmentInvTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.jobEquipmentDataGridView)).EndInit();
            this.chatTab.ResumeLayout(false);
            this.attackTab.ResumeLayout(false);
            this.movementTab.ResumeLayout(false);
            this.mapPanel.ResumeLayout(false);
            this.devTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button addEntityButton;
        private System.Windows.Forms.Button removeEntityButton;
        private System.Windows.Forms.Button removeSkillListBox;
        private System.Windows.Forms.Button addSkillListBox;

        private System.Windows.Forms.ListBox nearEntitiesListBox;

        private System.Windows.Forms.Panel mapPanel;


        private System.Windows.Forms.Label currLocalLabel;
        private System.Windows.Forms.Label currWorldLabel;
        private System.Windows.Forms.Label currWorldLabelValue;

        private System.Windows.Forms.Label worldCoordsLabel;
        private System.Windows.Forms.Label localCoordsLabel;

        private System.Windows.Forms.TabPage movementTab;
        private System.Windows.Forms.Label currLocalLabelValue;

        private System.Windows.Forms.TabPage devTab;
        private System.Windows.Forms.CheckBox debugGwCheckbox;
        private System.Windows.Forms.CheckBox debugAgCheckbox;
        
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label nameLabelValue;
        private System.Windows.Forms.Label jobNameLabelValue;
        private System.Windows.Forms.Label jobNameLabel;
        private System.Windows.Forms.Label jobLevelLabelValue;
        private System.Windows.Forms.Label jobLevelLabel;

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

        private System.Windows.Forms.Label jobExpLabel;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar2;

        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;

        private System.Windows.Forms.Label localCoordsLabelValue;
        private System.Windows.Forms.Label worldCoordsLabelValue;
        private System.Windows.Forms.Label spLabelValue;

        private System.Windows.Forms.Label goldLabelValue;

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
        private System.Windows.Forms.Button refreshEntitiesButton;

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Label passwordLabel;

        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label usernameLabel;

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.ComboBox serverComboBox;
        
        private System.Windows.Forms.TabPage attackTab;
        private System.Windows.Forms.ListBox attackSkillsListBox;
        private System.Windows.Forms.ListBox attackEntitiesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox availSkillsListBox;

        #endregion

        
        private global::SimpleCL.Ui.Components.Map minimap;
        private global::SimpleCL.Ui.Components.TextProgressBar expProgressBar;
        private global::SimpleCL.Ui.Components.TextProgressBar hpProgressBar;
        private global::SimpleCL.Ui.Components.TextProgressBar mpProgressBar;
        private global::SimpleCL.Ui.Components.TextProgressBar jobExpProgressBar;
        private Button attackButton;
    }
}