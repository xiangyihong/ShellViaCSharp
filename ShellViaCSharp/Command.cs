using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    abstract class Command
    {
        public Command()
        {

        }

        public string CommandName
        {
            get;
            protected set;
        }
        public virtual CodeAndMessage Process(string args, ref Env env)
        {
            return new CodeAndMessage(ShellCode.OK, null);
        }
    }
}
