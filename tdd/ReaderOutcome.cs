using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class ReaderOutcome
    {
        public int ReadSymbolsCount ;
        public string StringToWrite ;
        public string TagName ;


        public ReaderOutcome(int readSymbolCount, string stringToWrite, string tagName)
        {
            this.ReadSymbolsCount = readSymbolCount;
            this.StringToWrite = stringToWrite;
            this.TagName = tagName;
        }

        public ReaderOutcome()
        {
            this.ReadSymbolsCount = 0;
            this.StringToWrite = "";
            this.TagName = "";
        }

        public bool Empty()
        {
            return (this.ReadSymbolsCount == 0 && this.StringToWrite == "");
        }
    }

    
}
