using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class Tag
    {
        public string HtmlTag;
        public string TextTag;

        public Tag(string htmlTag, string textTag)
        {
            this.HtmlTag = htmlTag;
            this.TextTag = textTag;
        }

        public static Tag EmptyTag()
        {
            return new Tag("", "");
        }

        public bool Equals(Tag a, Tag b)
        {
            return (a.HtmlTag == b.HtmlTag && a.TextTag == b.TextTag);
        }
    }
}
