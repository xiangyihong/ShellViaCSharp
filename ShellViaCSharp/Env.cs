using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    sealed class Env
    {
        public Env()
        {
            home_ = @"e:\";
            dirHistory_ = new List<string>();
            CurrentDirectory = home_;
        }

        public string CurrentDirectory
        {
            get;
            set;
        }

        public string HomeDirectory
        {
            get
            {
                return home_;
            }
        }
        
        public List<string> DirectoryHistory
        {
            get
            {
                return dirHistory_;
            }
        }

        private readonly string home_;
        private List<string> dirHistory_;
    }
}
