using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace tdd
{
    class TagReader : ElementReader
    {
        protected Tag TagToRead;

        public TagReader(Tag tagToRead)
        {
            this.TagToRead = tagToRead;
        }



        public virtual bool ReadOpeningTag(string str, int index)
        {
            if (index + TagToRead.TextTag.Length + 1 >= str.Length)
                return false;
      
            return (ReadTag(str, index) &&
               str.Substring(index + TagToRead.TextTag.Length + 1).Contains(TagToRead.TextTag + " "));
        }



        public virtual bool ReadClosingTag(string str, int index)
        {
            if (index + TagToRead.TextTag.Length + 1 > str.Length)
                return false;

            return ReadTag(str, index);
        }

        public bool ReadTag(string str, int index)
        {
            return (Regex.IsMatch(str.Substring(index), "^\\s" + TagToRead.TextTag) ||
                    Regex.IsMatch(str.Substring(index), "^" + TagToRead.TextTag + "\\s"));
        }



        public ReaderOutcome ReadElement(string str, int index, string lastReadTag)
        {
            if (lastReadTag == TagToRead.HtmlTag && ReadClosingTag(str, index))
                return new ReaderOutcome(TagToRead.TextTag.Length + 1, "</" + TagToRead.HtmlTag + ">", TagToRead.HtmlTag);


            if (ReadOpeningTag(str, index))
                return new ReaderOutcome(TagToRead.TextTag.Length + 1, "<" + TagToRead.HtmlTag + ">", TagToRead.HtmlTag);

            return new ReaderOutcome();
        }
    }
}
