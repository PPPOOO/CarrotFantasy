using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopPage : MonoBehaviour
{

    private Text tex_coin;
    private Text tex_roundCount;
    private Text tex_TotalCount;
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;
    private GameObject emp_PauseGo;
    private GameObject emp_PlayingTextGo;
    private NormalModelPanel normalModelPanel;

    public Sprite[] btn_gameSpeedSprites;
    public Sprite[] btn_pauseSprites;


    public bool isNormalSpeed;
    private bool isPause;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_coin = transform.Find("Tex_Coin").GetComponent<Text>();
        emp_PlayingTextGo = transform.Find("Emp_PlayingText").gameObject;
        emp_PauseGo = transform.Find("Emp_Pause").gameObject;
        tex_roundCount = emp_PlayingTextGo.transform.Find("Tex_RoundText").GetComponent<Text>();
        tex_TotalCount= emp_PlayingTextGo.transform.Find("Tex_TotalCount").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause= transform.Find("Btn_Pause").GetComponent<Image>();
    }

    private void OnEnable()
    {

        tex_coin.text = GameController.Instance.coin.ToString();
        tex_TotalCount.text = normalModelPanel.totalRound.ToString();
        img_Btn_Pause.sprite = btn_pauseSprites[0];
        img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        isPause = false;
        isNormalSpeed = true;
        emp_PauseGo.SetActive(false);
        emp_PlayingTextGo.SetActive(true);
    }

    public void UpdateCoinText()
    {
        tex_coin.text = GameController.Instance.coin.ToString();
    }
    public void UpdateRoundText()
    {
        normalModelPanel.ShowRoundText(tex_roundCount);
    }

    public void ChangeGameSpeed()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isNormalSpeed = !isNormalSpeed;
        if (isNormalSpeed)
        {
            GameController.Instance.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[1];
        }
    }

    public void PauseGame()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isPause = !isPause;
        if (isPause)
        {
            GameController.Instance.isPause = true;
            img_Btn_Pause.sprite = btn_pauseSprites[1];
            emp_PauseGo.SetActive(true);
            emp_PlayingTextGo.SetActive(false);
        }
        else
        {
            GameController.Instance.isPause = false;
            img_Btn_Pause.sprite = btn_pauseSprites[0];
            emp_PauseGo.SetActive(false);
            emp_PlayingTextGo.SetActive(true);
        }
    }

    public void ShowMenu()
    {
        normalModelPanel.ShowMenuPage();
    }
}
