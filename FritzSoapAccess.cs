using System;
using System.Net;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using FritzWebAccess.SoapTypes;

// Sample Fritz!Box SOAP reqiuest and response

// POST https://fritz.box/tr064/upnp/control/hosts HTTP/1.1
// User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; MS Web Services Client Protocol 4.0.30319.42000)
// VsDebuggerCausalityData: uIDPo9xYFSZefCpIrRzN+SNIpikAAAAAmsBQEjXHW0mckwLsN9RYZqCTwkC7p69CrZEBm00SYoAACQAA
// Content-Type: text/xml; charset=utf-8
// SOAPAction: "urn:dslforum-org:service:Hosts:1#GetHostNumberOfEntries"
// Authorization: Digest username="admin", ...
// Host: fritz.box
// Content-Length: 313
// Expect: 100-continue
// 
// <?xml version="1.0" encoding="utf-8"?><soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" 
// xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><soap:Body><GetHostNumberOfEntries 
// xmlns="urn:dslforum-org:service:Hosts:1" /></soap:Body></soap:Envelope>
// HTTP/1.1 200 OK
// CONNECTION: close

// Connection: Keep-Alive
// Content-Length: 345
// CONTENT-TYPE: text/xml; charset="utf-8"
// DATE: Sun, 01 Jan 2017 20:45:23 GMT
// SERVER: FRITZ!Box 7490 (UI) UPnP/1.0 AVM FRITZ!Box 7490 (UI) 113.06.60
// EXT: 
// Keep-Alive: timeout=60, max=300
// 
// <?xml version="1.0"?>
// <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
// <s:Body>
// <u:GetHostNumberOfEntriesResponse xmlns:u="urn:dslforum-org:service:Hosts:1">
// <NewHostNumberOfEntries>13</NewHostNumberOfEntries>
// </u:GetHostNumberOfEntriesResponse>
// </s:Body>
// </s:Envelope>


namespace FritzWebAccess
{
    /// <summary>
    /// Access to Fritz!Box via UPnP (Soap).
    /// </summary>
    /// <remarks>
    /// UPnP must be anabled in the device settings.
    /// </remarks>
    public class FritzSoapAccess
    {
        public Uri BaseAddress { get; set; } = new Uri("https://fritz.box/");

        // default "admin" is used, if Fritz!Box security is set to "password only"
        public string Username { get; set; } = "admin";

        public string Password { get; set; } = string.Empty;

        public FritzSoapAccess()
        {
        }

        public int GetHostNumberOfEntries()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/hosts",
                "urn:dslforum-org:service:Hosts:1#GetHostNumberOfEntries"
                );

            // var resultString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Sample result:
            // <?xml version="1.0"?>
            // <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/" >
            //   <s:Body>
            //     <u:GetHostNumberOfEntriesResponse xmlns:u="urn:dslforum-org:service:Hosts:1">
            //       <NewHostNumberOfEntries>13</NewHostNumberOfEntries>
            //     </u:GetHostNumberOfEntriesResponse>
            //   </s:Body>
            // </s:Envelope>

            var result = GetElementValue(response.GetResponseStream(), "NewHostNumberOfEntries");
            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return -1;
        }

        public int GetTotalBytesSent()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wancommonifconfig1",
                "urn:dslforum-org:service:WANCommonInterfaceConfig:1#GetTotalBytesSent");
            var result = GetElementValue(response.GetResponseStream(), "NewTotalBytesSent");
            return (result != null) ? Convert.ToInt32(result) : -1;
        }

        public int GetTotalBytesReceived()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wancommonifconfig1",
                "urn:dslforum-org:service:WANCommonInterfaceConfig:1#GetTotalBytesReceived");
            var result = GetElementValue(response.GetResponseStream(), "NewTotalBytesReceived");
            return (result != null) ? Convert.ToInt32(result) : -1;
        }

        public CommonLinkProperties GetCommonLinkProperties()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wancommonifconfig1",
                "urn:dslforum-org:service:WANCommonInterfaceConfig:1#GetCommonLinkProperties");

            var values = GetElementValues(
                response.GetResponseStream(),
                new string[]
                {
                    "NewWANAccessType",
                    "NewLayer1UpstreamMaxBitRate",
                    "NewLayer1DownstreamMaxBitRate",
                    "NewPhysicalLinkStatus"
                });

            return new CommonLinkProperties
            {
                AccessType = values["NewWANAccessType"],
                MaxUpstreamBitRate = values["NewLayer1UpstreamMaxBitRate"],
                MaxDownstreamBitRate = values["NewLayer1DownstreamMaxBitRate"],
                Status = values["NewPhysicalLinkStatus"]
            };
        }

        public DslInterfaceInfo GetDslInterfaceInfo()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wandslifconfig1",
                "urn:dslforum-org:service:WANDSLInterfaceConfig:1#GetInfo");

            // return new StreamReader(response.GetResponseStream()).ReadToEnd();

            var values = GetElementValues(
                response.GetResponseStream(),
                new string[]
                {
                    "NewStatus",
                    "NewUpstreamCurrRate",        // Datenraten in kbit/s
                    "NewDownstreamCurrRate",
                    "NewUpstreamMaxRate",
                    "NewDownstreamMaxRate",
                    "NewUpstreamNoiseMargin",     // Störabstandsmarge in dB*10
                    "NewDownstreamNoiseMargin",
                    "NewUpstreamAttenuation",     // Leistungsdämpfung in dB*10
                    "NewDownstreamAttenuation"
                });

            return new DslInterfaceInfo
            {
                Status = values["NewStatus"],
                CurrentUpstreamRate = values["NewUpstreamCurrRate"],
                CurrentDownstreamRate = values["NewDownstreamCurrRate"],
                MaxUpstreamRate = values["NewUpstreamMaxRate"],
                MaxDownstreamRate = values["NewDownstreamMaxRate"],
                UpstreamNoiseMargin = values["NewUpstreamNoiseMargin"],
                DownstreamNoiseMargin = values["NewDownstreamNoiseMargin"],
                UpstreamAttenuation = values["NewUpstreamAttenuation"],
                DownstreamAttenuation = values["NewDownstreamAttenuation"]
            };
        }

        public string GetExternalIPAddress()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wanpppconn1",
                "urn:dslforum-org:service:WANPPPConnection:1#GetExternalIPAddress"
                );

            return GetElementValue(response.GetResponseStream(), "NewExternalIPAddress");
        }

        public WirelessLanInfo GetWirelessLanInfo()
        {
            var response = SendSoapRequest(
                "tr064/upnp/control/wlanconfig1",
                "urn:dslforum-org:service:WLANConfiguration:1#GetInfo"
                );

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }


        private HttpWebResponse SendSoapRequest(string relativeUrl, string soapAction)
        {
            // build absolute addr
            var requestAddress = new Uri(BaseAddress, relativeUrl);

            // create request
            var request = WebRequest.Create(requestAddress) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", soapAction);
            request.Credentials = new NetworkCredential(Username, Password);

            // set request content
            var SoapActionParts = soapAction.Split('#');
            Stream requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.ASCII);
            streamWriter.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            streamWriter.WriteLine(@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">");
            streamWriter.WriteLine(@"<s:Body>");
            streamWriter.WriteLine(@"<{1} xmlns=""{0}"" />", SoapActionParts[0], SoapActionParts[1]);
            streamWriter.WriteLine(@"</s:Body>");
            streamWriter.WriteLine(@"</s:Envelope>");
            streamWriter.Close();

            //send request and get response as string
            return request.GetResponse() as HttpWebResponse;
        }

        private string GetElementValue(Stream xmlStream, string elementName)
        {
            var responseDocument = new XPathDocument(xmlStream);
            var responseNavigator = responseDocument.CreateNavigator();
            var resultElement = responseNavigator.SelectSingleNode("//" + elementName);
            if (resultElement != null)
            {
                return resultElement.Value;
            }

            // not found
            return null;
        }

        private Dictionary<string, string> GetElementValues(Stream xmlStream, IEnumerable<string> elementNames)
        {
            var result = new Dictionary<string, string>();

            var responseDocument = new XPathDocument(xmlStream);
            var responseNavigator = responseDocument.CreateNavigator();

            foreach (string elementName in elementNames)
            {
                var resultElement = responseNavigator.SelectSingleNode("//" + elementName);
                if (resultElement != null)
                {
                    result[elementName] = resultElement.Value;
                }
            }

            return result;
        }


    }
}
