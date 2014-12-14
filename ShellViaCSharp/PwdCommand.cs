using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    class PwdCommand: Command
    {
        public PwdCommand()
        {
            CommandName = "pwd";
        }

        public override CodeAndMessage Process(string args, ref Env env)
        {
            //ignore every future features now, just print using Console
            Console.WriteLine(env.CurrentDirectory);

            return CodeAndMessage.Default;
        }
    }
}
