using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class AnySymbolReader : ElementReader
    {
        public AnySymbolReader() { }


        public ReaderOutcome ReadElement(string str, int index)
        {
            return new ReaderOutcome(1, str[index] + "", "");
        }
    }
}
