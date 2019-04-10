using UnityEngine;
using System.Collections;

public interface IBuilder <T>
{
    T GetProductClass(GameObject gameobject);

    GameObject GetProduct();

    void GetData(T productClassGo);

    void GetOtherResource(T productClassGo);
}
