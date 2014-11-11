using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace tdd
{
    class Processor
    {

        Stack<string> TextBetweenTags;
        Stack<Tag> TagsStack;
        string buffer;
        int index;
        private bool InCodeTag;
        private List<Tag> TagsList;

        public Processor()
        {
            this.TextBetweenTags = new Stack<string>();
            this.TagsStack = new Stack<Tag>();
            this.index = 0;
            this.buffer = "";
            this.InCodeTag = false;
            this.TagsList = new List<Tag>();
            this.TagsList.Add(new Tag("strong", "__"));
            this.TagsList.Add(new Tag("em", "_"));
            this.TagsList.Add(new Tag("code", "`"));
        }

        public string Rewrite(string inputText)
        {
            while (index < inputText.Length)
            {
                bool tagRead = false;
                foreach (var tag in TagsList)
                {
                    TagReader reader = new TagReader(tag);
                    if (TagsStack.Any() && TagsStack.Peek().Equals(tag))
                    {
                        tagRead = reader.ReadClosingTag(inputText, index);
                        if (tagRead)
                        {
                            PushTagInStack(tag);
                            break;
                        }
                    }
                        
                    tagRead = reader.ReadOpeningTag(inputText, index);
                    if (tagRead)
                    {
                        if (tag.HtmlTag == "code")
                        {
                            index += 2;
                            buffer += "<code>";
                            while (!reader.ReadClosingTag(inputText, index))
                            {
                                buffer += inputText[index];
                                index++;
                            }

                            buffer += "</code>";
                            index++;

                        }
                        PushTagInStack(tag);
                        break;
                    }                                     
                }

                if (!tagRead)
                {
                    buffer += inputText[index];
                    index++;
                }
               
            }

            string htmlCode = buffer;

            while (TextBetweenTags.Any())
            {
                htmlCode = TextBetweenTags.Peek() + htmlCode;
                TextBetweenTags.Pop();
            }

            return htmlCode;
        }

    

        void PushTagInStack(Tag tag)
        {
            if (TagsStack.Any() && TagsStack.Peek().Equals(tag))
            {
                buffer = "<" + tag.HtmlTag + ">" + buffer + "</" + tag.HtmlTag + ">";
                TagsStack.Pop();
            }
            else
            {
                TextBetweenTags.Push(buffer);
                buffer = "";
                TagsStack.Push(tag);
            }
            index += tag.TextTag.Length + 1;
        }

   

        static void Main(string[] args)
        {
        }
    }

    [TestFixture]
    public class MarkupLanguageProcessor_should
    {

        private static void CheckRewrite(string inputText, string expectedHtml)
        {
            Processor processor = new Processor();

            var result = processor.Rewrite(inputText);
            Assert.AreEqual(expectedHtml, result);
        }


        [Test]
        public void place_text_between_single_underscores_in_em_tag()
        {
            CheckRewrite(" _Hello_ ", "<em>Hello</em>");
        }

        [Test]
        public void place_text_between_double_underscores_in_strong_tag()
        {
            CheckRewrite(" __bold__ ", "<strong>bold</strong>");
        }

        [Test]
        public void place_text_between_backtics_in_code_tags()
        {
            CheckRewrite(" `code` ", "<code>code</code>");
        }

        [Test]
        public void not_mark_tags_between_code_tag()
        {
            CheckRewrite(" `var _i_ = 0` ", "<code>var _i_ = 0</code>");
        }

        [Test]
        public void not_mark_unpair_tags()
        {
            CheckRewrite("__a _b`c", "__a _b`c");
        }
    }
}
