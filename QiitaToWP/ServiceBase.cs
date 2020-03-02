using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestProject.QiitaToWP
{
    /// <summary>
    /// サービス基底クラス
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// HttpClient
        /// </summary>
        protected static readonly HttpClient HpClient;

        /// <summary>
        /// staticコンストラクタ
        /// </summary>
        static ServiceBase()
        {
            HpClient = new HttpClient();
        }

        /// <summary>
        /// 記事一覧取得
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>記事一覧</returns>
        public abstract Task<JArray> GetArticleList(params string[] param);        
    }
}
