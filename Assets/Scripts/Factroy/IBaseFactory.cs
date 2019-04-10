using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseFactory
{
    GameObject GetItem(string ItemName);

    void PushItem(string itemName, GameObject item);
}
