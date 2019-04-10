using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MonsterNestSceneState : BaseSceneState
{


    public MonsterNestSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.MonsterNestPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/BGMusic02"));
    }
    public override void ExitScene()
    {
        SceneManager.LoadScene(1);
        base.ExitScene();
    }
}
