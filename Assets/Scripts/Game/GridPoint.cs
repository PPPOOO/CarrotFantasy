using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;


public class GridPoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

#if Tool
    private Sprite monsterPathSprite;
    public GameObject[] itemPrefabs;
    public GameObject currentItem;
#endif
    [System.Serializable]
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }
    [System.Serializable]
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    private GameController gameController;
    private GameObject towerListGo;
    public GameObject handleTowerCanvasGo;
    private Transform upLevelButtonTrans;
    private Transform sellTowerButtonTrans;
    private Vector3 upLevelButtonInitPos;
    private Vector3 sellTowerButtonInitPos;

    public GridState gridState;
    public GridIndex gridIndex;
    public bool hasTower;

    private Sprite gridSprite;
    private Sprite startSprite;
    private Sprite cantBulidSprite;


    public GameObject towerGo;
    public Tower tower;
    public TowerPersonalProperty towerPersonalProperty;
    public int towerLevel;
    private GameObject levelUpSignalGo;

    private void Awake()
    {
#if Tool
        
        gridSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/1/Monster/6-1");
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID+"/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败,路径是" + prefabsPath + i);
            }
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
#if Game
        gameController = GameController.Instance;
        gridSprite = gameController.GetSprite("NormalModel/Game/Grid");
        startSprite = gameController.GetSprite("NormalModel/Game/StartSprite");
        cantBulidSprite = gameController.GetSprite("NormalModel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0.2f), 3);
        t.OnComplete(ChangeSpriteToGrid);
        towerListGo = gameController.towerListGo;
        handleTowerCanvasGo = gameController.handleTowerCanvas;
        upLevelButtonTrans = handleTowerCanvasGo.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = handleTowerCanvasGo.transform.Find("Btn_SellTower");
        upLevelButtonInitPos = upLevelButtonTrans.localPosition;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        levelUpSignalGo = transform.Find("LevelUp").gameObject;
#endif
    }

    private void Update()
    {
        if (levelUpSignalGo != null)
        {
            if (hasTower)
            {
                if (towerPersonalProperty.upLevelPrice <= gameController.coin && towerLevel < 3)
                {
                    levelUpSignalGo.SetActive(true);
                }
                else
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
            else
            {
                if (levelUpSignalGo.activeSelf)
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
        }    
    }

    private void ChangeSpriteToGrid()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBulidSprite;

        }
    }


    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif
#if Game
        towerGo = null;
        towerPersonalProperty = null;
        hasTower = false;
#endif
    }

#if Game

    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void CreateItem()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource(GameController.Instance.mapMaker.bigLevelID + "/Item/" + gridState.itemID);
        itemGo.GetComponent<Item>().itemID = gridState.itemID;
        itemGo.transform.SetParent(GameController.Instance.transform);
        Vector3 createPos = transform.position - new Vector3(0, 0, 3);
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,-GameController.Instance.mapMaker.gridHeight)/2;
        }
        else if (gridState.itemID <= 4)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,0,0)/2;
        }
        itemGo.transform.position = createPos;
        itemGo.GetComponent<Item>().gridPoint = this;
    }




    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        gameController.HandleGrid(this);
    }

    public void ShowGrid()
    {
        if (!hasTower)
        {
            spriteRenderer.enabled = true;
            towerListGo.transform.position = CorrectTowerListGoPos();
            towerListGo.SetActive(true);
        }
        else
        {
            handleTowerCanvasGo.transform.position = transform.position;
            CorrtectHandledTowerCanvasGoPos();
            handleTowerCanvasGo.SetActive(true);
            handleTowerCanvasGo.transform.GetChild(1).SendMessage("UpdateUI");
            tower.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }

    public void HideGrid()
    {
        if (!hasTower)
        {
            towerListGo.SetActive(false);
        }
        else
        {
            handleTowerCanvasGo.SetActive(false);
            tower.transform.Find("attackRange").gameObject.SetActive(false);
        }
        spriteRenderer.enabled = false;
    }

    public void ShowCanBuild()
    {
        spriteRenderer.enabled = true;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 2);
        t.OnComplete(() =>
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        });
    }

    private Vector3 CorrectTowerListGoPos()
    {
        Vector3 correctPos = Vector3.zero;
        if(gridIndex.xIndex<=2&& gridIndex.xIndex >= 0)
        {
            correctPos += new Vector3(gameController.mapMaker.gridWidth,0,0);
        }
        else if(gridIndex.xIndex <= 11 && gridIndex.xIndex >= 9)
        {
            correctPos -= new Vector3(gameController.mapMaker.gridWidth, 0, 0);
        }
        if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)
        {
            correctPos += new Vector3(0,gameController.mapMaker.gridHeight, 0);
        }
        else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)
        {
            correctPos -= new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }
        correctPos = transform.position + correctPos;
        return correctPos;
    }

    private void CorrtectHandledTowerCanvasGoPos()
    {
        upLevelButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex<=0)
        {
            if (gridIndex.xIndex == 0)
            {
                sellTowerButtonTrans.position+= new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                sellTowerButtonTrans.position-= new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
        }
        else if(gridIndex.yIndex >= 6)
        {
            if (gridIndex.xIndex == 0)
            {
                upLevelButtonTrans.position+= new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                upLevelButtonTrans.position -= new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
        else
        {
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;

        }
    }

    public void AfterBuild()
    {
        spriteRenderer.enabled = false;
        towerGo = transform.GetChild(1).gameObject;
        tower = towerGo.GetComponent<Tower>();
        towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
        towerLevel = towerPersonalProperty.levelTower;
    }

#endif
#if Tool
    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.P))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        else if (Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            if (gridState.itemID == itemPrefabs.Length)
            {
                gridState.itemID = -1;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem == null)
            {
                CreateItem();
            }
            else
            {
                Destroy(currentItem);
                CreateItem();

            }
            gridState.hasItem = true;
        }
        else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    private void CreateItem()
    {
        Vector3 createPos = transform.position;
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth, -MapMaker.Instance.gridHeight) / 2;
        }
        else if (gridState.itemID <= 4)
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth,0) / 2;

        }
        GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID], createPos, Quaternion.identity);
        currentItem = itemGo;
    }

    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
            
        }
    }
#endif
}
