using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class BackslashReader
    {
        public BackslashReader() { }

        public bool ReadBackslash(string str, int index)
        {
            return (str[index] == '\\' && index + 1 < str.Length) ;
        } 

    }
}
