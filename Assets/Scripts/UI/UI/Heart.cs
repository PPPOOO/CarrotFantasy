using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestoryEffect", animationTime);
    }

    private void DestoryEffect()
    {
        GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].PushItem(resourcePath,gameObject);
    }

}
