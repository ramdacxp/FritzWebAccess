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
                Console.WriteLine("Usage: FritzWebAccess <Password> [1|0]");
                Console.WriteLine("Optional 2nd parameter enables/disables WLAN.");
                return;
            }

            var password = args[0];
            bool setWirelessLan = args.Length == 2;
            bool enableWirelessLan = setWirelessLan && (args[1] == "1");

            // *******************************************************************************************

            Console.WriteLine("\nWeb Access");
            Console.WriteLine("~~~~~~~~~~");

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

            // *******************************************************************************************

            Console.WriteLine("\nSOAP Access");
            Console.WriteLine("~~~~~~~~~~~");

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                /// Console.WriteLine("Allowing HTTPS for: {0}", certificate.Issuer);
                return true;
            };

            var soap = new FritzSoapAccess
            {
                Password = password
            };

            if ( setWirelessLan )
            {
                Console.Write("SetWirelessLan({0}) ...", enableWirelessLan);
                soap.SetWirelessLan(enableWirelessLan);
                Console.WriteLine(" Done.");
            }

            Console.WriteLine("TotalBytesSent:       {0}", soap.GetTotalBytesSent());
            Console.WriteLine("TotalBytesReceived:   {0}", soap.GetTotalBytesReceived());

            Console.WriteLine("CommonLinkProperties: {0}", soap.GetCommonLinkProperties());
            Console.WriteLine("DslInterfaceInfo:     {0}", soap.GetDslInterfaceInfo());
            Console.WriteLine("ExternalIPAddress:    {0}", soap.GetExternalIPAddress());
            Console.WriteLine("WirelessLanInfo:      {0}", soap.GetWirelessLanInfo());

            int numberOfHosts = soap.GetHostNumberOfEntries();
            Console.WriteLine("HostNumberOfEntries:  {0}", numberOfHosts);
            for( int i=0; i<numberOfHosts; i++)
            {
                Console.WriteLine("#{0} {1}", i, soap.GetGenericHostEntryExt(i));
            }




            // *******************************************************************************************

            Console.WriteLine("\n(Return)");
            Console.ReadLine();
        }


    }
}
