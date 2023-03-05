using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDrive上のメタシートを読み込んで、使いやすいように変換したデータを返すクラス
    /// </summary>
    public class MetaSheetLoader
    {
        /// <summary>
        /// このソースコードのファイルから設定ファイルまでの相対パス
        /// </summary>
        const string CONFIG_RELATIVE_PATH = "../Config/config.json";

        /// <summary>
        /// 設定ファイル中のID情報のキー
        /// </summary>
        const string CONFIG_FILE_KEY_ID = "ID";
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

        /// <summary>
        /// 設定ファイルからメタシートのシートIDを読み込んで返す
        /// </summary>
        /// <returns>メタシートのシートIDの文字列</returns>
        private string LoadMetaSheetID()
        {
            try
            {
                // jsonファイルをテキスト形式で開き、読み込んだ文字列をパースする
                using (StreamReader file = File.OpenText(GetConfigPath()))
                {
                    string jsonString = file.ReadToEnd();

                    var dic = JsonConvert.DeserializeObject
                                <Dictionary<string, string>>
                                (jsonString);

                    return dic["ID"];
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Loading Meta Sheet ID failed." + e.Message);
            }
        }
    }
}