using UnityEngine;
using System.Collections;

public class NormalModelSceneState : BaseSceneState
{


    public NormalModelSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.NormalModelPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.CloseBGMusic();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        GameManager.Instance.audioSourceManager.OpenBGMusic();
    }
}
