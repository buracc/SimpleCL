using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Enums;
using SimpleCL.Model;
using SimpleCL.Network;
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
            // characterListDataGridView.RowHeaderMouseDoubleClick += SelectCharacter;
            
            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();

            characterListDataGridView.KeyDown += (sender, args) =>
            {
                DataGridViewCell currentCell = characterListDataGridView.CurrentCell;
                if (args.KeyCode == Keys.Enter && currentCell != null)
                {
                    SelectCharacter(characterListDataGridView.Rows[currentCell.RowIndex], args);
                }
            };
        }

        private void SelectCharacter(object sender, EventArgs args)
        {
            CharSelect selected = (CharSelect) ((DataGridViewRow) sender).DataBoundItem;
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