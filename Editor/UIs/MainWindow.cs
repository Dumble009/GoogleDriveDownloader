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
        /// 画面を構成する全てのUI要素のリスト
        /// </summary>
        List<IUIElement> uis;

        /// <summary>
        /// コア機能のオブジェクトをまとめて保持するオブジェクト
        /// </summary>
        CoreObjects coreObjects;

        /// <summary>
        /// UIが提供する全ての機能のリスト
        /// </summary>
        List<IUIFunction> uiFunctions;

        /// <summary>
        /// UIの描画処理を行うオブジェクト
        /// </summary>
        UIDrawer uiDrawer;

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

        private void Awake()
        {
            uis = CreateUIElements();

            coreObjects = CreateCoreObjects();

            uiFunctions = CreateUIFunctions(coreObjects, uis);

            uiDrawer = new UIDrawer(uis);
        }

        private void OnGUI()
        {
            uiDrawer.Draw();
        }


        /// <summary>
        /// ウインドウに描画する全てのUIをインスタンス化し、そのリストを返す
        /// </summary>
        /// <returns>
        /// 描画対象のUIのリスト。上に描画されるものから順に先頭に配置されている
        /// </returns>
        private static List<IUIElement> CreateUIElements()
        {
            var retVal = new List<IUIElement>();

            // ウインドウの描画に必要なのは、リロードボタンと、シートの一覧表示
            // リストの先頭から順番に上に描画されていくので、上に描画してほしい物から順番にリストに入れていく

            retVal.Add(new ReloadUI());
            retVal.Add(new SheetList());

            return retVal;
        }

        /// <summary>
        /// UI機能を実装するオブジェクトを全て作成し、既存のUI要素と紐づけて返す
        /// </summary>
        /// <param name="coreObjects">
        /// UI機能が依存しているコア機能のオブジェクト群
        /// </param>
        /// <param name="uis">
        /// UI機能と結びつくUI要素のリスト
        /// </param>
        /// <returns>
        /// コア機能の依存性の注入、及びUI要素との紐づけが完了したUI機能オブジェクトのリスト
        /// </returns>
        private static List<IUIFunction> CreateUIFunctions(CoreObjects coreObjects, List<IUIElement> uis)
        {
            var retVal = new List<IUIFunction>();

            // 必要なUI機能は、メタシートデータの再読み込み機能とシートのエクスポート機能
            // リスト内の順番は機能に影響を与えない

            retVal.Add(new LoadMetaSheetDataFunction(coreObjects.MetaSheetLoader));
            retVal.Add(new SheetExportFunction(
                coreObjects.SheetLoader,
                coreObjects.SheetDataConverter,
                coreObjects.Config
            ));

            foreach (var uiFunc in retVal)
            {
                uiFunc.SetUIElements(uis);
            }

            return retVal;
        }

        /// <summary>
        /// UI機能が必要とするコア機能のオブジェクトを、依存関係も含めて全て作成して返す
        /// </summary>
        /// <returns>
        /// UI機能が必要とするコア機能のオブジェクトを一まとめにしたオブジェクト
        /// </returns>
        private static CoreObjects CreateCoreObjects()
        {
            var retVal = new CoreObjects();

            retVal.SheetDataConverter = new JSONConverter();

            var spreadSheetService = new SpreadSheetsService();

            retVal.SheetLoader = new SheetLoader(spreadSheetService);

            var sourceCodeLocator = new SourceCodeLocator();
            retVal.Config = new Config(sourceCodeLocator);
            retVal.MetaSheetLoader = new MetaSheetLoader(
                retVal.SheetLoader,
                retVal.Config
            );

            return retVal;
        }

        /// <summary>
        /// UI機能が必要とするコア機能のオブジェクトをまとめて保持するクラス
        /// </summary>
        private class CoreObjects
        {
            /// <summary>
            /// メタシートからデータを読み込む処理を行うオブジェクト。
            /// メタシートのリロード操作を行う際に必要
            /// </summary>
            public IMetaSheetLoader MetaSheetLoader;

            /// <summary>
            /// シートからデータを読み込む処理を行うオブジェクト。
            /// シートのエクスポート処理を行う際に必要
            /// </summary>
            public ISheetLoader SheetLoader;

            /// <summary>
            /// シートから取得したデータをゲームで利用可能な形式等に変換するオブジェクト。
            /// シートのエクスポート処理を行う際に必要
            /// </summary>
            public ISheetDataConverter SheetDataConverter;

            /// <summary>
            /// アプリケーションの設定項目
            /// シートのエクスポート処理、及びメタシートの読み込みを行う際に必要
            /// </summary>
            public IConfig Config;
        }
    }
}