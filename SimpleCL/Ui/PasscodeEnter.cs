using System;
using System.Text;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Enums;
using SimpleCL.Network;

namespace SimpleCL.Ui
{
    public partial class PasscodeEnter : Form
    {
        private readonly byte[] _key = {0x0F, 0x07, 0x3D, 0x20, 0x56, 0x62, 0xC9, 0xEB};
        private readonly Blowfish _blowfish = new Blowfish();
        private readonly Server _gateway;

        public PasscodeEnter(Server gateway, string title = "Enter passcode")
        {
            _gateway = gateway;
            _blowfish.Initialize(_key);
            InitializeComponent();
            Text = title;
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
            if (passcodeString.Length < 6 || passcodeString.Length > 8)
            {
                _gateway.Log("Passcode is 6-8 characters");
                return;
            }

            byte[] encodedPasscode = Encoding.ASCII.GetBytes(passcodeString);
            byte[] encryptedPasscode = _blowfish.Encode(encodedPasscode);

            Packet passcode = new Packet(Opcodes.Gateway.Request.PASSCODE, true);
            passcode.WriteUInt8(4);
            passcode.WriteUInt16(passcodeString.Length);
            passcode.WriteUInt8Array(encryptedPasscode);

            _gateway.Inject(passcode);
            Dispose(true);
        }
    }
}