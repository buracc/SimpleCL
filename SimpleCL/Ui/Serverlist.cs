using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Models;
using SimpleCL.Models.Server;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    public partial class Serverlist : Form
    {
        private readonly string _username;
        private readonly string _password;
        private readonly Server _gateway;
        public Serverlist(string username, string password, IReadOnlyCollection<GameServer> servers, Server gateway)
        {
            _username = username;
            _password = password;
            _gateway = gateway;
            InitializeComponent();
            serverlistDataGridView.DataSource = servers;
            serverlistDataGridView.RowHeaderMouseDoubleClick += SelectServer;
            
            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();
            
            serverlistDataGridView.KeyDown += (sender, args) =>
            {
                var currentCell = serverlistDataGridView.CurrentCell;
                if (args.KeyCode == Keys.Enter && currentCell != null)
                {
                    SelectServer(serverlistDataGridView.Rows[currentCell.RowIndex], args);
                }
            };
        }

        private void SelectServer(object sender, EventArgs args)
        {
            var selected = (GameServer) ((DataGridViewRow) sender).DataBoundItem;
            if (selected == null)
            {
                return;
            }
            
            var login = new Packet(Opcode.Gateway.Request.LOGIN2, true);
            login.WriteByte(Locale.SRO_TR_Official_GameGami);
            login.WriteAscii(_username);
            login.WriteAscii(_password);
            login.WriteByteArray(NetworkUtils.GetMacAddressBytes());
            login.WriteUShort(selected.Id);
            login.WriteByte(1);
            
            _gateway.Inject(login);
            Dispose(true);
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _gateway.Disconnect();
            }
        }
    }
}