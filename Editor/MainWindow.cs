using UnityEngine;
using UnityEditor;

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
            Debug.Log(sheetID);
        }
    }
}