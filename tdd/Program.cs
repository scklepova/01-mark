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

        public static string Rewrite(string inputText)
        {
            var i = 0;
            bool underscoreRead = false;
            string result = "";
            while (i < inputText.Length)
            {
                if (inputText[i] == '_')
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

        
    }
     

}
