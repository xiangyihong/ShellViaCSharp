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
            RealCode();
            Console.ReadKey();
        }

        static void Test()
        {
            //FileEnumerator it = new FileEnumerator(@"", true, 0);
            // Console.WriteLine(it.Count());

            string normalArgs = "- -- abcd -a bcd -ab --abcd -ab aaa -abcdefghi abcd ";
            string badArgs = "---";
            foreach (var ov in new OptionEnumerator(normalArgs))
            {
                string option = ov.Option==null ? "null" : ov.Option;
                string value = ov.Value==null ? "null" : ov.Value;


                Console.WriteLine("{0}:{1}", option, value);
            }

            /*string value = null;
            switch(value)
            {
                case null:
                    Console.WriteLine("null");
                    break;
                default:
                    Console.WriteLine("default");
                    break;
            }*/
        }

        static void RealCode()
        {
            Shell shell = new Shell();

            ShellCode hr = shell.Run();

            Console.WriteLine("Shell Exited with code {0}", hr.ToString());
        }
    }
}
