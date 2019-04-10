using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFacade
{
    private UIManager mUIManager;
    private GameManager mGameManager;
    private AudioSourceManager mAudioSourceManager;
    public PlayerManager mPlayerManager;

    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();

    private GameObject mask;
    private Image maskImage;
    public Transform canvasTransform;

    public IBaseSceneState currentSceneState;
    public IBaseSceneState lastSceneState;

    public UIFacade(UIManager uiManager)
    {
        mGameManager = GameManager.Instance;
        mPlayerManager = mGameManager.playerManager;
        mAudioSourceManager = mGameManager.audioSourceManager;
        mUIManager = uiManager;
        InitMask();
    }

    public void ChangeSceneState(IBaseSceneState baseSceneState)
    {
        lastSceneState = currentSceneState;
        ShowMask();
        currentSceneState = baseSceneState;
    }

    public void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 1), 2f);
        t.OnComplete(ExitSceneComplete);
    }

    private void ExitSceneComplete()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }

    public void HideMask()
    {
        DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 1f);
        mask.transform.SetSiblingIndex(10);
    }


    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        mask = CreateUIAndSetUIPos( "Img_Mask");
        maskImage = mask.GetComponent<Image>();
    }



    public void InitDict()
    {
        foreach (var item in mUIManager.currentScenePanelDict)
        {
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if (basePanel==null)
            {
                Debug.Log("获取Ibasepanel脚本失败,该物体为"+ item.Value.name);
            }
            basePanel.InitPanel();
            currentScenePanelDict.Add(item.Key,basePanel);
        }
    }

    public void ClearDict()
    {
        currentScenePanelDict.Clear();
        mUIManager.ClearDict();
    }

    public void AddPanelToDict(string uiPanelName)
    {
        mUIManager.currentScenePanelDict.Add(uiPanelName, GetGameOjectResource(FactoryType.UIPanelFactory, uiPanelName));
    }

    public GameObject CreateUIAndSetUIPos(string uiName)
    {
        GameObject itemGo = GetGameOjectResource(FactoryType.UIFactory, uiName);
        itemGo.transform.SetParent(canvasTransform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }


    public GameObject GetGameOjectResource(FactoryType factoryType, string resourcePath)
    {
        return mGameManager.GetGameOjectResource(factoryType, resourcePath);
    }
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath, GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(factoryType, resourcePath,itemGo);
    }

    public void CloseOrOpenBGMusic()
    {
        mAudioSourceManager.CloseOrOpenBGMusic();
    }
    public void CloseOrOpenEffectMusic()
    {
        mAudioSourceManager.CloseOrOpenEffectMusic();
    }

    public void PlayButtonAudioClip()
    {
        mAudioSourceManager.PlayButtonAudioClip();
    }

    public void PlayPagingAudioClip()
    {
        mAudioSourceManager.PlayPagingAudioClip();
    }
}
