using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class BackslashReader : ElementReader
    {
        public BackslashReader() { }

        public ReaderOutcome ReadElement(string str, int index, string lastReadTag)
        {
            if (str[index] == '\\' && index + 1 < str.Length) 
                return new ReaderOutcome(2, str[index + 1] + "", "");

            return  new ReaderOutcome();
        } 

    }
}
