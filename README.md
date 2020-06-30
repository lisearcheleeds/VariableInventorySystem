# VariableInventorySystem
<strong>VariableInventorySystem</strong> は自由なセルサイズを持つインベントリシステムです。<br /><br />

<img src="https://user-images.githubusercontent.com/13120364/85971579-7db8f700-ba08-11ea-8a7c-3853539d8fd5.png">

CellはそれぞれWidth, Heightパラメータを持ち、各View間でのプレイヤーによる相互移動やAuto Fill、Inventory in Inventory、セルの回転に対応しています。<br /><br />

開発者は実装済みのStandardをSampleと同じ用に実装するか、Standardの実装を便りに独自の表示を実装するか拡張を行うか選択出来ます。<br /><br />

また、<strong>VariableInventorySystem</strong>はあなたのプロジェクト独自のボタンやリソース、リソースローダーに対応するよう拡張が可能です<br /><br />

Standardを利用した場合に開発者が行う実装は以下です<br />
- IVariableInventoryCellData を実装したCellDataクラスの作成<br />
- IVariableInventoryAsset を実装したCellAssetクラスの作成<br />
- IVariableInventoryAsset を非同期で読み込むためのLoaderクラスの作成<br /><br />

最新のドキュメント
https://docs.google.com/document/d/1hDmPfOeNtPUTgqlm44_TkWDHCkRnlV_77gpyI6CdCG0/edit?usp=sharing

Latest Document
https://docs.google.com/document/d/1w4u9rgpsuKNblADzEyEZAUxM7iptjvmrMwxQIbipmH8/edit?usp=sharing
