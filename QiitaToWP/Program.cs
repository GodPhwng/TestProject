using System;
using System.Threading.Tasks;

namespace TestProject.QiitaToWP
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
        /// <returns>Task</returns>
        public static async Task Main(string[] args)
        {
            try
            {
                var q2wp = new Qiita2WP();

                // Qiitaの記事をWordPressに反映
                await q2wp.Qiita2WPArticle();

                Console.WriteLine("処理終了");
            }
            catch (Exception err)
            {
                Console.WriteLine("エラー終了");
                Console.WriteLine(err.Message);
            }
            finally
            {
                Console.Read();
            }
        }
    }
}
