using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        
        public string RewriteFileToHtml(string inputFile)
        {
            string[] paragraphs = SplitFileIntoParagraphs(inputFile);
            List<string> htmlParagraphs = new List<string>();
            for (int i = 0; i < paragraphs.Length; i++)
            {
                string rewritedParagraph = RewriteParagraph(paragraphs[i]);
                htmlParagraphs.Add(rewritedParagraph);
            }
     
            return JoinParagraphsToString(htmlParagraphs);
        }

        public string RewriteStringToHtml(string inputText)
        {
            string[] paragraphs = Regex.Split(inputText, Environment.NewLine + "\\s*" + Environment.NewLine);
            List<string> htmlParagraphs = new List<string>();
            for (int i = 0; i < paragraphs.Length; i++)
            {
                string rewritedParagraph = RewriteParagraph(paragraphs[i]);
                htmlParagraphs.Add(rewritedParagraph);
            }

            return JoinParagraphsToString(htmlParagraphs); 
        }


        public string[] SplitFileIntoParagraphs(string filename)
        {
            string inputText = File.ReadAllText(filename);
            return Regex.Split(inputText, Environment.NewLine + "\\s*" + Environment.NewLine);
        }



        public string JoinParagraphsToString(List<string> paragraphs)
        {
            string resultHtml = paragraphs.First();
            paragraphs.RemoveAt(0);
            foreach (var paragraph in paragraphs)
            {
                resultHtml += "<p>" + paragraph + "</p>" + Environment.NewLine;
            }

            return resultHtml;
        }



        private string RewriteParagraph(string paragraph)
        {
            buffer = "";
            index = 0;
            TagsStack.Clear();
            TextBetweenTags.Clear();
            while (index < paragraph.Length)
            {
                BackslashReader backslashReader = new BackslashReader();
                if (backslashReader.ReadBackslash(paragraph, index))
                {
                    buffer += paragraph[index + 1];
                    index += 2;
                    continue;
                }

                bool tagRead = false;
                foreach (var tag in TagsList)
                {
                    TagReader reader = new TagReader(tag);
                    if (TagsStack.Any() && TagsStack.Peek().Equals(tag))
                    {
                        tagRead = reader.ReadClosingTag(paragraph, index);
                        if (tagRead)
                        {
                            PushTagInStack(tag);
                            break;
                        }
                    }

                    tagRead = reader.ReadOpeningTag(paragraph, index);
                    if (tagRead)
                    {
                        if (tag.HtmlTag == "code")
                        {
                            index += 2;
                            buffer += "<code>";
                            while (!reader.ReadClosingTag(paragraph, index))
                            {
                                buffer += paragraph[index];
                                index++;
                            }

                            buffer += "</code>";
                            index += 2;
                        }
                        else
                        {
                            PushTagInStack(tag);
                            buffer += "<" + tag.HtmlTag + ">";
                        }

                        break;
                    }
                }

                if (!tagRead)
                {
                    buffer += paragraph[index];
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
                buffer = buffer + "</" + tag.HtmlTag + ">";
                TagsStack.Pop();
            }
            else
            {

                TagsStack.Push(tag);
            }
            index += tag.TextTag.Length + 1;

        }

   

        static void Main(string[] args)
        {
            
        }
    }

    
}
