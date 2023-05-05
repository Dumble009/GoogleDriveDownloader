namespace GoogleDriveDownloader
{
    /// <summary>
    /// Unity上における、コンフィグファイルや認証情報ファイルが格納されているフォルダのパスを返すクラス
    /// </summary>
    public class SystemFileLocatorForUnity : ISystemFileLocator
    {
        public string GetConfigFolderPath()
        {
            throw new System.NotImplementedException();
        }

        public string GetCredentialsFolderPath()
        {
            throw new System.NotImplementedException();
        }
    }
}