using System;
using TestProject.GoogleAPITest.Calendar;

namespace TestProject.GoogleAPITest
{
    /// <summary>
    /// メインクラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// メインエントリ
        /// </summary>
        /// <param name="args">実行時引数</param>
        public static void Main(string[] args)
        {
            try
            {
                // カレンダーID
                var calendarId = "カレンダーID";

                // Googleカレンダーテストクラスインスタンス化
                var calApi = new CalendarAPITest(@"C:\job\TestProject\GoogleAPITest\testproject-269217-813bf9be17a5.json");

                // イベント読み取り
                calApi.ReadEvents(calendarId);

                // イベント追加
                var evt = calApi.InsertEvent(calendarId);

                // イベント更新
                evt = calApi.UpdateEvent(calendarId, evt);

                // イベント削除
                calApi.DeleteEvent(calendarId, evt.Id);
            }
            catch (Exception err)
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
