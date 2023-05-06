using UnityEngine;
using UnityEditor;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// ドライブ上の、あるシートの情報とそのエクスポートボタンを表示するUI
    /// </summary>
    public class SheetListItem
    {
        /// <summary>
        /// エクスポートボタンに表示する文言
        /// </summary>
        const string EXPORT_BUTTON_LABEL = "Export";

        /// <summary>
        /// エクスポート先を表示するラベルのテンプレート
        /// </summary>
        const string EXPORT_PATH_LABEL_TEMPLATE = "Export to : {0}";

        /// <summary>
        /// エクスポートボタンの横幅
        /// </summary>
        const int BUTTON_WIDTH = 100;

        /// <summary>
        /// 現在折り畳みを展開しているかどうか。trueなら展開して中身を表示している
        /// </summary>
        bool isShown = false;

        /// <summary>
        /// このオブジェクトが対象とするシートのメタデータ
        /// </summary>
        MetaSheetData passedMetaSheetData;

        /// <summary>
        /// エクスポートイベントに対するイベントハンドラ
        /// </summary>
        OnExportSheetHandler onExportHandler;

        /// <summary>
        /// 各シートの名称ラベルの、フォントサイズ等のスタイル
        /// 全てのアイテムで共通のスタイルを使用するので、静的オブジェクトにして生成、破棄のコストを抑える
        /// </summary>
        static GUIStyle displayNameStyle;

        /// <summary>
        /// シートデータを渡し、その中身を用いてオブジェクトを初期化する
        /// </summary>
        /// <param name="metaSheetData">
        /// このオブジェクトに情報を表示してほしいシートのメタ情報
        /// </param>
        public SheetListItem(MetaSheetData metaSheetData)
        {
            passedMetaSheetData = metaSheetData;

            onExportHandler += (_) => { }; // 空関数で初期化しておくことでnull参照を防ぐ

            // displayNameStyleは静的オブジェクトなので、他のオブジェクトが作成していれば再作成する必要は無い
            if (displayNameStyle == null)
            {
                displayNameStyle = new GUIStyle();
                displayNameStyle.fontStyle = FontStyle.Bold;

                var styleState = new GUIStyleState();
                styleState.textColor = Color.white;
                displayNameStyle.normal = styleState;
            }
        }

        /// <summary>
        /// シート情報をまとめたUI要素を描画する
        /// </summary>
        public void Draw()
        {
            GUILayout.Label(passedMetaSheetData.DisplayName, displayNameStyle);

            GUILayout.Label(
                string.Format(
                    EXPORT_PATH_LABEL_TEMPLATE,
                    passedMetaSheetData.SavePath
                )
            );

            if (GUILayout.Button(EXPORT_BUTTON_LABEL, GUILayout.Width(BUTTON_WIDTH)))
            {
                onExportHandler(passedMetaSheetData);
            }
        }

        /// <summary>
        /// エクスポートボタンを押された際に実行される処理を登録する
        /// </summary>
        /// <param name="_handler">
        /// エクスポートボタンを押された際に実行される処理
        /// </param>
        public void RegisterOnExportSheet(OnExportSheetHandler _handler)
        {
            onExportHandler += _handler;
        }
    }

}