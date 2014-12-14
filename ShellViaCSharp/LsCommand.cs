using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShellViaCSharp
{
    class LsCommand: Command
    {
        public LsCommand()
        {
            CommandName = "ls";
        }

        public override CodeAndMessage Process(string args, ref Env env)
        {
            ShellCode code = ShellCode.OK;
            string message = null;
            dirOrFile = null;
            ParseArgs(args, env);

            if(Directory.Exists(dirOrFile))
            {
                FileEnumeratorArgs enumArgs = new FileEnumeratorArgs();
                enumArgs.Recursive = resursive;
                enumArgs.Deep = deep;
                foreach(string name in new FileEnumerator(dirOrFile, enumArgs))
                {
                    DisplayFile(name);
                }
            }
            else if(File.Exists(dirOrFile))
            {
                DisplayFile(dirOrFile);
            }
            else
            {
                code = ShellCode.NotExist;
                message = string.Format("Cannot find {0}", dirOrFile);
            }

            return new CodeAndMessage(code, message);
        }

        private void ParseArgs(string args, Env env)
        {
            foreach(var ov in new OptionEnumerator(args))
            {
                switch(ov.Option)
                {
                    case null:
                        if(ov.Value == null)
                        {
                            dirOrFile = env.CurrentDirectory;
                        }
                        dirOrFile = ov.Value;
                        break;
                    default:
                        break;
                }
            }
            if(dirOrFile == null)
            {
                dirOrFile = env.CurrentDirectory;
            }
        }

        private void DisplayFile(string filename)
        {
            Console.WriteLine(filename);
        }

        private string dirOrFile;
        private int deep;
        private bool resursive;
    }
}
