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
        /// ドライブ上からシートを読み込むために使用する
        /// </summary>
        ISheetLoader sheetLoader;

        /// <summary>
        /// ドライブから読み込んだシートをエクスポート可能な形式に変換するために使う
        /// </summary>
        ISheetDataConverter converter;

        /// <summary>
        /// ファイルのエクスポート先を設定ファイルから取得するために使う
        /// </summary>
        IConfig config;

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
            config = _config;

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
        /// <param name="metaSheetDatas">
        /// エクスポートしたいシートのメタデータのリスト
        /// </param>
        private void OnExport(List<MetaSheetData> metaSheetDatas)
        {
            foreach (var metaSheetData in metaSheetDatas)
            {
                var sheetData = sheetLoader.LoadSheetData(
                    metaSheetData.SheetID,
                    metaSheetData.SheetName
                );
                var fileContent = converter.Convert(sheetData);

                var savePath = Path.Combine(
                    config.GetExportRootPath(),
                    metaSheetData.SavePath
                );

                // ディレクトリが無ければ作る必要がある
                var directoryName = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                File.WriteAllBytes(savePath, fileContent.ToArray()); // WriteAllBytesはファイルがあれば上書きし、なければ作って書く
            }
        }
    }
}