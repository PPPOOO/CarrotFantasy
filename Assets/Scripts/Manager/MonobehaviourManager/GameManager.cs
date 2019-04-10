using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerManager playerManager;
    public FactoryManager factoryManager;
    public AudioSourceManager audioSourceManager;
    public UIManager uiManager;


    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Stage currentStage;
    public bool initPlayerManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        playerManager = new PlayerManager();
        //playerManager.SaveData();
        playerManager.ReadData();
        factoryManager = new FactoryManager();
        audioSourceManager = new AudioSourceManager();
        uiManager = new UIManager();
        uiManager.mUIFacade.currentSceneState.EnterScene();
    }


    public GameObject CreateItem(GameObject itemGo)
    {
        GameObject go = Instantiate(itemGo);
        return go;
    }

    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }
    public AudioClip GetAudioClip (string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return factoryManager.runtimeAnimatiorControllerFactory.GetSingleResources(resourcePath);
    }

    public GameObject GetGameOjectResource(FactoryType factoryType,string resourcePath)
    {
        return factoryManager.factoryDict[factoryType].GetItem(resourcePath);
    }
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath,GameObject itemGo)
    {
        factoryManager.factoryDict[factoryType].PushItem(resourcePath, itemGo);
    }
}
