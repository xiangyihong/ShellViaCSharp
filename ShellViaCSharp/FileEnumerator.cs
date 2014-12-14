using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShellViaCSharp
{
    class FileEnumerator: IEnumerable<string>
    {
        private string rootDirectory_;
        private bool recusive_;
        private int deep_;
        private string filter_;

        public FileEnumerator(string root, bool recusive, int deep, string filter = null)
        {
            rootDirectory_ = root;
            recusive_ = recusive;

            //deep == 0 recusive till the end
            deep_ = (deep < 0) ? 1 : deep;

            filter_ = filter;
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
            if(deep_ > 0 && layer >= deep_)
            {
                yield break;
            }

            IEnumerable<string> dir;
            try
            {
                if (filter_ == null)
                {
                    dir = Directory.EnumerateFiles(directory);
                }
                else
                {
                    dir = Directory.EnumerateFiles(directory, filter_);
                }
            }
            catch(Exception)
            {
                yield break ;
            }
            
            foreach(string filename in dir)
            {
                yield return filename;
            }

            if(recusive_ && ((deep_ == 0) ||((layer+1) <= deep_)))
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
