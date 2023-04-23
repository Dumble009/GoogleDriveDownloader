using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// ユーザ操作に従って、シートのデータを読み込み、ゲームで使える形式にエクスポートする機能
    /// </summary>
    public class SheetExportFunction : IUIFunction
    {
        /// <summary>
        /// 依存性注入を行うコンストラクタ
        /// </summary>
        /// <param name="_sheetLoader">
        /// シートの読み込み処理を行ってくれるオブジェクト
        /// </param>
        /// <param name="_converter">
        /// 読み込んだシートを適切な形式に変換してくれるクラス
        /// </param>
        public SheetExportFunction(
            ISheetLoader _sheetLoader,
            ISheetDataConverter _converter
        )
        {

        }

        public void SetUIElements(List<IUIElement> uis)
        {
            throw new System.NotImplementedException();
        }
    }
}