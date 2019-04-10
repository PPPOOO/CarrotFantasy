using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalModelPanel : BasePanel
{
    //页面
    private GameObject topPageGo;
    private GameObject gameOverPageGo;
    private GameObject gameWinPageGo;
    private GameObject menuPageGo;
    private GameObject img_FinalWave;
    private GameObject img_StartUIGo;
    private GameObject prizePageGo;

    public int totalRound;

    public TopPage topPage;
    private GameController gameController;



    protected override void Awake()
    {
        base.Awake();
        gameController = GameController.Instance;
        transform.SetSiblingIndex(1);
        topPageGo = transform.Find("Img_TopPage").gameObject;
        gameWinPageGo = transform.Find("GameWinPage").gameObject;
        gameOverPageGo = transform.Find("GameOverPage").gameObject;
        menuPageGo = transform.Find("MenuPage").gameObject;
        prizePageGo = transform.Find("PrizePage").gameObject;
        img_FinalWave = transform.Find("Img_FinalWave").gameObject;
        img_StartUIGo = transform.Find("StartUI").gameObject;
        topPage = topPageGo.GetComponent<TopPage>();
    }

    private void OnEnable()
    {
        InvokeRepeating("PlayAudio", 0.5f, 1);
        Invoke("StartGame", 3.5f);
    }

   

    private void PlayAudio()
    {
        img_StartUIGo.SetActive(true);
        GameController.Instance.PlayEffectMusic("NormalModel/CountDown");
    }

    private void StartGame()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Go");
#if Game
        gameController.StartGame();
#endif
        img_StartUIGo.SetActive(false);
        CancelInvoke();

    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameController = GameController.Instance;
        totalRound = gameController.currentStage.mTotalRound;
        topPageGo.SetActive(true);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        topPage.UpdateCoinText();
        topPage.UpdateRoundText();
    }

    public void ShowPrizePage()
    {
        gameController.isPause = true;
        prizePageGo.SetActive(true);
    }

    public void ClosePrizePage()
    {
        mUIFacade.PlayButtonAudioClip();
        gameController.isPause = false;
        prizePageGo.SetActive(false);
    }

    public void ShowMenuPage()
    {
        mUIFacade.PlayButtonAudioClip();
        gameController.isPause = true;
        menuPageGo.SetActive(true);
    }

    public void CloseMenuPage()
    {
        mUIFacade.PlayButtonAudioClip();
        gameController.isPause = false;
        menuPageGo.SetActive(false);
    }

    public void ShowGameWinPage()
    {
        Stage stage = GameManager.Instance.playerManager.unLockedNormalModelLevelList[gameController.currentStage.mLevelID - 1 + (gameController.currentStage.mBigLevelID - 1) * 5];
        if (gameController.IfAllClear())
        {
            stage.mAllClear = true;
        }
        int carrotState = gameController.GetCarrotState();
        if (carrotState != 0&&stage.mCarrotState!=0)
        {
            if (stage.mCarrotState > carrotState)
            {
                stage.mCarrotState = carrotState;
            }
        }
        else if (stage.mCarrotState == 0)
        {
            stage.mCarrotState = carrotState;
        }
        if (gameController.currentStage.mLevelID%5!=0&& gameController.currentStage.mLevelID - 1 + (gameController.currentStage.mBigLevelID - 1) * 5<GameManager.Instance.playerManager.unLockedNormalModelLevelList.Count)
        {
            GameManager.Instance.playerManager.unLockedNormalModelLevelList[gameController.currentStage.mLevelID + (gameController.currentStage.mBigLevelID - 1) * 5].unLocked=true;
        }

        UpdatePlayerManagerData();
        gameWinPageGo.SetActive(true);
        gameController.gameOver = false;
        GameManager.Instance.playerManager.adventrueModelNum++;
        GameController.Instance.PlayEffectMusic("NormalModel/Perfect");
    }

    public void ShowGameOverPage()
    {
        UpdatePlayerManagerData();
        gameOverPageGo.SetActive(true);
        gameController.gameOver = false;
        GameController.Instance.PlayEffectMusic("NormalModel/Lose");
    }
    

    public void ShowRoundText(Text roundText)
    {
        int roundNum = gameController.level.currentRound+1;
        string roundStr = "";
        if (roundNum < 10)
        {
            roundStr = "0  " + roundNum;
        }
        else
        {
            roundStr = roundNum / 10 + "  " + roundNum % 10
;        }
        roundText.text = roundStr;
    }

    public void ShowFinalWaveUI()
    {
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI",0.508f);
        GameController.Instance.PlayEffectMusic("NormalModel/Finalwave");
    }
    
    private void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
    }

    private void UpdatePlayerManagerData()
    {
        GameManager.Instance.playerManager.coin += gameController.coin;
        GameManager.Instance.playerManager.killMonsterNum += gameController.killMonsterTotal;
        GameManager.Instance.playerManager.clearItemNum += gameController.clearItemNum;
    }

    public void Replay()
    {
        mUIFacade.PlayButtonAudioClip();
        UpdatePlayerManagerData();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
        Invoke("SetPauseFalse", 2);
        Invoke("ResetGame",2);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(3);
        ResetUI();
        gameObject.SetActive(true);
    }


    public void ResetUI()
    {
        gameOverPageGo.SetActive(false);
        gameWinPageGo.SetActive(false);
        menuPageGo.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ChooseOtherLevel()
    {
        mUIFacade.PlayButtonAudioClip();
        Invoke("SetPauseFalse", 2);
        UpdatePlayerManagerData();
        Invoke("ToOtherScene", 2);
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }

    public void ToOtherScene()
    {
        Invoke("SetPauseFalse", 2);
        ResetUI();
        SceneManager.LoadScene(2);
    }

    private void SetPauseFalse()
    {
        gameController.gameOver = false;
    }
}
