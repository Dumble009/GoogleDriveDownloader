using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// メタシートから取得したシートデータを一覧表示するUIが実装するインタフェース
    /// </summary>
    public interface ISheetListUI
    {
        /// <summary>
        /// 一覧表示する情報を更新する。
        /// 既存のUIは全て新しい情報で置き換えられる
        /// </summary>
        /// <param name="metaSheetDatas">
        /// 新しく一覧表示したいMetaSheetDataのリスト
        /// </param>
        void UpdateList(List<MetaSheetData> metaSheetDatas);
    }
}