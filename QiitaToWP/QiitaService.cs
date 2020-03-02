using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TestProject.QiitaToWP
{
    /// <summary>
    /// QiitaAPI呼び出しクラス
    /// </summary>
    public class QiitaService : ServiceBase
    {
        /// <summary>
        /// TOPURL
        /// </summary>
        private const string TOP_URL = "https://qiita.com";

        /// <summary>
        /// 記事一覧取得定義
        /// </summary>
        private const string GET_ARTICLELIST = "/api/v2/users/{0}/items";

        /// <summary>
        /// 記事一覧取得
        /// </summary>
        /// <param name="param">パラメータ群</param>
        /// <returns>返り値JSON配列</returns>
        public override async Task<JArray> GetArticleList(params string[] param)
        {
            var body = await HpClient.GetStringAsync(TOP_URL + string.Format(GET_ARTICLELIST, param[0]));
            return JArray.Parse(body);
        }
    }
}
