using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CoinMove : MonoBehaviour
{
    private Text coinText;
    private Image coinImage;
    public Sprite[] coinSprites;

    public int prize;
    private void Awake()
    {
        coinText = transform.Find("Tex_Coin").GetComponent<Text>();
        coinImage = transform.Find("Img_Coin").GetComponent<Image>();
        coinSprites = new Sprite[2];
        coinSprites[0] = GameController.Instance.GetSprite("NormalModel/Game/Coin");
        coinSprites[1] = GameController.Instance.GetSprite("NormalModel/Game/ManyCoin");
    }

    private void OnEnable()
    {
        ShowCoin();
    }

    private void ShowCoin()
    {
        coinText.text = prize.ToString();
        if (prize >= 500)
        {
            coinImage.sprite = coinSprites[1];
        }
        else
        {

            coinImage.sprite = coinSprites[0];
        }
        transform.DOLocalMoveY(60, 0.5f);
        DOTween.To(() => coinImage.color, toColor => coinImage.color=toColor, new Color(1, 1, 1, 0),0.5f);
        Tween tween = DOTween.To(() => coinText.color, toColor => coinText.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        tween.OnComplete(DestroyCoin);
    }
    
    private void DestroyCoin()
    {
        transform.localPosition = Vector3.zero;
        coinImage.color = new Color(1, 1, 1, 1);
        coinText.color = new Color(1, 1, 1, 1);
        GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
    }
}
