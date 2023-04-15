
namespace GoogleDriveDownloader
{
    /// <summary>
    /// ソースコードが格納されているディレクトリのパスを計算する機能を提供するクラスが実装するインタフェース
    /// </summary>
    public interface ISourceCodeLocator
    {
        /// <summary>
        /// このメソッドを呼び出したクラスが格納されているディレクトリのパスを返す。
        /// </summary>
        /// <returns>
        /// このメソッドを呼び出したクラスのファイルが格納されているディレクトリのパス。
        /// 絶対パスか相対パスかは保証されない。
        /// </returns>
        string GetDirectoryOfSourceCodePath();
    }
}