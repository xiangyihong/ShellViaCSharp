using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    class Command
    {
        public Command()
        {

        }

        public virtual ShellCode Process(string args, ref Env env)
        {
            return ShellCode.OK;
        }
    }
}
