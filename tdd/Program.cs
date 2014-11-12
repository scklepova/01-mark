﻿using System;
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
        private readonly List<ElementReader> Readers;


        public Processor()
        {
            var tagsList = new List<Tag> { new Tag("strong", "__"), new Tag("em", "_") };
            this.Readers = new List<ElementReader> { new BackslashReader(), new BackticksReader() };
            foreach (var tag in tagsList)
            {
                this.Readers.Add(new TagReader(tag));
            }
            this.Readers.Add(new AnySymbolReader());
        }


        public string RewriteFileToHtml(string inputFile)
        {
            var paragraphs = SplitFileIntoParagraphs(inputFile);
            var htmlParagraphs = paragraphs.Select(RewriteParagraph).ToList();

            return JoinParagraphsToString(htmlParagraphs);
        }


        public string RewriteStringToHtml(string inputText) //use only for tests
        {
            var paragraphs = Regex.Split(inputText, Environment.NewLine + "\\s*" + Environment.NewLine);
            var htmlParagraphs = new List<string>();
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
            string buffer = "";
            int index = 0;
            Stack<string> tagStack = new Stack<string>();

            while (index < paragraph.Length)
            {
                bool tagRead = false;
                foreach (var reader in Readers)
                {
                    ReaderOutcome outcome = reader.ReadElement(paragraph, index);
                    if (outcome.Empty())
                        continue;

                    buffer += outcome.StringToWrite;
                    index += outcome.ReadSymbolsCount;
                    if (outcome.TagName != "")
                        PushTagInStack(outcome.TagName, tagStack);
                    break;

                }

            }

            return buffer;
        }


        void PushTagInStack(string tagName, Stack<string> tagStack)
        {
            if (tagStack.Any() && tagStack.Peek() == tagName)
                tagStack.Pop();
            else
                tagStack.Push(tagName);
        }



        static void Main(string[] args)
        {

        }
    }


}
