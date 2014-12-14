using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    class CodeAndMessage
    {
        public static CodeAndMessage Default = new CodeAndMessage(ShellCode.OK, null);

        public ShellCode Code
        {
            get
            {
                return code_;
            }
        }
        public string Message
        {
            get
            {
                return message_;
            }
        }

        public CodeAndMessage(ShellCode c, string m)
        {
            code_ = c;
            message_ = m;
        }

        private readonly ShellCode code_;
        private readonly string message_;
    }
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
            CodeAndMessage hr = CodeAndMessage.Default;

            while (true)
            {

                input = Console.ReadLine();
                cmdAndArgs = ParseInput(input);

                //check if the cmd is built-in
                //only use if since only "quit" is supported
                if (cmdAndArgs.Cmd == "quit")
                {
                    return hr.Code;
                }

                if (userAlias_.ContainsKey(cmdAndArgs.Cmd))
                {
                    input = userAlias_[cmdAndArgs.Cmd] + cmdAndArgs.Args;
                    cmdAndArgs = ParseInput(input);
                }

                if (commands_.ContainsKey(cmdAndArgs.Cmd))
                {
                    hr = commands_[cmdAndArgs.Cmd].Process(cmdAndArgs.Args, ref env_);
                    if(hr.Code != ShellCode.OK && hr.Message != null)
                    {
                        Console.WriteLine(hr.Message);
                    }
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
                
                if (s.Length > 0)
                {
                    int index = s.IndexOf(' ');
                    if (index == -1)
                    {
                        index = s.Length;
                    }
                    cmd = s.Substring(0, index);
                    args = s.Substring(index);
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
            commands_.Add("cd", new CdCommand());
            commands_.Add("pwd", new PwdCommand());

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
