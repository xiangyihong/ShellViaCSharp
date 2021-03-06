﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellViaCSharp
{
    public class OptionAndValue
    {
        public string Option
        {
            get
            {
                return option_;
            }
        }

        public string Value
        {
            get
            {
                return value_;
            }
        }

        public OptionAndValue(string o, string v)
        {
            option_ = o;
            value_ = v;
        }

        private readonly string option_;
        private readonly string value_;
    }

    class OptionEnumerator: IEnumerable<OptionAndValue>
    {
        public OptionEnumerator(string args)
        {
            args_ = args;
        }

        public IEnumerator<OptionAndValue> GetEnumerator()
        {
            return ParseArgs();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        //ugly implementation
        private IEnumerator<OptionAndValue> ParseArgs()
        {
            int n = args_.Length;
            int index = 0;

            string option;
            string value;

            while(index < n)
            {
                //get option
                string word;
                int nextIndex = ParseNextWord(args_, index, out word);

                if(word == string.Empty)
                {
                    yield break;
                }

                //no option at all
                if(word[0] != '-')
                {
                    yield return new OptionAndValue(null, word);
                    index = nextIndex;
                    continue;
                }

                //option


                //for situations like "-a" "-" "--"
                if(word.Length <= 2)
                {
                    int optionIndex = 0;
                    
                    if(word.Length > 1 && word[1] != '-')
                    {
                        optionIndex = 1;
                    }

                    option = word.Substring(optionIndex, word.Length - optionIndex);

                    nextIndex = TryGetValue(args_, nextIndex, out value);
                    yield return new OptionAndValue(option, value);
                    index = nextIndex;
                    continue;
                }

                //case for word.Length > 2

                //long option style
                if(word[1] == '-')
                {
                    if(word[2] == '-')
                    {
                        yield break;
                    }

                    option = word.Substring(2);

                    nextIndex = TryGetValue(args_, nextIndex, out value);
                    yield return new OptionAndValue(option, value);
                    index = nextIndex;
                    continue;
                }

                //short option style
                //deal with types like "-abcd"
                int wordIndex = 1;
                while(wordIndex < word.Length-1)
                {
                    yield return new  OptionAndValue(word.Substring(wordIndex, 1), null);
                    ++wordIndex;
                }

                option = word.Substring(wordIndex, 1);
                nextIndex = TryGetValue(args_, nextIndex, out value);
                yield return new OptionAndValue(option, value);

                index = nextIndex;
            }
        }

        private int TryGetValue(string s, int start, out string value)
        {
            int boundry = start;
            string tryValue = string.Empty;
            boundry = ParseNextWord(args_, start, out tryValue);
            if (tryValue == string.Empty || tryValue[0] == '-')
            {
                value = null;
                boundry = start;
            }
            else
            {
                value = tryValue;
            }

            return boundry;
        }

        private int ParseNextWord(string s, int start, out string word)
        {
            int n = s.Length;

            //parse preceding white space
            while(start < n && char.IsWhiteSpace(s[start]))
            {
                ++start;
            }
            //get real word
            int boundry = start;
            while (boundry < n && !char.IsWhiteSpace(s[boundry]))
            {
                ++boundry;
            }
            word = s.Substring(start, boundry - start);
            return boundry;
        }
        private string args_;
    }
}
