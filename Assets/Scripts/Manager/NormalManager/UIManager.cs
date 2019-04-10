using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public UIFacade mUIFacade;
    public Dictionary<string, GameObject> currentScenePanelDict = new Dictionary<string, GameObject>();
    private GameManager mGameManager;


    public UIManager()
    {
        mGameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        mUIFacade.currentSceneState = new StartLoadSceneState(mUIFacade);

    }

    private void PushPanel(string uiPanelName,GameObject uiPanelGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, uiPanelName, uiPanelGo);
    }

    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            PushPanel(item.Value.name.Substring(0,item.Value.name.Length-7), item.Value);
        }
        currentScenePanelDict.Clear();
    }
}
