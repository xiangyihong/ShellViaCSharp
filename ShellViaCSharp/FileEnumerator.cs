using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShellViaCSharp
{
    class FileEnumeratorArgs
    {

        public FileEnumeratorArgs()
        {
            ShowFile = true;
            ShowDir = true;
            Recursive = false;
            Deep = 0;
            Filter = null;
        }

        public bool ShowFile
        {
            get;
            set;
        }
        public bool ShowDir
        {
            get;
            set;
        }
        public bool Recursive
        {
            get;
            set;
        }
        public int Deep
        {
            get;
            set;
        }
        public string Filter
        {
            get;
            set;
        }
    }
    class FileEnumerator: IEnumerable<string>
    {
        FileEnumeratorArgs args_;
        string rootDirectory_;

        public FileEnumerator(string root, FileEnumeratorArgs args)
        {
            rootDirectory_ = root;
            args_ = args;
            if(args_.Deep < 0)
            {
                args_.Deep = 1;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return DirectoryWalker(rootDirectory_, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<string> DirectoryWalker(string directory, int layer)
        {
            if(args_.Deep > 0 && layer >= args_.Deep)
            {
                yield break;
            }

            IEnumerable<string> dir;
            try
            {
                if (args_.Filter == null)
                {
                    dir = Directory.EnumerateFiles(directory);
                }
                else
                {
                    dir = Directory.EnumerateFiles(directory, args_.Filter);
                }
            }
            catch(Exception)
            {
                yield break ;
            }
            
            //show subdirectory names first
            if(args_.ShowDir)
            {
                foreach(string name in Directory.EnumerateDirectories(directory))
                {
                    yield return name;
                }
            }

            foreach(string filename in dir)
            {
                yield return filename;
            }


            if(args_.Recursive && ((args_.Deep == 0) ||((layer+1) <= args_.Deep )))
            {
                foreach(string dirname in Directory.EnumerateDirectories(directory))
                {
                    IEnumerator<string> subdir = DirectoryWalker(dirname, layer + 1);
                    while (subdir.MoveNext())
                    {
                        yield return subdir.Current;
                    }
                }
            }
        }
    }
}
