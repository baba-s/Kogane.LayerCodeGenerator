# Kogane Layer Code Generator

レイヤー名を管理するクラスを生成するエディタ拡張

## 使用例

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

Project Settings で設定を変更できます