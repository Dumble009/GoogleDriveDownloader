
namespace GoogleDriveDownloader
{
    /// <summary>
    /// コンフィグファイルや認証情報ファイルなどの、
    /// システムが必要とするファイルを格納するフォルダのパスを管理するクラスが備えるインタフェース
    /// </summary>
    public interface ISystemFileLocator
    {
        /// <summary>
        /// コンフィグファイルが格納されているフォルダのパスを返す。
        /// 相対パスか絶対パスかは保証されていないが、このフォルダにコンフィグファイルのファイル名を連結すると
        /// コンフィグファイルを参照できることは保証される。
        /// </summary>
        /// <returns>
        /// コンフィグファイルが格納されているフォルダのパス文字列
        /// </returns>
        string GetConfigFolderPath();

        /// <summary>
        /// 認証情報ファイルが格納されているフォルダのパスを返す。
        /// 相対パスか絶対パスかは保証されていないが、このフォルダに認証情報ファイルのファイル名を連結すると
        /// 認証情報ファイルを参照できることは保証される。
        /// </summary>
        /// <returns>
        /// 認証情報ファイルが格納されているフォルダのパス文字列
        /// </returns>
        string GetCredentialsFolderPath();
    }
}