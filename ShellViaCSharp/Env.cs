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
        


        private readonly string home_;
        
    }
}
