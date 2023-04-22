using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

public class UIDrawerTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    UIDrawer target;

    /// <summary>
    /// モックUI要素を格納するリスト
    /// </summary>
    List<IUIElement> uis;

    [SetUp]
    public void Setup()
    {
        // とりあえずリストを作って渡しておく。
        // 後から要素を入れれば、自動的にDrawerの方にも反映される
        uis = new List<IUIElement>();
        target = new UIDrawer(uis);
    }

    /// <summary>
    /// 複数のUI要素に対して、正常に描画指示を出す事が出来るか調べるテスト
    /// </summary>
    [Test]
    public void MultipleElementsTest()
    {
        var ui1 = new MockUIElement();
        var ui2 = new MockUIElement();
        var ui3 = new MockUIElement();

        uis.Add(ui1);
        uis.Add(ui2);
        uis.Add(ui3);

        // 実行順を把握できるように、
        // OnDrawが呼び出された時に文字列に異なる値を加える
        string calledFlag = "";
        ui1.OnDraw += () => { calledFlag += '1'; };
        ui2.OnDraw += () => { calledFlag += '2'; };
        ui3.OnDraw += () => { calledFlag += '3'; };

        target.Draw();

        Assert.AreEqual("123", calledFlag);

        // Drawは何度も呼び出される処理なので、複数回の呼び出しをテストしておく
        target.Draw();
        target.Draw();

        Assert.AreEqual("123123123", calledFlag);
    }

    /// <summary>
    /// 一つのUI要素のみに対して、正常に描画指示を出す事が出来るか調べるテスト
    /// </summary>
    [Test]
    public void SingleElementTest()
    {
        var ui1 = new MockUIElement();
        uis.Add(ui1);

        target.Draw();

        Assert.AreEqual(1, ui1.DrawCallCount);

        target.Draw();
        target.Draw();

        Assert.AreEqual(3, ui1.DrawCallCount);
    }

    /// <summary>
    /// 1つもUI要素を持たなくてもエラーなどが出ないか調べるテスト
    /// </summary>
    [Test]
    public void NoElementTest()
    {
        Assert.DoesNotThrow(() =>
        {
            target.Draw();
            target.Draw();
            target.Draw();
        });
    }
}
