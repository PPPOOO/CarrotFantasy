using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NormalGameOptionSceneState : BaseSceneState
{

    public NormalGameOptionSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
        if (gameNormalOptionPanel.isInBigLevelPanel)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        gameNormalOptionPanel.isInBigLevelPanel = true;
        base.ExitScene();
    }
}
