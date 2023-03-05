using System;
using System.Reflection;

/// <summary>
/// テストコードが使用する共通処理をまとめた静的クラス
/// </summary>
public class TestUtil
{
    /// <summary>
    /// 任意のオブジェクトのprivateメソッドを実行する
    /// </summary>
    /// <typeparam name="T">privateメソッドを呼び出したいオブジェクトの型</typeparam>
    /// <param name="obj">privateメソッドを呼び出したいオブジェクト</param>
    /// <param name="methodName">呼び出したいprivateメソッドの名称</param>
    /// <param name="args">渡したい引数の配列。無ければnull</param>
    /// <returns>実行したprivate関数の返り値。無ければnull</returns>
    static public object CallPrivateMethod<T>(
        T obj,
        string methodName,
        object[] args
    )
    {
        Type type = typeof(T);

        // T型のmethodNameという関数の情報を取得
        MethodInfo methodInfo = type.GetMethod(
            methodName,
            BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance
        );

        // 取得した関数を呼び出し
        return methodInfo.Invoke(obj, args);
    }
}
