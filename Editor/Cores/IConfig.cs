namespace GoogleDriveDownloader
{
    /// <summary>
    /// コンフィグファイルを読み込み、外部クラスにその内容を提供するクラスのインタフェース
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Googleドライブ上の、メタシートのIDの取得
        /// </summary>
        /// <returns>
        /// Googleドライブ内の、メタシートのID文字列
        /// </returns>
        public string GetMetaSheetID();

        /// <summary>
        /// ファイルのエクスポート先となるフォルダのパス
        /// </summary>
        /// <returns>
        /// ファイルのエクスポート先となるフォルダのパス。Assetsフォルダからの相対パス
        /// </returns>
        public string GetExportRootPath();
    }
}