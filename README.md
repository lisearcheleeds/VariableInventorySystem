# VariableInventorySystem
<strong>VariableInventorySystem</strong> は自由なセルサイズを持つインベントリシステムです。<br /><br />

CellはそれぞれWidth, Heightパラメータを持ち、各View間でのプレイヤーによる相互移動やAuto Fill、Inventory in Inventory、セルの回転に対応しています。<br /><br />

開発者は実装済みのStandardをSampleと同じ用に実装するか、Standardの実装を便りに独自の表示を実装するか拡張を行うか選択出来ます。<br /><br />

また、<strong>VariableInventorySystem</strong>はあなたのプロジェクト独自のボタンやリソース、リソースローダーに対応するよう拡張が可能です<br /><br />

Standardを利用した場合に開発者が行う実装は以下です<br />
- IVariableInventoryCellData を実装したCellDataクラスの作成<br />
- IVariableInventoryAsset を実装したCellAssetクラスの作成<br />
- IVariableInventoryAsset を非同期で読み込むためのLoaderクラスの作成<br /><br />

これらの処理はGitHubで確認が可能です<br />
https://github.com/lisearcheleeds/VariableInventorySystem<br />

後で説明を書く
