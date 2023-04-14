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
        /// 認証処理を行い、スプレッドシートのサービスを提供するオブジェクトを作成して返す
        /// </summary>
        /// <returns>
        /// スプレッドシートにアクセスするサービスを提供してくれるオブジェクト
        /// </returns>
        static public SheetsService CreateSheetsService()
        {
            return null;
        }
    }
}