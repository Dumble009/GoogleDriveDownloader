using UnityEngine;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// 仕切り線の描画機能を提供するクラス
    /// </summary>
    public class SeparateLine
    {
        /// <summary>
        /// 仕切り線に適用するテクスチャ
        /// 仕切り線(Box)の色を直接変える事は出来ないので、
        /// プロシージャルに生成したテクスチャを適用する事で色を変える
        /// </summary>
        private static Texture2D lineTexture;

        /// <summary>
        /// ウインドウに横長の仕切り線を描画する
        /// </summary>
        /// <param name="lineThickness">
        /// 線の太さ
        /// </param>
        static public void Draw(int lineThickness = 1)
        {
            var style = new GUIStyle();
            style.normal.background = GetOrCreateLineTexture();
            GUILayout.Box("", style, GUILayout.ExpandWidth(true), GUILayout.Height(lineThickness));
        }

        /// <summary>
        /// 仕切り線に適用するテクスチャを作成するか、キャッシュから返す
        /// </summary>
        /// <returns>
        /// 仕切り線に適用するテクスチャ
        /// </returns>
        static private Texture2D GetOrCreateLineTexture()
        {
            if (lineTexture == null)
            {
                const int TEX_WIDTH = 2, TEX_HEIGHT = 2;
                Color[] pixels = new Color[TEX_WIDTH * TEX_HEIGHT];

                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = Color.white;
                }
                lineTexture = new Texture2D(TEX_WIDTH, TEX_HEIGHT);

                lineTexture.SetPixels(pixels);
                lineTexture.Apply();
            }
            return lineTexture;
        }
    }
}