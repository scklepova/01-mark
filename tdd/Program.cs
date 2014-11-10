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
        private static bool inCodeTag;

        

        public static string Rewrite(string inputText)
        {
            TextBetweenTags = new Stack<string>();
            Tags = new Stack<string>();
            TagsInInput = new Stack<string>();
            index = 0;
            buffer = "";
            inCodeTag = false;

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
                htmlCode = TextBetweenTags.Peek() + TagsInInput.Peek() + htmlCode;
                TagsInInput.Pop();
                TextBetweenTags.Pop();
            }

            return htmlCode;
        }




        public static KeyValuePair<string, string> BeginsWithTag(string str)
        {
            KeyValuePair<string, string> tag = BeginWithBacktick(str);
            if (IsEmptyPair(tag) && inCodeTag)
                return EmptyPair();
             
            if (IsEmptyPair(tag))
                tag = BeginWithDoubleUnderscore(str);

            if (IsEmptyPair(tag))
                tag = BeginWithSingleUnderscore(str);

            return tag;
        }


        public static bool IsEmptyPair(KeyValuePair<string, string> pair)
        {
            return (pair.Key == "" && pair.Value == "");
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

        public static bool IsUnderscoreBetweenDigitsOrLetters(string str)
        {
            if (index == 0 || index == str.Length - 1)
                return false;
            return (str[index] == '_' && IsDigitOrLetter(str[index - 1]) && IsDigitOrLetter(str[index + 1]));

        }



        public static KeyValuePair<string, string> BeginWithDoubleUnderscore(string str)
        {
            if (index >= str.Length - 1)
                return EmptyPair();

            if (index > 0 && IsDigitOrLetter(str[index - 1]) && index + 2 < str.Length && IsDigitOrLetter(str[index + 2]))
                return EmptyPair();

            if (str[index] != '_' || str[index + 1] != '_')
                return EmptyPair();

            string TagString = "__";
            string TagName = "strong";
            return new KeyValuePair<string, string>(TagString, TagName);
        }



        public static KeyValuePair<string, string> BeginWithBacktick(string str)
        {
            if (str[index] == '`' && (inCodeTag || !inCodeTag && str.Substring(index + 1).Contains('`')))
            {
                string TagString = "`";
                string TagName = "code";
                inCodeTag = !inCodeTag;
                return new KeyValuePair<string, string>(TagString, TagName);
            }
            return EmptyPair();
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

        [Test]
        public void place_text_between_backtics_in_code_tags()
        {
            var result = Processor.Rewrite("`code`");
            Assert.AreEqual("<code>code</code>", result);
        }

        [Test]
        public void not_mark_tags_between_code_tag()
        {
            var result = Processor.Rewrite("`var _i_ = 0`");
            Assert.AreEqual("<code>var _i_ = 0</code>", result);
        }


        [Test]
        public void not_mark_unpair_tags()
        {
            var result = Processor.Rewrite("__a _b`c");
            Assert.AreEqual("__a _b`c", result);
        }
       
    }
     

}
