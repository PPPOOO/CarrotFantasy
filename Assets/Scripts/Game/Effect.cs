using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{

    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestroyEffect", animationTime);
    }

    private void DestroyEffect()
    {
        GameController.Instance.PushGameObjectToFactory(resourcePath, gameObject);
    }
}
