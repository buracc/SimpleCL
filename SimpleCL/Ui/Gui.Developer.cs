using System.ComponentModel;
using System.Windows.Forms;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        public BindingList<string> FilteredOpcodes = new();

        private void InitDevControls()
        {
            debugAgCheckbox.Checked = true;
            debugGwCheckbox.Checked = true;
            
            filterPacketTextBox.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    AddFilteredPacket(sender);
                }
            };

            filteredPacketsListBox.MouseDoubleClick += (sender, args) =>
            {
                if (sender is not ListBox listBox)
                {
                    return;
                }

                var selected = listBox.SelectedItem;
                if (selected is not string selectedOpcode)
                {
                    return;
                }

                FilteredOpcodes.Remove(selectedOpcode);
            };

            packetlogRtb.MouseDown += (_, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                var menu = new ContextMenu();
                var clear = new MenuItem("Clear");

                clear.Click += (_, _) => { packetlogRtb.Clear(); };

                menu.MenuItems.Add(clear);
                menu.Show(packetlogRtb, e.Location);
            };

            filteredPacketsListBox.DataSource = FilteredOpcodes;

            FilteredOpcodes.Add("0");
        }

        public bool DebugGateway()
        {
            return debugGwCheckbox.Checked;
        }

        public bool DebugAgent()
        {
            return debugAgCheckbox.Checked;
        }

        public void Log(string message, string sender = "SimpleCL")
        {
            this.InvokeLater(() =>
                {
                    loggerBox.Items.Add($"[{sender}] {message}");
                    loggerBox.SelectedIndex = loggerBox.Items.Count - 1;
                    loggerBox.SelectedIndex = -1;
                }
            );
        }

        private void AddFilteredPacket(object sender)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            try
            {
                FilteredOpcodes.Add(textBox.Text.Replace("0x", "").ToUpper());
            }
            catch
            {
                Log("Failed to add opcode to filtered list.");
            }
        }

        public void LogPacket(string data)
        {
            this.InvokeLater(() =>
            {
                if (packetlogRtb.TextLength > 1000000)
                {
                    packetlogRtb.Clear();
                }

                packetlogRtb.AppendText($"{data}\n\n");
                packetlogRtb.ScrollToCaret();
            });
        }
    }
}