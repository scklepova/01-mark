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


        public ReaderOutcome(int readSymbolCount, string stringToWrite)
        {
            this.ReadSymbolsCount = readSymbolCount;
            this.StringToWrite = stringToWrite;
        }

        public ReaderOutcome()
        {
            this.ReadSymbolsCount = 0;
            this.StringToWrite = "";
        }

        public bool isEmptyOutcome(ReaderOutcome outcome)
        {
            return (outcome.ReadSymbolsCount == 0 && outcome.StringToWrite == "");
        }
    }

    
}
