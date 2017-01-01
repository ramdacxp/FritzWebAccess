using System;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace FritzWebAccess
{
    /// <summary>
    /// Access to Fritz!Box Web pages via authentification.
    /// </summary>
    /// <remarks>
    /// Requires a Fritz!Box running FRITZ!OS 5.50 or higher.
    /// API description can be found at:
    /// https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/AVM_Technical_Note_-_Session_ID.pdf
    /// </remarks>
    public class FritzWebAccess
    {
        public Uri Address { get; set; } = new Uri("http://fritz.box/");

        public Uri SoapAddress
        {
            get { return new Uri(Address.Scheme + "://" + Address.Host + ":49000/"); }
        }

        public string SessionId { get; private set; }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(SessionId) && (SessionId != "0000000000000000"); }
        }

        public FritzWebAccess()
        {
        }

        public void LogIn(string password)
        {
            LogIn(string.Empty, password);
        }

        public void LogIn(string username, string password)
        {
            var loginAddress = new Uri(Address, "login_sid.lua");
            var doc = XDocument.Load(loginAddress.AbsoluteUri);
            SessionId = GetValue(doc, "SID");

            if (!IsAuthenticated)
            {
                string challenge = GetValue(doc, "Challenge");
                string uri = string.Format(
                    @"{0}?username={1}&response={2}",
                    loginAddress.AbsoluteUri,
                    username,
                    GetAuthResponse(challenge, password));

                doc = XDocument.Load(uri);
                SessionId = GetValue(doc, "SID");
            }
        }

        private string GetValue(XDocument doc, string elementName)
        {
            XElement info = doc.FirstNode as XElement;
            return info.Element(elementName).Value;
        }

        private string GetAuthResponse(string challenge, string password)
        {
            return challenge + "-" + GetMD5Hash(challenge + "-" + password);
        }

        private string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public string GetWebContent(string relativeUrl, string parameter = "")
        {
            return GetContent(Address, relativeUrl, parameter);
        }

        public string GetSoapContent(string relativeUrl, string parameter = "")
        {
            return GetContent(SoapAddress, relativeUrl, parameter);
        }

        public string GetContent(Uri baseAddress, string relativeUrl, string parameter = "")
        {
            var uri = new Uri(baseAddress, relativeUrl + "?sid=" + SessionId + parameter);
            var request = WebRequest.Create(uri) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    return content;
                }
            }
        }

        /// <summary>
        /// Gets the telephone call history.
        /// </summary>
        /// <param name="numberOfDays">If given, the number of days to return; otherwise all.</param>
        /// <returns>Call history as XML string.</returns>
        public string GetCallHistory( int numberOfDays = -1 )
        {
            var parameter = (numberOfDays > 0) ? "&days=" + numberOfDays.ToString() : "";
            var callHistory = GetSoapContent("calllist.lua", parameter);

            //Format:
            //
            //<?xml version="1.0" encoding="utf-8"?>
            //<root>
            //  <!-- days:-1 -->
            //  <!-- max:999 -->
            //  <!-- id:nil -->
            //  <!-- timestamp:nil -->
            //  <!-- calls:400 -->
            //  <timestamp>1420905600</timestamp>
            //  <Call>
            //    <Id>2941</Id>
            //    <Type>1</Type>
            //    <Caller>01234555</Caller>
            //    <Called>SIP: 123456</Called>
            //    <CalledNumber>123456</CalledNumber>
            //    <Name>Mustermann, Max</Name>
            //    <Numbertype>sip</Numbertype>
            //    <Device>My Main Phone</Device>
            //    <Port>1</Port>
            //    <Date>01.01.17 13:33</Date>
            //    <Duration>0:11</Duration>
            //    <Count></Count>
            //    <Path />
            //  </Call>
            //  ...
            //</root>

            return callHistory;
        }

    }
}
