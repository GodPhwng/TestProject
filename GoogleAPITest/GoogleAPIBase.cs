using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TestProject.GoogleAPITest
{
    /// <summary>
    /// GoogleAPI利用においての規定クラス
    /// </summary>
    public abstract class GoogleAPIBase<T> where T : IClientService
    {
        /// <summary>
        /// クライアントサービスインターフェース
        /// </summary>
        protected T Serive { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyJsonPath">APIキーのJSONファイルのパス</param>
        /// <param name="scope">スコープ</param>
        public GoogleAPIBase(string keyJsonPath, string[] scope)
        {
            var jObject = JObject.Parse(File.ReadAllText(keyJsonPath));
            var serviceAccountEmail = jObject["client_email"].ToString();
            var privateKey = jObject["private_key"].ToString();

            var credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scope
            }.FromPrivateKey(privateKey));

            this.Serive = this.CreateService(credential);
        }

        /// <summary>
        /// サービス作成メソッド
        /// </summary>
        /// <param name="credential">認証情報</param>
        /// <returns>クライアントサービスインターフェース</returns>
        protected abstract T CreateService(ICredential credential);
    }
}
