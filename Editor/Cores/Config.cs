
namespace GoogleDriveDownloader
{
    /// <summary>
    /// コンフィグファイルを読み込み、その内容を外部のクラスに提供する
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// オブジェクトの作成と同時に、コンフィグファイルの読み込み、内容のパースを行う
        /// </summary>
        /// <param name="sourceCodeLocator">
        /// コンフィグファイルのパスを計算するために使用する、ソースコードのパスを返してくれるオブジェクト
        /// </param>
        public Config(ISourceCodeLocator sourceCodeLocator)
        {

        }

        public string GetExportRootPath()
        {
            throw new System.NotImplementedException();
        }

        public string GetMetaSheetID()
        {
            throw new System.NotImplementedException();
        }
    }
}