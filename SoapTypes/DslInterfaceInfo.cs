using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzWebAccess.SoapTypes
{
    public class DslInterfaceInfo
    {
        public string Status { get; set; }
        
        // Datenraten in kbit/s
        public string CurrentUpstreamRate { get; set; }
        public string CurrentDownstreamRate { get; set; }
        public string MaxUpstreamRate { get; set; }
        public string MaxDownstreamRate { get; set; }
        
        // Störabstandsmarge in dB*10
        public string UpstreamNoiseMargin { get; set; }
        public string DownstreamNoiseMargin { get; set; }
        
        // Leistungsdämpfung in dB*10
        public string UpstreamAttenuation { get; set; }
        public string DownstreamAttenuation { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Status: {0}, Up/Down- RateCurrent: {1}/{2}, RateMax: {3}/{4}, NoiseMargin: {5}/{6}, Attenuation: {7}/{8}",
                Status, 
                CurrentUpstreamRate, CurrentDownstreamRate, MaxUpstreamRate, MaxDownstreamRate,
                UpstreamNoiseMargin, DownstreamNoiseMargin, UpstreamAttenuation, DownstreamAttenuation);
        }
    }
}
