using System.IO;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleAPIの認証処理を行う静的クラス
    /// </summary>
    public class GoogleAuthAgent
    {
        /// <summary>
        /// このファイルからクレデンシャルファイルまでの相対パス
        /// </summary>
        private const string RELATIVE_PATH_TO_CREADENTIAL = "../Credentials/credentials.json";

        /// <summary>
        /// 認証処理を行い、スプレッドシートのサービスを提供するオブジェクトを作成して返す
        /// </summary>
        /// <returns>
        /// スプレッドシートにアクセスするサービスを提供してくれるオブジェクト
        /// アクセス権は読み取り専用
        /// </returns>
        static public SheetsService CreateSheetsService()
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
        static private GoogleCredential CreateGoogleCredential(
            params string[] scopes
        )
        {
            // クレデンシャルファイルのパスを計算
            var thisDirectoryPath = SourceCodeLocator
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