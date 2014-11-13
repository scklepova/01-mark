using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please, enter name of file for rewriting");
                return;
            }

            string inputText = "";
            string inputPath = @args[0];
            try
            {
                inputText = ReadTextFromFile(inputPath);
            }
            catch (Exception ex)
            {
                return;
            }


            var processor = new Processor();
            string rewrited = processor.RewriteStringToHtml(inputText);

            string resultFile = WriteRewritedTextIntoNewFile(Path.GetFileNameWithoutExtension(inputPath), rewrited);
            Console.WriteLine("File was parsed. See in " + resultFile);


        }

        static string ReadTextFromFile(string filename)
        {
            string inputText = "";
            try
            {
                inputText = File.ReadAllText(filename);
                return inputText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }



        static string WriteRewritedTextIntoNewFile(string filename, string toWrite)
        {
            string path = @"../../" + filename + "_rewrited.html";

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (FileStream fileStream = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(toWrite);
                    fileStream.Write(info, 0, info.Length);
                }

                return path;
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return "";
            }
            
        }


    }
}
