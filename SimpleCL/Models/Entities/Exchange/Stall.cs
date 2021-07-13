using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleCL.Annotations;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Items;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Models.Entities.Exchange
{
    public class Stall : INotifyPropertyChanged, IDisposable
    {
        private bool _opened;
        
        public string Title { get; set; }
        public string Description { get; set; }
        public ulong PlayerUid { get; set; }
        
        public readonly BindingList<StallItem> Items = new();

        public string Status => Opened ? Constants.Strings.Opened : Constants.Strings.Modifying;
        

        public bool Opened
        {
            get => _opened;
            set
            {
                _opened = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public void Visit()
        {
            var openPacket = new Packet(Opcode.Agent.Request.STALL_TALK);
            openPacket.WriteUInt(PlayerUid);
            openPacket.Send();
        }


        public void Leave()
        {
            var exitPacket = new Packet(Opcode.Agent.Request.STALL_LEAVE);
            exitPacket.Send();
        }
        
        public void Dispose()
        {
            Program.Gui.InvokeLater(() =>
            {
                Items.DisposeAll();
                Items.Clear();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}