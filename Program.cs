using System;
using System.Net;

namespace FritzWebAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: FritzWebAccess <Password>");
                return;
            }

            var password = args[0];


            var f = new FritzWebAccess();
            f.LogIn(password);

            Console.WriteLine("SessionId: {0}", f.SessionId);
            Console.WriteLine("IsAuthenticated: {0}", f.IsAuthenticated);

            // var caller = f.GetSoapContent("calllist.lua");
            // var caller = f.GetSoapContent("calllist.lua", "&days=0");

            var callerInfo = f.GetCallHistory(1);
            foreach (string line in callerInfo.Split(new[] { "\n" }, StringSplitOptions.None))
            {
                if ( line.StartsWith("<Call"))
                {
                    Console.WriteLine();
                    Console.WriteLine(line);
                }
            }

            // ----------------------------------------------------------

            Console.WriteLine("SOAP Tests");

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                /// Console.WriteLine("Allowing HTTPS for: {0}", certificate.Issuer);
                return true;
            };

            var soap = new FritzSoapAccess
            {
                Password = password
            };

            Console.WriteLine("HostNumberOfEntries:  {0}", soap.GetHostNumberOfEntries());
            Console.WriteLine("TotalBytesSent:       {0}", soap.GetTotalBytesSent());
            Console.WriteLine("TotalBytesReceived:   {0}", soap.GetTotalBytesReceived());

            Console.WriteLine("CommonLinkProperties: {0}", soap.GetCommonLinkProperties());
            Console.WriteLine("DslInterfaceInfo:     {0}", soap.GetDslInterfaceInfo());
            Console.WriteLine("ExternalIPAddress:    {0}", soap.GetExternalIPAddress());
            Console.WriteLine("WirelessLanInfo:      {0}", soap.GetWirelessLanInfo());

            

            Console.WriteLine("\n(Return)");
            Console.ReadLine();
        }




    }
}
