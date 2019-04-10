using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainPanel : BasePanel
{
    private Animator carrotAnimator;
    private Transform monsterTrans;
    private Transform cloudTrans;
    private Tween[] mainPanelTween;
    private Tween exitTween;



    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(8);
        carrotAnimator = transform.Find("Emp_Carrot").GetComponent<Animator>();
        carrotAnimator.Play("CarrotGrow");
        monsterTrans = transform.Find("Img_Monster");
        cloudTrans = transform.Find("Img_Cloud");
        mainPanelTween = new Tween[2];
        mainPanelTween[0] = transform.DOLocalMoveX(1920, 0.5f);
        mainPanelTween[0].SetAutoKill(false);
        mainPanelTween[0].Pause();
        mainPanelTween[1] = transform.DOLocalMoveX(-1920, 0.5f);
        mainPanelTween[1].SetAutoKill(false);
        mainPanelTween[1].Pause();
        PlayUITween();
    }

    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8);
        carrotAnimator.Play("CarrotGrow");
        if (exitTween!=null)
        {
            exitTween.PlayBackwards();
        }
        cloudTrans.gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        exitTween.PlayForward();
        cloudTrans.gameObject.SetActive(false);
    }

    private void PlayUITween()
    {
        monsterTrans.DOLocalMoveY(550, 2).SetLoops(-1,LoopType.Yoyo);
        cloudTrans.DOLocalMoveX(1300, 8).SetLoops(-1, LoopType.Restart);
    }

    public void MoveToRight()
    {
        mUIFacade.PlayButtonAudioClip();
        exitTween = mainPanelTween[0];
        mUIFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
    }

    public void MoveToLeft()
    {
        mUIFacade.PlayButtonAudioClip();
        exitTween = mainPanelTween[1];
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }

    public void ToNormalModelScene()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }
    public void ToBossModelScene()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new BossGameOptionSceneState(mUIFacade));
    }
    public void ToMonsterNestScene()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new MonsterNestSceneState(mUIFacade));
    }
    public void ToExitGame()
    {
        mUIFacade.PlayButtonAudioClip();
        GameManager.Instance.playerManager.SaveData();
        Application.Quit();
    }
}
