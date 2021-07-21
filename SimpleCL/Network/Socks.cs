using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleCL.Network
{
    public class Socks
    {
        private readonly string _socksAddr;
        private readonly int _socksPort;
        private readonly string _destAddr;
        private readonly int _destPort;
        private readonly string _username;
        private readonly string _password;
        private readonly Socket _socket;

        private const int SOCKS_VER = 0x05;
        private const int LEN_METHODS = 0x02;
        private const int USER_PASS_AUTH = 0x02;
        private const int NO_AUTH = 0x00;
        private const int CMD_CONNECT = 0x01;
        private const int SOCKS_ADDR_TYPE_IPV4 = 0x01;
        private const int SOCKS_ADDR_TYPE_IPV6 = 0x04;
        private const int SOCKS_ADDR_TYPE_DOMAIN_NAME = 0x03;
        private const int AUTH_METHOD_NOT_SUPPORTED = 0xFF;
        private const int SOCKS_CMD_SUCCESS = 0x00;
        private const int AUTH_VERSION = 0x01;

        private Socks(string socksAddress, int socksPort, string destAddress, int destPort, string username,
            string password)
        {
            _socksAddr = socksAddress;
            _socksPort = socksPort;
            _destAddr = destAddress;
            _destPort = destPort;
            _username = username;
            _password = password;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private Socket Connect()
        {
            _socket.Connect(_socksAddr, _socksPort);
            var buffer = new byte[] {SOCKS_VER, LEN_METHODS, NO_AUTH, USER_PASS_AUTH};
            
            _socket.Send(buffer);
            _socket.Receive(buffer, 0, 2, SocketFlags.None);
            
            switch (buffer[1])
            {
                case AUTH_METHOD_NOT_SUPPORTED:
                    _socket.Close();
                    throw new SocksAuthException("Authentication method not supported");
                case USER_PASS_AUTH when _username == null || _password == null:
                    _socket.Close();
                    throw new ArgumentException("No username or password provided");
            }


            if (buffer[1] == USER_PASS_AUTH)
            {
                var credentials = new byte[_username.Length + _password.Length + 3];
                credentials[0] = AUTH_VERSION;
                credentials[1] = (byte) _username.Length;
                Encoding.ASCII.GetBytes(_username).CopyTo(credentials, 2);
                credentials[_username.Length + 2] = (byte) _password.Length;
                Encoding.ASCII.GetBytes(_password).CopyTo(credentials, _username.Length + 3);

                _socket.Send(credentials, credentials.Length, SocketFlags.None);
                
                buffer = new byte[2];
                _socket.Receive(buffer, buffer.Length, SocketFlags.None);

                if (buffer[1] != SOCKS_CMD_SUCCESS)
                {
                    throw new SocksRefusedException("Invalid username or password");
                }
            }

            var addrType = GetAddressType();
            var address = GetDestAddressBytes(addrType, _destAddr);
            var port = GetDestPortBytes(_destPort);

            buffer = new byte[4 + port.Length + address.Length];
            buffer[0] = SOCKS_VER;
            buffer[1] = CMD_CONNECT;
            buffer[2] = 0x00;
            buffer[3] = addrType;

            address.CopyTo(buffer, 4);
            port.CopyTo(buffer, 4 + address.Length);
            _socket.Send(buffer);

            buffer = new byte[255];
            _socket.Receive(buffer, buffer.Length, SocketFlags.None);

            if (buffer[1] == SOCKS_CMD_SUCCESS)
            {
                return _socket;
            }

            throw new SocksRefusedException("Connection failed");
        }

        private byte GetAddressType()
        {
            IPAddress ipAddr;
            var result = IPAddress.TryParse(_destAddr, out ipAddr);

            if (!result)
                return SOCKS_ADDR_TYPE_DOMAIN_NAME;

            return ipAddr.AddressFamily switch
            {
                AddressFamily.InterNetwork => SOCKS_ADDR_TYPE_IPV4,
                AddressFamily.InterNetworkV6 => SOCKS_ADDR_TYPE_IPV6,
                _ => throw new BadDestinationAddressException()
            };
        }

        private byte[] GetDestAddressBytes(byte addressType, string host)
        {
            switch (addressType)
            {
                case SOCKS_ADDR_TYPE_IPV4:
                case SOCKS_ADDR_TYPE_IPV6:
                    return IPAddress.Parse(host).GetAddressBytes();
                case SOCKS_ADDR_TYPE_DOMAIN_NAME:
                    var bytes = new byte[host.Length + 1];
                    bytes[0] = Convert.ToByte(host.Length);
                    Encoding.ASCII.GetBytes(host).CopyTo(bytes, 1);
                    return bytes;
                default:
                    return null;
            }
        }

        private byte[] GetDestPortBytes(int value)
        {
            var array = new byte[2];
            array[0] = Convert.ToByte(value / 256);
            array[1] = Convert.ToByte(value % 256);
            return array;
        }

        public static Socket Connect(string socksAddress, int socksPort, string destAddress, int destPort,
            string username, string password)
        {
            var client = new Socks(socksAddress, socksPort, destAddress, destPort, username, password);
            return client.Connect();
        }
    }

    [Serializable]
    public class SocksAuthException : Exception
    {
        public SocksAuthException()
        {
        }

        public SocksAuthException(string message)
            : base(message)
        {
        }

        public SocksAuthException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SocksAuthException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class BadDestinationAddressException : Exception
    {
        public BadDestinationAddressException()
        {
        }

        public BadDestinationAddressException(string message)
            : base(message)
        {
        }

        public BadDestinationAddressException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BadDestinationAddressException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class SocksRefusedException : Exception
    {
        public SocksRefusedException()
        {
        }

        public SocksRefusedException(string message)
            : base(message)
        {
        }

        public SocksRefusedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SocksRefusedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}