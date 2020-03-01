using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.CSVParseTest
{
    public class CSVParseTest2
    {
        public void ReadCsv()
        {
            using(var stream = new StreamReader(@".\test.csv", Encoding.UTF8))
            using (var parser = new CsvParser(stream, CultureInfo.InvariantCulture))
            {
                string[] line;
                while ((line = parser.Read()) != null)
                {
                    line.ToList().ForEach(v =>
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
