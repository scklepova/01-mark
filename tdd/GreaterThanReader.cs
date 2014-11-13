using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class GreaterThanReader : ElementReader
    {
        public GreaterThanReader()
            : base() { }


        public ReaderOutcome ReadElement(string str, int index, string lastReadTag)
        {
            if (str[index] == '>')
                return new ReaderOutcome(1, "&gt;", "");
            return new ReaderOutcome();
        }
    }
}
