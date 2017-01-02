using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzWebAccess.SoapTypes
{
    public class CommonLinkProperties
    {
        public string AccessType { get; set; }
        public string MaxUpstreamBitRate { get; set; }
        public string MaxDownstreamBitRate { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format(
                "AccessType: {0}, MaxUpstreamBitRate: {1}, MaxDownstreamBitRate: {2}, Status: {3}",
                AccessType, MaxUpstreamBitRate, MaxDownstreamBitRate, Status);
        }
    }
}
