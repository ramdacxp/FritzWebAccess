
namespace FritzWebAccess.SoapTypes
{
    public class HostInfo
    {
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public bool IsActive { get; set; }
        public string HostName { get; set; }
        public string InterfaceType { get; set; }
        public string Port { get; set; }
        public string Speed { get; set; }
        
        public override string ToString()
        {
            return string.Format(
                "IP: {0}, MAC: {1}, IsActive: {2}, HostName: {3}, Interface: {4}, Port: {5}, Speed: {6}",
                IPAddress, MACAddress, IsActive, HostName, InterfaceType, Port, Speed
                );
        }
    }
}
