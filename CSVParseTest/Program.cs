using System;

namespace TestProject.CSVParseTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var pTest = new CSVParseTest();
                pTest.ReadCsv();

                var pTest2 = new CSVParseTest2();
                pTest2.ReadCsv();
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                Console.Read();
            }
        }
    }
}
