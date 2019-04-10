using UnityEngine;
using System.Collections;

public class GameNormalOptionPanel : BasePanel
{
    public bool isInBigLevelPanel = true;

    public void ReturnToLastPanel()
    {
        if (isInBigLevelPanel)
        {
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        }
        else
        {
            mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        isInBigLevelPanel = true;
        mUIFacade.PlayButtonAudioClip();
    }

    public void ToHelpPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }

}
