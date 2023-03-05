using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDrive上のメタシートを読み込んで、使いやすいように変換したデータを返すクラス
    /// </summary>
    public class MetaSheetLoader
    {
        const string CONFIG_RELATIVE_PATH = "../Config/config.json";
        public MetaSheetLoader()
        {

        }

        /// <summary>
        /// コンフィグファイルのパスを取得する
        /// </summary>
        /// <returns>このソースコードのファイルパスを元に計算された設定ファイルのパス</returns>
        private string GetConfigPath()
        {
            // スタックトレースからこのソースファイルのパスを取得する
            var stackFrame = new StackFrame(true);
            string sourceFilePath = stackFrame.GetFileName();


            // このソースコードが格納されているディレクトリのパスを取得し、
            // そこからの相対パスとして設定ファイルのパスを作成
            string sourceDirPath = Path.GetDirectoryName(sourceFilePath);

            return Path.Combine(sourceDirPath, CONFIG_RELATIVE_PATH);
        }
    }
}