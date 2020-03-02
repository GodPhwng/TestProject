using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.QiitaToWP
{
    /// <summary>
    /// QiitaToWordPressクラス
    /// </summary>
    public class Qiita2WP
    {
        /// <summary>
        /// QiitaからWordPressに記事を転送
        /// </summary>
        /// <returns>Task</returns>
        public async Task Qiita2WPArticle()
        {
            // Qiita記事取得
            var qiitaService = new QiitaService();
            var qiitaList = await qiitaService.GetArticleList("GodPhwng");
            
            // WP記事取得
            var wpService = new WPService("ユーザID:アプリケーションパスワード");
            var wpList = await wpService.GetArticleList();

            foreach (var qiita in qiitaList)
            {
                var url = qiita["url"].ToString();
                var title = qiita["title"].ToString();

                // Qiitaの記事URLが含まれる物を取得
                var matchArticle = wpList.FirstOrDefault(w => w["content"]["rendered"].ToString().Contains(url));

                // リクエストBody作成
                var json = new
                {
                    // 公開範囲
                    status = "publish",

                    // タイトル
                    title = title,

                    // 本文
                    content = $"\n<p>{title}<a href=\"{url}\">{url}</a></p>\n"
                };
                
                if (matchArticle != null)
                {
                    // 更新
                    await wpService.UpdateWPArticle(matchArticle["id"].ToString(), json);               
                }                
                else
                {
                    // 新規追加
                    await wpService.InsertWPArticle(json);
                }
            }
        }
    }
}
