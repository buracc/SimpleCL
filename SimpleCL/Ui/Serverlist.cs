using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Model.Server;
using SimpleCL.Network;
using SimpleCL.Network.Enums;
using SimpleCL.Util;

namespace SimpleCL.Ui
{
    public partial class Serverlist : Form
    {
        private readonly Server _gateway;
        public Serverlist(List<SilkroadServer> servers, Server gateway)
        {
            _gateway = gateway;
            InitializeComponent();
            serverlistDataGridView.DataSource = servers;
            serverlistDataGridView.RowHeaderMouseDoubleClick += SelectServer;
            
            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();
        }

        private void SelectServer(object sender, DataGridViewCellMouseEventArgs args)
        {
            SilkroadServer selected = (SilkroadServer) ((DataGridView) sender).SelectedRows[0].DataBoundItem;
            if (selected == null)
            {
                return;
            }
            
            Packet login = new Packet(Opcodes.Gateway.Request.LOGIN2, true);
            login.WriteUInt8(Locale.SRO_TR_Official_GameGami);
            login.WriteAscii(Credentials.Username);
            login.WriteAscii(Credentials.Password);
            login.WriteUInt8Array(NetworkUtils.GetMacAddressBytes());
            login.WriteUInt16(selected.Id);
            login.WriteUInt8(1);
            
            _gateway.Inject(login);
            Dispose(true);
        }
    }
}