using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Model.Game;
using SimpleCL.Model.Server;
using SimpleCL.Network;
using SimpleCL.Network.Enums;
using SimpleCL.Util;

namespace SimpleCL.Ui
{
    public partial class CharacterSelect : Form
    {
        private readonly Server _agent;
        public CharacterSelect(List<CharSelect> chars, Server agent)
        {
            _agent = agent;
            InitializeComponent();
            characterListDataGridView.DataSource = chars;
            characterListDataGridView.RowHeaderMouseDoubleClick += SelectCharacter;
        }

        private void SelectCharacter(object sender, DataGridViewCellMouseEventArgs args)
        {
            CharSelect selected = (CharSelect) ((DataGridView) sender).SelectedRows[0].DataBoundItem;
            if (selected == null)
            {
                return;
            }
            
            Packet characterJoin = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_JOIN);
            characterJoin.WriteAscii(selected.Name);
            _agent.Inject(characterJoin);
            Dispose(true);
        }
    }
}