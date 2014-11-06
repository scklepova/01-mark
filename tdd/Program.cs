using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace tdd
{
    class Processor
    {
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

        public static string Rewrite(string inputText)
        {
            var i = 0;
            bool underscoreRead = false;
            string result = "";
            while (i < inputText.Length)
            {
                if (inputText[i] == '_' && !IsUnderscoreBetweenDigitsOrLetters(inputText, i))
                {
                    if (underscoreRead)
                        result += "</em>";
                    else
                        result += "<em>";

                    underscoreRead = !underscoreRead;
                    

                }
                else
                {
                    result += inputText[i];
                    
                }
                i++;

            }

            return result;
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
    }
     

}
