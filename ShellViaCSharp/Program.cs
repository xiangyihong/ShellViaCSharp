using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        static void Test()
        {
            //FileEnumerator it = new FileEnumerator(@"", true, 0);
            // Console.WriteLine(it.Count());

            foreach(var ov in new OptionEnumerator("-a bcd -ab --abcd -ab aaa"))
            {
                string option = ov.Option==null ? "null" : ov.Option;
                string value = ov.Value==null ? "null" : ov.Value;


                Console.WriteLine("{0}:{1}", option, value);
            }
        }

        static void RealCode()
        {
            Shell shell = new Shell();

            ShellCode hr = shell.Run();

            Console.WriteLine("Shell Exited with code {0}", hr.ToString());
        }
    }
}
