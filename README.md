# GoogleDriveDownloader

## Requirements

### GoogleAPI Library

You need GoogleAPI library. You can download it via [nuget](https://www.nuget.org/downloads). Download `nuget.exe` and execute below;

```bash
nuget.exe install Google.Apis.Sheets.v4 -OutputDirectory Package
```

Then, libraries will be downloaded under Package directory. Each library has variants for different .NET implementations. I used `netstandard2.0` for this project. Copy `.lib` files of each library, except Newtonsoft.Json, to `Packages` directory of this repository.

### Service Account of GoogleCloudPlatform

This tool requires a service account of GoogleCloudPlatform which can access to spreadsheets in your GoogleDrive.

https://cloud.google.com/iam/docs/service-accounts-create

Create `Credentials` directory in your project's root rather than a root of this repository, and put `credentials.json` there.

### Config.json

You need to create configuration file for this tool.

Create `Config` directory in your project's root, and make `Config.json` there. The content of this file should be like below;

```json
{
  "MetaSheetID": "id-of-your-meta-sheet",
  "ExportRootPath": "StreamingAssets"
}
```

## MetaSheet

A MetaSheet is a special spreadsheet which has information of your spreadsheets. It should have 5 columns, `ID`, `SheetID`, `SheetName`, `SavePath`, and `DisplayName`. Each row in this sheet expresses information of corresponding spreadsheet.

- `ID` is an id of each row in a MetaSheet. It should be unique.
- `SheetID` is an id of a spreadsheet in your GoogleDrive. It should be something like `1ABCHEFGHIJKLMNOPQRSTUVWXYZ0123456789`. You can check an sheet id of your spreadsheet in its url.
- `SheetName` is a name of a sheet (not a spreadsheet, a sheet inside a spreadsheet) which you want to refer.
- `SavePath` is a path where exported json file will be saved. It's a relative path from `ExportRootPath` of Config.json. So, if `ExportRootPath` is `"StreamingAssets"` and `SavePath` is `"parameters/character1.json"`, exported file will be saved as `"character1.json"` in `"StreamingAssets/parameters"`.
- `DisplayName` is a name of the row which will be displayed on the window of this tool.

Your MetaSheet should be structured like below table;

| ID  | SheetID | SheetName        | SavePath                          | DisplayName      |
| --- | ------- | ---------------- | --------------------------------- | ---------------- |
| 1   | 1ABC~   | PlayerCharacters | parameters/player_characters.json | PC Parameters    |
| 2   | 1ABC~   | EnemyCharacters  | parameters/enemy_characters.json  | Enemy Parameters |
| 3   | 1DEF    | StageNames       | stagenames.json                   | Stages           |

## SpreadSheet

Your spreadsheet should be structured like below table;

| ID  | ATK | DEF | SPD |
| --- | --- | --- | --- |
| 0   | 10  | 15  | 20  |
| 1   | 30  | 20  | 10  |
| 2   | 10  | 40  | 5   |

- The first row of the sheet should have a name of each column. This name will be used as a name of a paramater in a json file.
- The first column of the sheet must be `ID`. The value of this column must be unique.
