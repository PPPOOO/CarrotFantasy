using UnityEngine;
using System.Collections;

public class StartLoadPanel : BasePanel
{

    protected override void Awake()
    {
        base.Awake();
        Invoke("LoadNextScene", 2);
    }

    private void LoadNextScene()
    {
        mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
    }

}
