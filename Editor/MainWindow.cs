using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDriveDownloaderのメイン画面
    /// </summary>
    public class MainWindow : EditorWindow
    {
        /// <summary>
        /// ダウンロード対象のスプレッドシートのID
        /// </summary>
        string sheetID;

        /// <summary>
        /// ウインドウをオープンする
        /// </summary>
        [MenuItem("Window/GoogleDriveDownloader")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MainWindow),
                                false,
                                "GoogleDriveDownloader",
                                true
                            );
        }

        /// <summary>
        /// 各UIウィジェットを作成
        /// </summary>
        void OnGUI()
        {
            sheetID = EditorGUILayout.TextField(
                "Sheet ID",
                sheetID
            );

            if (GUILayout.Button("Download"))
            {
                var loader = new SheetLoader();
                var sheetData = loader.LoadSheetData(sheetID, 5);

                ShowRowData(sheetData.GetRow("1"));
                ShowRowData(sheetData.GetRow("2"));
                ShowRowData(sheetData.GetRow("3"));
            }
        }

        private void ShowRowData(Dictionary<string, string> rowData)
        {
            Debug.Log($"NAME : {rowData["NAME"]}, ATK : {rowData["ATK"]}, DEF : {rowData["DEF"]}, SPD : {rowData["SPD"]}, HP : {rowData["HP"]}");
        }
    }
}