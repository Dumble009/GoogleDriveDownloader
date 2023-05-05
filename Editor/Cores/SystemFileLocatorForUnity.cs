namespace GoogleDriveDownloader
{
    /// <summary>
    /// Unity上における、コンフィグファイルや認証情報ファイルが格納されているフォルダのパスを返すクラス
    /// </summary>
    public class SystemFileLocatorForUnity : ISystemFileLocator
    {
        // Unityエディタ上で実行されるコードの相対パスはプロジェクトルートからの相対パスとして解決される

        /// <summary>
        /// Unityプロジェクトルートからのコンフィグファイルが格納されているフォルダの相対パス
        /// Configフォルダはプロジェクトルートに配置されている
        /// </summary>
        const string CONFIG_FOLDER_RELATIVE_PATH = "Config";


        /// <summary>
        /// Unityプロジェクトルートからの認証情報ファイルが格納されているフォルダの相対パス
        /// Credentialsフォルダはプロジェクトルートに配置されている。
        /// </summary>
        const string CREDENTIALS_FOLDER_RELATIVE_PATH = "Credentials";

        public string GetConfigFolderPath()
        {
            return CONFIG_FOLDER_RELATIVE_PATH;
        }

        public string GetCredentialsFolderPath()
        {
            return CREDENTIALS_FOLDER_RELATIVE_PATH;
        }
    }
}