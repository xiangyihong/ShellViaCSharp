using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShellViaCSharp
{
    class CdCommand: Command
    {
        public CdCommand()
        {

        }

        public override CodeAndMessage Process(string args, ref Env env)
        {
            ShellCode code = ShellCode.OK ;
            string message = null;

            string toDir = null;

            //may have more options
            //so do not finish foreach when found toDir
            foreach(OptionAndValue ov in new OptionEnumerator(args))
            {
                switch(ov.Option)
                {
                    case null:
                        toDir = ov.Value;
                        break;
                    case "-":
                        break;
                    case "--":
                        int n = env.DirectoryHistory.Count;
                        if(n < 1)
                        {
                            toDir = env.HomeDirectory;
                        }
                        else
                        {
                            toDir = env.DirectoryHistory[n - 1];
                            env.DirectoryHistory.RemoveAt(n - 1);
                        }
                        break;
                    default:
                        break;
                }
            }

            string fullToDir;
            if(Path.IsPathRooted(toDir))
            {
                fullToDir = toDir;
            }
            else
            {
                fullToDir = env.CurrentDirectory + Path.DirectorySeparatorChar + toDir;
            }

            if(!Directory.Exists(fullToDir))
            {
                code = ShellCode.NotExist;
                message = string.Format("Directory {0} not Exist!!!", fullToDir);
                return new CodeAndMessage(code, message);
            }

            try
            {
                //ugly way to test if the directory is accessable
                Directory.EnumerateFiles(fullToDir);

                //if not accessable, this code is never executed
                env.DirectoryHistory.Add(env.CurrentDirectory);
                env.CurrentDirectory = fullToDir;
            }
            catch(Exception)
            {
                code = ShellCode.NotAuthorized;
                message = string.Format("Cannot access to directory {0}", fullToDir);
            }

            return new CodeAndMessage(code, message);
        }
    }
}
