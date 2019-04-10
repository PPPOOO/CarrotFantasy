﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState
{

    public MainSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.MainPanel);
        mUIFacade.AddPanelToDict(StringManager.SetPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);

        base.EnterScene();
        GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/BGMusic"));
    }

    public override void ExitScene()
    {
        base.ExitScene();
        if (mUIFacade.currentSceneState.GetType()==typeof(NormalGameOptionSceneState))
        {
            SceneManager.LoadScene(2);
        }
        else if(mUIFacade.currentSceneState.GetType() == typeof(BossGameOptionSceneState))
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }
}
