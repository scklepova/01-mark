using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class BackticksReader : ElementReader
    {
        public BackticksReader()
            : base() { }


        public ReaderOutcome ReadElement(string str, int index)
        {
            if (str[index] == '`' && index < str.Length - 1 && str.Substring(index + 1).Contains("`"))
            {
                int count = 0;
                while (str[index + count + 1] != '`')
                    count++;

                string toWrite = "<code>" + str.Substring(index + 1, count) + "</code>";
                return new ReaderOutcome(count + 2, toWrite, "code");
            }

            return  new ReaderOutcome();
        }
    }
}
