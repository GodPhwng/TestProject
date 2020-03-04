using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

            // タグ一覧取得
            var tagList = await wpService.GetTagList();

            foreach (var qiita in qiitaList)
            {                
                var url = qiita["url"].ToString();
                var title = qiita["title"].ToString();                                

                // リクエストBody作成
                var json = new
                {
                    // 作成日時
                    date = DateTime.Parse(qiita["created_at"].ToString()).ToString("s"),

                    // 公開範囲
                    status = "publish",

                    // タイトル
                    title = title,

                    // 本文
                    content = $"\n<p>{title}<a href=\"{url}\">{url}</a></p>\n",

                    // タグ(インサート処理は非同期で実行し、すべての処理が終わるのを待つ)
                    tags = await Task.WhenAll(qiita["tags"].Select(async q => await this.GetAndAddTagListAsync(wpService, q, tagList)))
                };

                // Qiitaの記事URLが含まれる物を取得
                var matchArticle = wpList.FirstOrDefault(w => w["content"]["rendered"].ToString().Contains(url));

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

        /// <summary>
        /// Qiitaタグと同一のWordPressタグIDを取得
        /// WordPress側に存在しない場合は、新規追加を行う
        /// </summary>
        /// <param name="wpService">WPService</param>
        /// <param name="tag">Qiitaタグ</param>
        /// <param name="wpTagList">WordPressタグ一覧</param>
        /// <returns>Qiitaタグと同一のWordPressタグID</returns>
        private async Task<int> GetAndAddTagListAsync(WPService wpService, JToken tag, JArray wpTagList)
        {
            var findTag = wpTagList.FirstOrDefault(wpt => wpt["name"].ToString() == tag["name"].ToString());
            if (findTag == null)
            {
                // Tag追加リクエスト(同期リクエスト)
                var id = await wpService.InsertTag(new { name = tag["name"].ToString() });

                // リストに追加
                wpTagList.Add(JToken.FromObject(new { id = id ?? -1, name = tag["name"].ToString() }));

                return id ?? -1;
            }
            else
            {
                return findTag["id"].Value<int>();
            }
        }
    }
}
