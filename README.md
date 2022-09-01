# Kogane Layer Code Generator

レイヤー名を管理するクラスを生成するエディタ拡張

## 使用例

![2022-09-01_211605](https://user-images.githubusercontent.com/6134875/187912814-8945e86f-732a-475e-8f0d-2e119c215c06.png)

Tags & Layers で「Layers」を変更して保存すると

```csharp
using System.Collections.Generic;

namespace Kogane
{
    public static partial class Layers
    {
        public static Layer Default { get; } = new Layer( "Default" );
        public static Layer TransparentFX { get; } = new Layer( "TransparentFX" );
        public static Layer IgnoreRaycast { get; } = new Layer( "Ignore Raycast" );
        public static Layer Water { get; } = new Layer( "Water" );
        public static Layer UI { get; } = new Layer( "UI" );

        public static IReadOnlyList<Layer> All { get; } = new[]
        {
            Default,
            TransparentFX,
            IgnoreRaycast,
            Water,
            UI,
        };
    }
}
```

上記のようなレイヤー名を管理するクラスが自動で生成されます

```csharp
using Kogane;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Awake()
    {
        gameObject.layer = Layers.UI;

        var isHit = Physics.Raycast
        (
            origin: Vector3.zero,
            direction: Vector3.forward,
            maxDistance: float.MaxValue,
            layerMask: Layers.UI.Mask
        );

        // {
        //     "m_name": "Default",
        //     "m_index": 5,
        //     "m_mask": 32
        // }
        Debug.Log( Layers.UI );
    }
}
```

生成されたクラスは上記のように使用できます

![2022-09-01_212220](https://user-images.githubusercontent.com/6134875/187912837-f7f4b60c-d446-4c5a-a033-e1a749f52f18.png)

Project Settings で設定を変更できます

|項目|内容|
|:--|:--|
|Generate|手動でコードを生成します|
|Reset to Default|設定をデフォルトに戻します|
|Is Auto Generate|自動生成を有効化するかどうか|
|Output Path|コードの出力先|
|XXXX Template|コードのテンプレート|