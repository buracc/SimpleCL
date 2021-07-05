using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCL.SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Character;
using SimpleCL.Network;

namespace SimpleCL.Ui
{
    public partial class CharacterSelection : Form
    {
        private readonly Server _agent;
        public CharacterSelection(IReadOnlyCollection<Character> chars, Server agent)
        {
            _agent = agent;
            InitializeComponent();
            characterListDataGridView.DataSource = chars;
            
            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();

            characterListDataGridView.KeyDown += (_, args) =>
            {
                var currentCell = characterListDataGridView.CurrentCell;
                if (args.KeyCode == Keys.Enter && currentCell != null)
                {
                    SelectCharacter(characterListDataGridView.Rows[currentCell.RowIndex], args);
                }
            };
        }

        private void SelectCharacter(object sender, EventArgs args)
        {
            var selected = (Character) ((DataGridViewRow) sender).DataBoundItem;
            if (selected == null)
            {
                return;
            }

            LocalPlayer.Get.MaxHp = selected.Hp;
            LocalPlayer.Get.MaxMp = selected.Mp;
            var characterJoin = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_JOIN);
            characterJoin.WriteAscii(selected.Name);
            _agent.Inject(characterJoin);
            Dispose(true);
        }
    }
}