using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteFactory : IBaseResourcesFactory<Sprite>
{


    protected Dictionary<string, Sprite> factoryDict = new Dictionary<string, Sprite>();
    protected string loadPath;

    public SpriteFactory()
    {
        loadPath = "Pictures/";
    }

    public Sprite GetSingleResources(string resourcePath)
    {
        Sprite itemGo = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(itemLoadPath))
        {
            itemGo = factoryDict[itemLoadPath];
        }
        else
        {
            itemGo = Resources.Load<Sprite>(itemLoadPath);
            factoryDict.Add(itemLoadPath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemLoadPath + "的资源加载失败");
        }

        return itemGo;
    }
}
