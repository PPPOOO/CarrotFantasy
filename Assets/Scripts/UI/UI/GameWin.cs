using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameWin : MonoBehaviour
{
    private Text tex_RoundCount;
    private Text tex_TotalCount;
    private Text tex_CurrentLevel;
    private Image img_Carrot;
    private NormalModelPanel normalModelPanel;
    public Sprite[] carrotSprites;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_RoundCount = transform.Find("Tex_RoundText").GetComponent<Text>();
        tex_TotalCount = transform.Find("Tex_TotalCount").GetComponent<Text>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
        img_Carrot = transform.Find("Img_Carrot").GetComponent<Image>();
        carrotSprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            carrotSprites[i] = GameController.Instance.GetSprite("GameOption/Normal/Level/Carrot_" + (i + 1).ToString());
        }
    }
    private void OnEnable()
    {
        tex_TotalCount.text = normalModelPanel.totalRound.ToString();
        tex_RoundCount.text = GameController.Instance.level.currentRound.ToString();
        tex_CurrentLevel.text= (GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5).ToString();
        normalModelPanel.ShowRoundText(tex_RoundCount);
        if (GameController.Instance.carrotHP >= 8)
        {
            img_Carrot.sprite = carrotSprites[0];
        }
        else if(GameController.Instance.carrotHP >= 5)
        {
            img_Carrot.sprite = carrotSprites[1];
        }
        else
        {
            img_Carrot.sprite = carrotSprites[2];
        }
    }

    public void Replay()
    {
        normalModelPanel.Replay();
    }
    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}
