using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace tdd
{
    class Processor_test
    {
        [TestFixture]
        public class Markup_Language_Processor_should
        {

            private static void CheckRewrite(string inputText, string expectedHtml)
            {
                var processor = new Processor();

                var resultHtml = processor.RewriteStringToHtml(inputText);
              
                CompareHtmlCodes(expectedHtml, resultHtml);
            }

            static void CompareHtmlCodes(string codeA, string codeB)
            {
                codeA = Regex.Replace(codeA, "[\\n\\t\\s\\r]", String.Empty);
                codeB = Regex.Replace(codeB, "[\\n\\t\\s\\r]", String.Empty);

                Assert.AreEqual(codeA, codeB);
            }

            void CheckRewriteFromTextFile(string inputFilename, string expectedHtmlFilename)
            {
                string inputText = File.ReadAllText(inputFilename);
                string expectedHtml = File.ReadAllText(expectedHtmlFilename);

                var processor = new Processor();
                var resultHtml = processor.RewriteStringToHtml(inputText);
              
                CompareHtmlCodes(expectedHtml, resultHtml);
            }


            [Test]
            public void place_text_between_single_underscores_in_em_tag()
            {
                CheckRewrite(" _H_ ", "<em>H</em>");
            }

            [Test]
            public void place_text_between_double_underscores_in_strong_tag()
            {
                CheckRewrite(" __bold__ ", "<strong>bold</strong>");
            }

            [Test]
            public void place_text_between_backtics_in_code_tags()
            {
                CheckRewrite("`code`", "<code>code</code>");
            }

            [Test]
            public void not_mark_tags_between_code_tag()
            {
                CheckRewrite("`var _i_ = 0`", "<code>var _i_ = 0</code>");
            }

            [Test]
            public void not_mark_unpair_tags()
            {
                CheckRewrite("__a _b`c", "__a _b`c");
            }

            [Test]
            public void not_mark_underscores_between_digits_and_letters()
            {
                CheckRewrite("abc_def__fgh", "abc_def__fgh");
            }

            [Test]
            public void shield_symbols_after_backslash()
            {
                CheckRewriteFromTextFile("../../tests/backslashes.txt", "../../tests/rewritedBackslashes.txt");
            }

            [Test]
            public void place_text_after_two_lines_in_paragraph()
            {
                CheckRewriteFromTextFile("../../tests/backslashes.txt", "../../tests/rewritedBackslashes.txt");
            }

            [Test]
            public void parse_inserted_tags()
            {
                CheckRewrite(" __a _b_ c__ ", "<strong>a<em>b</em>c</strong>");
                CheckRewrite(" _a __b__ c_ ", "<em>a<strong>b</strong>c</em>");
                CheckRewriteFromTextFile("../../tests/inserted_tags.txt", "../../tests/rewrited_inserted_tags.txt");
            }

            [Test]
            public void parse_complex_text()
            {
                CheckRewriteFromTextFile("../../tests/complexTest.txt", "../../tests/rewritedComplexTest.txt");
            }
        }
    }
}
