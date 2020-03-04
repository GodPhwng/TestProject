using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.QiitaToWP
{
    /// <summary>
    /// WordPress API呼び出しクラス
    /// </summary>
    public class WPService : ServiceBase
    {
        /// <summary>
        /// TOPURL
        /// </summary>
        private const string TOP_URL = "http://kurosu.s1009.xrea.com";

        /// <summary>
        /// 記事取得、投稿時の定義
        /// </summary>
        private const string POSTS = "/wp-json/wp/v2/posts";

        /// <summary>
        /// 記事更新時の定義
        /// </summary>
        private const string UPDATE_POSTS = "/wp-json/wp/v2/posts/{0}";

        /// <summary>
        /// タグ取得、投稿時の定義
        /// </summary>
        private const string TAGS = "/wp-json/wp/v2/tags";

        /// <summary>
        /// アプリケーションキー(Basic認証に使用するキー)
        /// </summary>
        private readonly string apllicationKey;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="applicationKey">アプリケーションキー(Basic認証に使用するキー)</param>
        public WPService(string applicationKey) : base()
        {
            this.apllicationKey = applicationKey;
        }

        /// <summary>
        /// 記事一覧取得
        /// </summary>
        /// <param name="param">パラメータ群</param>
        /// <returns>返り値JSON配列</returns>
        public override async Task<JArray> GetArticleList(params string[] param)
        {
            var body = await HpClient.GetStringAsync(TOP_URL + POSTS);
            return JArray.Parse(body);
        }

        /// <summary>
        /// タグ一覧取得
        /// </summary>
        /// <param name="page">ページIndex</param>
        /// <returns>返り値JSON配列</returns>
        public async Task<JArray> GetTagList(int page = 1)
        {
            // 1ページ単位、100件取得
            var responce = await HpClient.GetAsync($"{TOP_URL}{TAGS}?page={page}&per_page=100");

            // 100件区切りした場合のトータルのページ
            // 例えば201件ある場合、3ページとなる
            var toalPage = responce.Headers.GetValues("X-WP-TotalPages").First();

            var body = await responce.Content.ReadAsStringAsync();
            var tagArray = JArray.Parse(body);

            if (Convert.ToInt32(toalPage) > page)
            {
                // 再起処理 すべてのページのタグを収集
                tagArray.Merge(await this.GetTagList(++page));
            }

            return tagArray;
        }

        /// <summary>
        /// 記事更新
        /// </summary>
        /// <param name="id">記事ID</param>
        /// <param name="json">更新情報JSON</param>
        /// <returns>処理の成否</returns>
        public async Task<bool> UpdateWPArticle(string id, object json)
        {
            var request = this.CreateHttpRequestMessage(HttpMethod.Post, TOP_URL + string.Format(UPDATE_POSTS, id));
            request.Content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json"); ;

            // リクエスト
            var result = await HpClient.SendAsync(request);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                // エラーメッセージ
                var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());
                Console.WriteLine(resultJson["message"].ToString());

                return false;
            }
        }

        /// <summary>
        /// 記事追加
        /// </summary>
        /// <param name="json">記事情報JSON</param>
        /// <returns>処理の成否</returns>
        public async Task<bool> InsertWPArticle(object json)
        {
            var request = this.CreateHttpRequestMessage(HttpMethod.Post, TOP_URL + POSTS);
            request.Content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            // リクエスト
            var result = await HpClient.SendAsync(request);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                // エラーメッセージ
                var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());
                Console.WriteLine(resultJson["message"].ToString());

                return false;
            }
        }

        /// <summary>
        /// タグ追加
        /// </summary>
        /// <param name="json">タグ情報JSON</param>
        /// <returns>作成したタグのID</returns>
        public async Task<int?> InsertTag(object json)
        {
            var request = this.CreateHttpRequestMessage(HttpMethod.Post, TOP_URL + TAGS);
            request.Content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            // リクエスト
            var result = await HpClient.SendAsync(request);

            // レスポンスbody
            var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());

            if (result.StatusCode == HttpStatusCode.Created)
            {
                // 追加したタグのIDを返却
                return resultJson["id"].Value<int>();
            }
            else
            {
                // エラーメッセージ
                Console.WriteLine(resultJson["message"].ToString());

                return null;
            }
        }

        /// <summary>
        /// HttpRequestMessage作成
        /// </summary>
        /// <param name="method">Httpメソッド</param>
        /// <param name="url">URL</param>
        /// <returns>HttpRequestMessage</returns>
        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);

            // Basi認証ヘッダー
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.apllicationKey));
            request.Headers.Add("Authorization", $"Basic {credentials}");

            return request;
        }
    }
}
