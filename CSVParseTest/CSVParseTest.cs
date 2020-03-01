using Microsoft.VisualBasic.FileIO;
using System;
using System.Linq;
using System.Text;

namespace TestProject.CSVParseTest
{
    public class CSVParseTest
    {
        public void ReadCsv()
        {
            using (var parser = new TextFieldParser(@".\test.csv", Encoding.UTF8))
            {
                // 区切り文字
                parser.Delimiters = new string[] { "," };

                // 囲み文字あり
                parser.HasFieldsEnclosedInQuotes = true;

                while (!parser.EndOfData)
                {
                    var values = parser.ReadFields();
                    values.ToList().ForEach(v =>
                    {
                        // わかりやすいようにパイプを入れる
                        Console.Write(v + "|");
                    });

                    // わかりやすいように改行
                    Console.WriteLine();
                }
            }
        }
    }
}
