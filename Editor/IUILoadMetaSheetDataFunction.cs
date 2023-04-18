using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// UIからメタシートの読み込み操作が行われた際に呼び出されるイベントのデリゲート
    /// </summary>
    public delegate void UILoadMetaSheetDataFunctionHandler();

    /// <summary>
    /// メタシートのデータを読み込んでUIに反映する機能を提供するインタフェース
    /// </summary>
    public interface IUILoadMetaSheetDataFunction
    {
        /// <summary>
        /// メタシートのデータの読み込み操作が行われた際に実行されるイベントを登録する
        /// </summary>
        /// <param name="_handler">
        /// メタシートのデータの読み込み操作が行われた際に実行されるイベント
        /// </param>
        void RegisterOnTriggered(UILoadMetaSheetDataFunctionHandler _handler);

        /// <summary>
        /// 読み込まれたメタシートのデータを受け取りUIに反映する
        /// </summary>
        /// <returns></returns>
        void PassNewMetaSheetDatas(List<MetaSheetData> metaSheetDatas);
    }
}