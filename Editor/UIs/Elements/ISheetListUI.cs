using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// シートのエクスポート操作が行われた時に呼び出されるイベントのデリゲート
    /// </summary>
    /// <param name="data">
    /// エクスポートしたいシートの情報を持ったMetaSheetData
    /// </param>
    public delegate void OnExportSheetHandler(MetaSheetData data);

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

        void RegisterOnExportSheet(OnExportSheetHandler _handler);
    }
}