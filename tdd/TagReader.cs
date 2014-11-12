using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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

            return (str.Substring(index, TagToRead.TextTag.Length + 1) == " " + TagToRead.TextTag &&
               str.Substring(index + TagToRead.TextTag.Length + 1).Contains(TagToRead.TextTag + " "));
        }



        public virtual bool ReadClosingTag(string str, int index)
        {
            if (index + TagToRead.TextTag.Length + 1 > str.Length)
                return false;

            return (str.Substring(index, TagToRead.TextTag.Length + 1) == TagToRead.TextTag + " ");
        }



        public ReaderOutcome ReadElement(string str, int index)
        {
            if (ReadOpeningTag(str, index))
                return new ReaderOutcome(TagToRead.TextTag.Length + 1, "<" + TagToRead.HtmlTag + ">", TagToRead.HtmlTag);

            if (ReadClosingTag(str, index))
                return new ReaderOutcome(TagToRead.TextTag.Length + 1, "</" + TagToRead.HtmlTag + ">", TagToRead.HtmlTag);

            return new ReaderOutcome();
        }
    }
}
