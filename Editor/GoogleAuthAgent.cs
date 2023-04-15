using System.IO;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleAPIの認証処理を行うクラス
    /// </summary>
    public class GoogleAuthAgent
    {
        /// <summary>
        /// このファイルからクレデンシャルファイルまでの相対パス
        /// </summary>
        private const string RELATIVE_PATH_TO_CREADENTIAL = "../Credentials/credentials.json";

        /// <summary>
        /// クレデンシャルファイルのパスを計算するために使用する、
        /// このソースコードが格納されているパスを計算してくれるオブジェクト
        /// </summary>
        ISourceCodeLocator sourceCodeLocator;

        public GoogleAuthAgent(ISourceCodeLocator _sourceCodeLocator)
        {
            sourceCodeLocator = _sourceCodeLocator;
        }

        /// <summary>
        /// 認証処理を行い、スプレッドシートのサービスを提供するオブジェクトを作成して返す
        /// </summary>
        /// <returns>
        /// スプレッドシートにアクセスするサービスを提供してくれるオブジェクト
        /// アクセス権は読み取り専用
        /// </returns>
        public SheetsService CreateSheetsService()
        {
            // このサービスでは書き込み等は行わない予定なので、読み取り専用で認証を行う
            var googleCredential = CreateGoogleCredential(SheetsService.Scope.SpreadsheetsReadonly);

            var sheetsService = new SheetsService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleCredential
                }
            );
            return sheetsService;
        }

        /// <summary>
        /// GoogleAPIへの認証処理
        /// </summary>
        /// <param name="scopes">
        /// 必要なサービスの範囲。複数指定できる。
        /// </param>
        /// <returns>
        /// 指定されたスコープへのアクセス権を持つ認証オブジェクト。
        /// </returns>
        private GoogleCredential CreateGoogleCredential(
            params string[] scopes
        )
        {
            // クレデンシャルファイルのパスを計算
            var thisDirectoryPath = sourceCodeLocator
                                    .GetDirectoryOfSourceCodePath();
            var credentialPath = Path.Combine(thisDirectoryPath, RELATIVE_PATH_TO_CREADENTIAL);

            // 認証情報をクレデンシャルファイルから読み込む
            using var fileStream = new FileStream(
                credentialPath,
                FileMode.Open,
                FileAccess.Read
            );

            // 読み込んだ認証情報を元に認証を通す。
            var googleCredential = GoogleCredential
                                    .FromStream(fileStream)
                                    .CreateScoped(scopes);

            return googleCredential;
        }
    }
}