using SimpleCL.Util.Extension;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        public void AddChatMessage(string message)
        {
            this.InvokeLater(() =>
            {
                chatBox.Items.Add(message);
            });
        }
    }
}