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
            Shell shell = new Shell();

            ShellCode hr = shell.Run();

            Console.WriteLine("Shell Exited with code {0}", hr.ToString());
            Console.ReadKey();
        }
    }
}
