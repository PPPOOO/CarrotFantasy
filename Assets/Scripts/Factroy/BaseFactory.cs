using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseFactory : IBaseFactory
{
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    public void PushItem(string itemName,GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objectPoolDict.ContainsKey(itemName))
        {
            objectPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前字典没有" + itemName + "的栈");
        }
    }

    public GameObject GetItem(string itemName)
    {
        GameObject itemGo = null;
        if (objectPoolDict.ContainsKey(itemName))
        {
            if (objectPoolDict[itemName].Count == 0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.Instance.CreateItem(go);
            }
            else
            {
                itemGo = objectPoolDict[itemName].Pop();
                itemGo.SetActive(true);
            }
        }
        else
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.Instance.CreateItem(go);
        }

        if (itemGo == null)
        {
            Debug.Log(itemName + "的名字物体获取失败");
        }
        return itemGo;
    }

    private GameObject GetResource(string itemName)
    {
        GameObject itemGo = null;
        string itemLoadPath = loadPath + itemName;
        if (factoryDict.ContainsKey(itemName))
        {
            itemGo = factoryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            factoryDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName+"的资源获取失败 ");
            Debug.Log("失败路径" + itemLoadPath);
        }
        return itemGo;
    }
    
}
