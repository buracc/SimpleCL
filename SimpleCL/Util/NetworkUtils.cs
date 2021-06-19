using System.Linq;
using System.Net.NetworkInformation;

namespace SimpleCL.Util
{
    public class NetworkUtils
    {
        public static byte[] GetMacAddressBytes()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic =>
                    nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().GetAddressBytes())
                .FirstOrDefault();
        }
    }
}