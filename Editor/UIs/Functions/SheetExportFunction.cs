using System.IO;
using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// ユーザ操作に従って、シートのデータを読み込み、ゲームで使える形式にエクスポートする機能
    /// </summary>
    public class SheetExportFunction : IUIFunction
    {
        /// <summary>
        /// シートの読み込み処理を行うオブジェクト
        /// </summary>
        ISheetLoader sheetLoader;

        /// <summary>
        /// 読み込んだシートのデータをファイル出力できる形式に変換してくれるオブジェクト
        /// </summary>
        ISheetDataConverter converter;

        /// <summary>
        /// このオブジェクトが購読している、エクスポート操作が行われた際にイベントを発行するUIのリスト
        /// </summary>
        List<ISheetExportUI> exportUIs;

        /// <summary>
        /// 依存性注入を行うコンストラクタ
        /// </summary>
        /// <param name="_sheetLoader">
        /// ドライブ上からシートのデータを読み込むために必要
        /// </param>
        /// <param name="_converter">
        /// シートから読み込んだデータをエクスポートする形式に変換するために必要
        /// </param>
        /// <param name="_converter">
        /// エクスポート先などの設定項目を取得するために必要
        /// </param>
        public SheetExportFunction(
            ISheetLoader _sheetLoader,
            ISheetDataConverter _converter,
            IConfig _config
        )
        {
            sheetLoader = _sheetLoader;
            converter = _converter;

            exportUIs = new List<ISheetExportUI>();
        }

        public void SetUIElements(List<IUIElement> uis)
        {
            foreach (var ui in uis)
            {
                if (ui is ISheetExportUI sheetExportUI)
                {
                    if (!exportUIs.Contains(sheetExportUI))
                    {
                        sheetExportUI.RegisterOnExportSheet(OnExport);
                        exportUIs.Add(sheetExportUI);
                    }
                }
            }
        }

        /// <summary>
        /// シートのエクスポート操作が行われた際に呼び出される処理
        /// </summary>
        /// <param name="metaSheetData">
        /// エクスポートしたいシートのメタデータ
        /// </param>
        private void OnExport(MetaSheetData metaSheetData)
        {
            var sheetData = sheetLoader.LoadSheetData(metaSheetData.SheetID);
            var fileContent = converter.Convert(sheetData);

            File.WriteAllBytes(metaSheetData.SavePath, fileContent.ToArray()); // WriteAllBytesはファイルがあれば上書きし、なければ作って書く
        }
    }
}