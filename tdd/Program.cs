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

        static Stack<string> TextBetweenTags;
        static Stack<string> Tags;
        static Stack<string> TagsInInput;
        static string buffer;
        static int index;

        

        public static string Rewrite(string inputText)
        {
            TextBetweenTags = new Stack<string>();
            Tags = new Stack<string>();
            TagsInInput = new Stack<string>();
            index = 0;
            buffer = "";

            while (index < inputText.Length)
            {
                KeyValuePair<string, string> tag = BeginsWithTag(inputText);
                string TagString = tag.Key;
                string TagName = tag.Value;
                if (TagName.Length > 0)
                {
                    PushTagInStack(TagString, TagName);
                }
                else
                {
                    buffer += inputText[index];
                    index++;
                }
            }


            string htmlCode = buffer;

            while (TagsInInput.Any())
            {
                htmlCode = TagsInInput.Peek() + htmlCode + TextBetweenTags.Peek();
                TagsInInput.Pop();
                TextBetweenTags.Pop();
            }

            return htmlCode;
        }




        public static KeyValuePair<string, string> BeginsWithTag(string str)
        {
            return BeginWithSingleUnderscore(str);
        }

        public static KeyValuePair<string, string> BeginWithSingleUnderscore(string str)
        {

            if (index > 0 && IsDigitOrLetter(str[index - 1]) && index < str.Length - 1 && IsDigitOrLetter(str[index + 1]))
                return EmptyPair();

            if (str[index] != '_')
                return EmptyPair();

            string tagString = "_";
            string tagName = "em";
            return new KeyValuePair<string, string>(tagString, tagName);
        }




        public static bool SpaceSymbol(char c)
        {
            return (c == '\n' || c == '\t' || c == '\r' || c == ' ');
        }




        public static KeyValuePair<string, string> EmptyPair()
        {
            return new KeyValuePair<string, string>("", "");
        }


        public static void PushTagInStack(string tagString, string tagName)
        {
           

            if (Tags.Any() && Tags.Peek() == tagName)
            {
                buffer = "<" + tagName + ">" + buffer + "</" + tagName + ">";
                Tags.Pop();
                TagsInInput.Pop();
            }
            else
            {
                TextBetweenTags.Push(buffer);
                buffer = "";
                TagsInInput.Push(tagString);
                Tags.Push(tagName);
            }
            index += tagString.Length;
        }

        public static bool IsDigitOrLetter(char c)
        {
            return (c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z');
        }

        public static bool IsUnderscoreBetweenDigitsOrLetters(string str, int index)
        {
            if (index == 0 || index == str.Length - 1)
                return false;
            return (str[index] == '_' && IsDigitOrLetter(str[index - 1]) && IsDigitOrLetter(str[index + 1]));

        }

        static void Main(string[] args)
        {
        }
    }

    [TestFixture]
    public class MarkupLanguageProcessor_should
    {
        [Test]
        public void place_text_between_single_underscores_in_em_tag()
        {
            var result = Processor.Rewrite("_Hello_");
            Assert.AreEqual("<em>Hello</em>", result);
        }

        [Test]
        public void miss_underscores_between_text_and_digits()
        {
            var result = Processor.Rewrite("abc_def");
            Assert.AreEqual("abc_def", result);
        }

        [Test]
        public void place_text_between_double_underscores_in_strong_tag()
        {
            var result = Processor.Rewrite("__bold__");
            Assert.AreEqual("<strong>bold</strong>", result);
        }
    }
     

}
