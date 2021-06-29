using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Models;
using SimpleCL.Models.Server;
using SimpleCL.Network;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    public partial class Serverlist : Form
    {
        private readonly Server _gateway;
        public Serverlist(List<GameServer> servers, Server gateway)
        {
            _gateway = gateway;
            InitializeComponent();
            serverlistDataGridView.DataSource = servers;
            serverlistDataGridView.RowHeaderMouseDoubleClick += SelectServer;
            
            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();
            
            serverlistDataGridView.KeyDown += (sender, args) =>
            {
                DataGridViewCell currentCell = serverlistDataGridView.CurrentCell;
                if (args.KeyCode == Keys.Enter && currentCell != null)
                {
                    SelectServer(serverlistDataGridView.Rows[currentCell.RowIndex], args);
                }
            };
        }

        private void SelectServer(object sender, EventArgs args)
        {
            GameServer selected = (GameServer) ((DataGridViewRow) sender).DataBoundItem;
            if (selected == null)
            {
                return;
            }
            
            Packet login = new Packet(Opcodes.Gateway.Request.LOGIN2, true);
            login.WriteByte(Locale.SRO_TR_Official_GameGami);
            login.WriteAscii(Credentials.Username);
            login.WriteAscii(Credentials.Password);
            login.WriteByteArray(NetworkUtils.GetMacAddressBytes());
            login.WriteUShort(selected.Id);
            login.WriteByte(1);
            
            _gateway.Inject(login);
            Dispose(true);
        }
    }
}