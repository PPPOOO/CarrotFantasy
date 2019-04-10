using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public Level level;
    private GameManager mGameManager;
    public int[] mMonsterIDList;
    public int mMonsterIDIndex;
    public Stage currentStage;
    public MapMaker mapMaker;
    public Transform targetTrans;
    public GameObject targetSignal;
    public GridPoint selectGrid;

    public NormalModelPanel normalModelPanel;

    public RuntimeAnimatorController[] contorllers;

    public int killMonsterNum;
    public int clearItemNum;
    public int killMonsterTotal;


    public int carrotHP = 10;
    public int coin;
    public int gameSpeed;
    public bool isPause;
    public bool creatingMonster;
    public bool gameOver;
    public bool isWin=false;


    public MonsterBuilder monsterBuilder;
    public TowerBuild towerBuild;

    public Dictionary<int, int> towerPriceDict;
    public GameObject towerListGo;
    public GameObject handleTowerCanvas;




    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        //currentStage = new Stage(10, 5, new int[] { 1,2,3,4,5}, false, 0, 1, 1, true, false);
        currentStage = mGameManager.currentStage;
        normalModelPanel = mGameManager.uiManager.mUIFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        normalModelPanel.EnterPanel();
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.mBigLevelID, currentStage.mLevelID);
        gameSpeed = 1;
        coin = 1000;
        monsterBuilder = new MonsterBuilder();
        towerBuild = new TowerBuild();

        for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
        {
            GameObject itemGo = mGameManager.GetGameOjectResource(FactoryType.UIFactory, "Btn_TowerBuild");
            itemGo.transform.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
            itemGo.transform.SetParent(towerListGo.transform);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
        }

        towerPriceDict = new Dictionary<int, int>
        {
            {1,100 },
            {2,120 },
            {3,140 },
            {4,160 },
            {5,160 },
        }; 

        contorllers = new RuntimeAnimatorController[12];
        for (int i = 0; i < contorllers.Length; i++)
        {
            contorllers[i] = GetRuntimeAnimatorController("Monster/" + mapMaker.bigLevelID + "/" + (i + 1));
        }
        level = new Level(mapMaker.roundInfoList.Count, mapMaker.roundInfoList);
        //level.HandleRound();
        normalModelPanel.topPage.UpdateCoinText();
        normalModelPanel.topPage.UpdateRoundText();
        isPause = true;
#endif
    }

    private void Update()
    {
#if Game
        if (!isPause)
        {
            if (level.currentRound >= level.totalRound||isWin)
            {
                return;
            }
            if (killMonsterNum >= mMonsterIDList.Length)
            {
                AddRoundNum();
            }
            else
            {
                if (!creatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            StopCreateMonster();
            creatingMonster = false;
        }
#endif
    }


    public void ShowSignal()
    {
        PlayEffectMusic("NormalModel/Tower/ShootSelect" );
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.gridHeight/2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }

    public void HideSignal()
    {
        targetSignal.gameObject.SetActive(false);
        targetTrans = null;
    }

    public void CreateMonster()
    {
        creatingMonster = true;
        InvokeRepeating("InstantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
    }

    private void InstantiateMonster()
    {
        PlayEffectMusic("NormalModel/Monster/Create");
        if (mMonsterIDIndex >= mMonsterIDList.Length)
        {
            StopCreateMonster();
            return;
        }
        GameObject effectGo = GetGameObjectResource("CreateEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];

        if (mMonsterIDIndex < level.roundList[level.currentRound].roundInfo.mMonsterIDList.Length)
        {
            monsterBuilder.m_monsterID = level.roundList[level.currentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
        }
        GameObject monsterGo = monsterBuilder.GetProduct();
        monsterGo.transform.SetParent(transform);
        monsterGo.transform.position = mapMaker.monsterPathPos[0];
        mMonsterIDIndex++;
        
    }

    public void StopCreateMonster()
    {
        CancelInvoke();
    }

    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
        normalModelPanel.UpdatePanel();
    }


    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        normalModelPanel.UpdatePanel();
    }

    public void DecreaseHP()
    {
        PlayEffectMusic("NormalModel/Carrot/Crash");
        carrotHP--;
        mapMaker.carrot.UpdateCarrotUI();
    }

    public bool IfAllClear()
    {
        for(int x = 0; x < MapMaker.xColumn; x++)
        {
            for(int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x, y].gridState.hasItem)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public int GetCarrotState()
    {
        int carrotState = 0;
        if (carrotHP >= 8)
        {
            carrotState = 1;
        }
        else if (carrotHP >= 5)
        {
            carrotState = 2;
        }
        else
        {
            carrotState = 3;
        }
        return carrotState;
    }

#if Game

    public void ShowPrizePage()
    {
        normalModelPanel.ShowPrizePage();
    }

    public void StartGame()
    {
        isPause = false;
        level.HandleLastRound();
    }


    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canBuild)
        {
            if (selectGrid == null)
            {
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalModel/Grid/GridSelect");
            }
            else if(grid == selectGrid)
            {
                grid.HideGrid();
                selectGrid = null;
                PlayEffectMusic("NormalModel/Grid/GridDeselect");
            }
            else if (grid != selectGrid)
            {
                selectGrid.HideGrid();
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalModel/Grid/GridSelect");
            }
        }
        else
        {
            grid.HideGrid();
            grid.ShowCanBuild();
            PlayEffectMusic("NormalModel/Grid/SelectFault");
            if (selectGrid != null)
            {
                selectGrid.HideGrid();
            }
        }
    }
#endif




    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudio(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }
    public GameObject GetGameObjectResource(string resourcePath)
    {
        return mGameManager.GetGameOjectResource(FactoryType.GameFactory, resourcePath);
    }
    public void PushGameObjectToFactory(string resourcePath,GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.GameFactory, resourcePath, itemGo);
    }

    public void PlayEffectMusic(string audioClipPath)
    {
        mGameManager.audioSourceManager.PlayEffectMusic(GetAudio(audioClipPath));
    }

}
