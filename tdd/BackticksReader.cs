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


        public ReaderOutcome ReadElement(string str, int index, string lastReadTag)
        {
            if (str[index] == '`' && index < str.Length - 1 && str.Substring(index + 1).Contains("`"))
            {
                int count = 0;
                string buffer = "";
                while (str[index + count + 1] != '`')
                {
                    if (str[index + count + 1] == '<')
                        buffer += "&lt;";
                    else if (str[index + count + 1] == '>')
                        buffer += "&gt;";
                    else
                        buffer += str[index + count + 1];

                    count++;
                }

                string toWrite = "<code>" + buffer + "</code>";
                return new ReaderOutcome(count + 2, toWrite, "code");
            }

            return  new ReaderOutcome();
        }
    }
}
