using UnityEngine;
using UnityEditor;
using System.IO;
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
        /// スプレッドシートから読み込んだ内容を書き込むファイルのパス。Assets以下の相対パスで指定する。
        /// </summary>
        string savePath;

        /// <summary>
        /// スプレッドシートの内容を変換してくれるオブジェクト
        /// </summary>
        ISheetDataConverter converter;

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

            savePath = EditorGUILayout.TextField(
                "Save Path",
                savePath
            );

            if (GUILayout.Button("Download"))
            {
                var loader = new SheetLoader();
                var sheetData = loader.LoadSheetData(sheetID, 5);

                ShowRowData(sheetData.GetRow("1"));
                ShowRowData(sheetData.GetRow("2"));
                ShowRowData(sheetData.GetRow("3"));

                converter = new JSONConverter();

                // 最終的なファイルの出力先となるパス
                string completePath = Path.Combine(Application.dataPath, savePath);
                ExportToFile(completePath, converter.Convert(sheetData));
            }
        }

        /// <summary>
        /// bytesの内容をファイルへと出力する。
        /// </summary>
        /// <param name="path">出力先のファイルパス</param>
        /// <param name="bytes">出力するバイト配列の内容</param>
        void ExportToFile(string path, List<byte> bytes)
        {
            File.WriteAllBytes(path, bytes.ToArray());
        }

        private void ShowRowData(Dictionary<string, string> rowData)
        {
            Debug.Log($"NAME : {rowData["NAME"]}, ATK : {rowData["ATK"]}, DEF : {rowData["DEF"]}, SPD : {rowData["SPD"]}, HP : {rowData["HP"]}");
        }
    }
}