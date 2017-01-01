using System;

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

            var f = new FritzWebAccess();
            f.LogIn(args[0]);

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

            Console.WriteLine("\n(Return)");
            Console.ReadLine();
        }


    }
}
