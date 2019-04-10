using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartLoadSceneState : BaseSceneState
{
    
    public StartLoadSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.StartLoadPanel);

        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(1);
    }
}
