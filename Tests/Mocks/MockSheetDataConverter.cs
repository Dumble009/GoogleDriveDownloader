using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// ISheetDataConverterのモッククラス。
/// Convert関数で引数として渡されたSheetDataオブジェクトを外部に公開し、
/// 事前に設定されたバイト配列をConvert関数の返り値として返す
/// </summary>
public class MockSheetDataConverter : ISheetDataConverter
{
    List<byte> convertResult;
    /// <summary>
    /// Convert関数の返り値として返されるバイト配列
    /// </summary>
    public List<byte> ConvertResult
    {
        set => convertResult = value;
    }

    SheetData passedSheetData;
    /// <summary>
    /// Convert関数の引数として渡されたSheetDataオブジェクト
    /// </summary>
    public SheetData PassedSheetData
    {
        get => passedSheetData;
    }

    public List<byte> Convert(SheetData sheetData)
    {
        passedSheetData = sheetData;

        return convertResult;
    }
}
