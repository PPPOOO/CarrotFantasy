using UnityEngine;
using System.Collections;

public class Prize : MonoBehaviour
{

    private void Update()
    {
        if (GameController.Instance.gameOver)
        {
            GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
        }
    }

    private void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GiftGot");
        GameController.Instance.isPause = true;
#if Game
            GameController.Instance.ShowPrizePage();
#endif

        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
    }
}
