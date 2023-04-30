using System.IO;
using System.Diagnostics;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// ソースコードのファイルパスを計算して返す機能を提供するクラス
    /// </summary>
    public class SourceCodeLocator : ISourceCodeLocator
    {
        public string GetDirectoryOfSourceCodePath()
        {
            // スタックトレースを使用して、呼び出し元のファイルパスを取得する
            var callerFileName = new StackTrace(true)
                                .GetFrame(1) // 0番がこの関数の呼び出し情報なので、1番のフレームを取得する事で、この関数を呼び出した関数のフレームを取得できる。
                                .GetFileName();

            var directoryName = Path.GetDirectoryName(callerFileName);

            return directoryName;
        }
    }
}