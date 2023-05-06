using System.Collections.Generic;
using UnityEngine;
using GoogleDriveDownloader;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// メタシート上の全シートを読み込んでエクスポートする機能のボタン
    /// </summary>
    public class ExportAllButton : IUIElement, IMetaSheetDataReceiveUI, ISheetExportUI
    {
        /// <summary>
        /// ボタンの横幅
        /// </summary>
        const int BUTTON_WIDTH = 100;

        /// <summary>
        /// ボタンに表示するラベル
        /// </summary>
        const string BUTTON_LABEL = "Export All";

        /// <summary>
        /// エクスポート対象のMetaSheetDataのリスト
        /// </summary>
        List<MetaSheetData> exportTargets;

        /// <summary>
        /// ボタンを押された時に呼び出す機能
        /// </summary>
        OnExportSheetHandler onExportHandler;

        public ExportAllButton()
        {
            exportTargets = new List<MetaSheetData>();
            onExportHandler = (_) => { }; // 空関数で初期化しnull参照を防ぐ
        }

        public void Draw()
        {
            if (
                GUILayout.Button(
                    BUTTON_LABEL,
                    GUILayout.Width(BUTTON_WIDTH)
                )
            )
            {
                onExportHandler(exportTargets);
            }
        }

        public void RegisterOnExportSheet(OnExportSheetHandler _handler)
        {
            onExportHandler += _handler;
        }

        public void UpdateList(List<MetaSheetData> metaSheetDatas)
        {
            exportTargets = metaSheetDatas;
        }
    }
}