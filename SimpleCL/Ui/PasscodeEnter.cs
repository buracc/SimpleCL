using System;
using System.Text;
using System.Windows.Forms;
using SimpleCL.Enums.Commons;
using SimpleCL.Network;
using SimpleCL.SecurityApi;

namespace SimpleCL.Ui
{
    public partial class PasscodeEnter : Form
    {
        private readonly byte[] _key = {0x0F, 0x07, 0x3D, 0x20, 0x56, 0x62, 0xC9, 0xEB};
        private readonly Blowfish _blowfish = new();
        private readonly Server _gateway;

        public PasscodeEnter(Server gateway, string title = "Enter passcode")
        {
            _gateway = gateway;
            _blowfish.Initialize(_key);
            InitializeComponent();
            base.Text = title;
            submitPasscode.Click += SubmitClicked;
            passcodeBox.KeyDown += SubmitClicked;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            CenterToScreen();

            passcodeBox.Text = Credentials.Passcode;
        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            if (e is KeyEventArgs eventArgs && eventArgs.KeyCode != Keys.Enter)
            {
                return;
            }

            SubmitPasscode(passcodeBox.Text);
        }

        public void SubmitPasscode(string passcodeString)
        {
            if (passcodeString.Length is < 6 or > 8)
            {
                _gateway.Log("Passcode is 6-8 characters");
                return;
            }

            var encodedPasscode = Encoding.ASCII.GetBytes(passcodeString);
            var encryptedPasscode = _blowfish.Encode(encodedPasscode);

            var passcode = new Packet(Opcodes.Gateway.Request.PASSCODE, true);
            passcode.WriteByte(4);
            passcode.WriteUShort(passcodeString.Length);
            passcode.WriteByteArray(encryptedPasscode);

            _gateway.Inject(passcode);
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