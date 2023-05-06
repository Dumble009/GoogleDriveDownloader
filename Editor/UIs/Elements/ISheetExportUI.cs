using System.Collections.Generic;

namespace GoogleDriveDownloader
{

    /// <summary>
    /// シートのエクスポート操作が行われた時に呼び出されるイベントのデリゲート
    /// </summary>
    /// <param name="datas">
    /// エクスポートしたいシートの情報を持ったMetaSheetDataのリスト
    /// </param>
    public delegate void OnExportSheetHandler(List<MetaSheetData> datas);

    /// <summary>
    /// 各シートのデータをエクスポートするイベントを提供するUIのインタフェース
    /// </summary>
    public interface ISheetExportUI
    {
        /// <summary>
        /// シートをエクスポートする操作が行われた際に
        /// 呼び出されるイベントを登録する
        /// </summary>
        /// <param name="_handler">
        /// シートをエクスポートする操作が行われた際に呼び出されるイベント
        /// </param>
        void RegisterOnExportSheet(OnExportSheetHandler _handler);
    }
}