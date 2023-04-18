using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDrive上のメタシートを読み込んで、使いやすいように変換したデータを返すクラスが実装するインタフェース
    /// </summary>
    public interface IMetaSheetLoader
    {

        /// <summary>
        /// メタシートを読み込んで、各行のデータを要素に持つリストを返す
        /// </summary>
        /// <returns>各行のデータを1つの要素として持つリスト</returns>
        List<MetaSheetData> LoadMetaSheet();
    }
}