using System;
using System.Collections;
using UnityEngine;

namespace VariableInventory
{
    public class StandardAssetLoader
    {
        public virtual IEnumerator LoadAsync(IVariableInventoryAsset imageAsset, Action<Texture2D> onLoad)
        {
            var loader = Resources.LoadAsync<Texture2D>((imageAsset as StandardAsset).Path);
            yield return loader;
            onLoad(loader.asset as Texture2D);
        }
    }
}
