using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HelpPanel : BasePanel
{
    private GameObject helpPageGo;
    private GameObject monsterPageGo;
    private GameObject towerPageGo;
    private SliderScrollView sliderScrollView;
    private SliderCanCoverScrollView sliderCanCoverScrollView;
    private Tween helpPanelTween;

    protected override void Awake()
    {
        base.Awake();
        helpPageGo = transform.Find("HelpPage").gameObject;
        monsterPageGo = transform.Find("MonsterPage").gameObject;
        towerPageGo = transform.Find("TowerPage").gameObject;
        sliderCanCoverScrollView = helpPageGo.transform.Find("Scroll View").GetComponent<SliderCanCoverScrollView>();
        sliderScrollView = towerPageGo.transform.Find("Scroll View").GetComponent<SliderScrollView>();
        helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
    }


    public void ShowHelpPage()
    {
        if (!helpPageGo.activeSelf)
        {
            mUIFacade.PlayButtonAudioClip();
            helpPageGo.SetActive(true);
        }
        
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }
    public void ShowMonsterPage()
    {
        mUIFacade.PlayButtonAudioClip();
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(true);
        towerPageGo.SetActive(false);
    }
    public void ShowTowerPage()
    {
        mUIFacade.PlayButtonAudioClip();
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(true);
    }

    public override void InitPanel()
    {
        base.InitPanel();
        
        transform.SetSiblingIndex(5);
        sliderScrollView.Init();
        sliderCanCoverScrollView.Init();
        ShowHelpPage();

        if (transform.localPosition == Vector3.zero)
        {
            gameObject.SetActive(false);
            helpPanelTween.PlayBackwards();
        }
        transform.localPosition = new Vector3(1920, 0, 0);
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        sliderScrollView.Init();
        sliderCanCoverScrollView.Init();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        base.ExitPanel();

        if (mUIFacade.currentSceneState.GetType() == typeof(NormalGameOptionSceneState))
        {
            mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
            SceneManager.LoadScene(2);
        }
        else
        {
            helpPanelTween.PlayBackwards();
            mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        }
        
    }

    public void MoveToCenter()
    {
        helpPanelTween.PlayForward();
    }

}
