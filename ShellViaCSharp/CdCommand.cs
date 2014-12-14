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
            InitHistory();
            CommandName = "cd";
        }

        public override CodeAndMessage Process(string args, ref Env env)
        {
            ShellCode code = ShellCode.OK ;
            string message = null;
            toDir = null;

            ParseArgs(args, env);

            string fullToDir;
            if(Path.IsPathRooted(toDir))
            {
                fullToDir = toDir;
            }
            else
            {
                //avoid too many slashed
                string sep = string.Empty;
                if(!env.CurrentDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    sep = Path.DirectorySeparatorChar.ToString() ;
                }
                fullToDir = env.CurrentDirectory + sep + toDir;
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
                dirHistory_.Add(env.CurrentDirectory);
                env.CurrentDirectory = fullToDir;
            }
            catch(Exception)
            {
                code = ShellCode.NotAuthorized;
                message = string.Format("Cannot access to directory {0}", fullToDir);
            }

            return new CodeAndMessage(code, message);
        }

        private void ParseArgs(string args, Env env)
        {
            //may have more options
            //so do not finish foreach when found toDir
            foreach (OptionAndValue ov in new OptionEnumerator(args))
            {
                switch (ov.Option)
                {
                    case null:
                        toDir = ov.Value;
                        if(toDir == ".")
                        {
                            toDir = env.CurrentDirectory;
                        }
                        else if(toDir == "..")
                        {
                            var parentInfo = Directory.GetParent(env.CurrentDirectory);
                            if(parentInfo != null)
                            {
                                toDir = parentInfo.FullName;
                            }
                            else
                            {
                                toDir = env.CurrentDirectory;
                            }
                        }
                        break;
                    case "-":
                        break;
                    case "--":
                        int n = dirHistory_.Count;
                        if (n < 1)
                        {
                            toDir = env.HomeDirectory;
                        }
                        else
                        {
                            toDir = dirHistory_[n - 1];
                            dirHistory_.RemoveAt(n - 1);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void InitHistory()
        {
            dirHistory_ = new List<string>();
        }

        private string toDir;
        private List<string> dirHistory_;
    }
}
