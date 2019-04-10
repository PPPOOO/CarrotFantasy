using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeAnimatiorControllerFactory : IBaseResourcesFactory<RuntimeAnimatorController>
{

    protected Dictionary<string, RuntimeAnimatorController> factoryDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;
    public RuntimeAnimatiorControllerFactory()
    {
        loadPath = "Animator/AnimatorController/";
    }

    public RuntimeAnimatorController GetSingleResources(string resourcePath)
    {
        RuntimeAnimatorController itemGo = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(resourcePath + "的资源加载失败");
        }

        return itemGo;
    }
}
