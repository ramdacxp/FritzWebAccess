using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzWebAccess.SoapTypes
{
    public class WirelessLanInfo
    {
        public bool IsEnabled { get; set; }
        public string Status { get; set; }
        public string Channel { get; set; }
        public string SSID { get; set; }

        public override string ToString()
        {
            return string.Format(
                "IsEnabled: {0}, Status: {1}, Channel: {2}, SSID: {3}",
                IsEnabled, Status, Channel, SSID );
        }
    }
}
