using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public GridPoint gridPoint;
    public int itemID;
    private GameController gameController;
    private int prize;
    private int HP;
    private int currentHP;
    private Slider slider;

    private float timeVal;
    private bool showHP=true;

    private void OnEnable()
    {
        if (itemID != 0)
        {
#if Game
            InitItem();
#endif
        }
        gameController = GameController.Instance;
    }

    private void Start()
    {
#if Tool
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Mask").GetComponent<BoxCollider>().enabled = false;
#endif
        gameController = GameController.Instance;
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
#if Game
            InitItem();
#endif
    }

    private void Update()
    {
        if (showHP)
        {
            if (timeVal <= 0)
            {
                slider.gameObject.SetActive(false);
                showHP = false;
                timeVal = 3;
            }
            else
            {
                timeVal -= Time.deltaTime;
            }
        }
    }

#if Game

    public void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            DestroyItem();
            return;
        }
        slider.value = (float)currentHP / HP;
        showHP = true;
        timeVal = 3;
    }
    private void DestroyItem()
    {
        if (gameController.targetTrans == transform)
        {
            gameController.HideSignal();
        }
        GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas").gameObject;
        coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
        coinGo.transform.SetParent(gameController.transform);
        coinGo.transform.position = transform.position;
        GameController.Instance.ChangeCoin(prize);

        GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect").gameObject;
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;

        gameController.PushGameObjectToFactory(gameController.mapMaker.bigLevelID + "/Item/" + itemID,gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();
        gameController.PlayEffectMusic("NormalModel/Item");
    }

    private void InitItem()
    {
        prize = 1000 - 100 * itemID;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        timeVal = 3;
    }


    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (gameController.targetTrans == null)
        {
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans != transform)
        {
            gameController.HideSignal();
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans == transform)
        {
            gameController.HideSignal();
        }
    }
#endif
}
