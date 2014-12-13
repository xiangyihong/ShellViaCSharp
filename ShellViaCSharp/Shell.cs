using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    sealed class Shell
    {
        private class CmdAndArgs
        {
            private readonly string cmd_;
            private readonly string args_;

            public string Cmd
            {
                get
                {
                    return cmd_;
                }
            }

            public string Args
            {
                get
                {
                    return args_;
                }
            }
            public CmdAndArgs(string c, string a)
            {
                cmd_ = c;
                args_ = a;
            }
        }

        public Shell()
        {
            env_ = new Env();
            commands_ = new Dictionary<string, Command>();
            userAlias_ = new Dictionary<string, string>();

            InitCommands();
            InitAlias();

        }

        public ShellCode Run()
        {
            string input;
            CmdAndArgs cmdAndArgs;
            ShellCode hr = ShellCode.OK;
            while (true)
            {
                input = Console.ReadLine();
                cmdAndArgs = ParseInput(input);

                //check if the cmd is built-in
                //only use if since only "quit" is supported
                if (cmdAndArgs.Cmd == "quit")
                {
                    return hr;
                }

                if (userAlias_.ContainsKey(cmdAndArgs.Cmd))
                {
                    input = userAlias_[cmdAndArgs.Cmd] + cmdAndArgs.Args;
                    cmdAndArgs = ParseInput(input);
                }

                if (commands_.ContainsKey(cmdAndArgs.Cmd))
                {
                    commands_[cmdAndArgs.Cmd].Process(cmdAndArgs.Args, ref env_);
                }
                else
                {
                    Console.WriteLine("Command not found!!!");
                }
            }
        }


        private CmdAndArgs ParseInput(string inputString)
        {
            string cmd = "";
            string args = "";

            if(inputString != null)
            {
                string s = RemoveExtraSpace(inputString);
                //args are not supported currently
                if (s.Length > 0)
                {
                    int index = s.IndexOf(' ');
                    if (index == -1)
                    {
                        index = s.Length;
                    }
                    cmd = s.Substring(0, index);
                }
            }
            
            return new CmdAndArgs(cmd, args);
        }

        private string RemoveExtraSpace(string s)
        {
            StringBuilder output = new StringBuilder();

            bool prevSpace = false;
            foreach(var c in s)
            {
                if(Char.IsWhiteSpace(c))
                {
                    if(prevSpace)
                    {
                        continue;
                    }
                    output.Append(' ');
                    prevSpace = true;
                }
                else
                {
                    prevSpace = false;
                    output.Append(c);
                }
            }

            return output.ToString();
        }
        private ReturnCode InitCommands()
        {
            return ReturnCode.OK;
        }

        private ReturnCode InitAlias()
        {
            return ReturnCode.OK;
        }

        private Dictionary<string, Command> commands_;
        private Dictionary<string, string> userAlias_;
        private Env env_;
    }
}
