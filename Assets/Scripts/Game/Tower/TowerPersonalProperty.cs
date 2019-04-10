using UnityEngine;
using System.Collections;

public class TowerPersonalProperty : MonoBehaviour
{
    //属性值
    public int levelTower;
    public int price;
    protected float timeVal;
    public float attackCD;
    public int sellPrice;
    public int upLevelPrice;

    //资源
    protected GameObject bullectGo;

    //引用
    public Tower tower;
    public Transform targetTrans;//攻击目标
    public Animator animator;
    private GameController gameController;

    public virtual void OnEnable()
    {
        gameController = GameController.Instance;
    }

    protected  virtual void Start()
    {
        gameController = GameController.Instance;
        upLevelPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
    }


    protected virtual void Update()
    {
        
        if (gameController.isPause || targetTrans == null || GameController.Instance.gameOver)
        {
            return;
        }
        if (!targetTrans.gameObject.activeSelf)
        {
            targetTrans = null;
            return;
        }
        if (timeVal >= attackCD / gameController.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
        //transform.LookAt(targetTrans);
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.LookAt(targetTrans.position);
        }

        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
    }

    public void Init()
    {
        tower = null;
    }
    public void SellTower()
    {
        gameController.PlayEffectMusic("NormalModel/Tower/TowerSell");
        gameController.ChangeCoin(sellPrice);
        gameController.GetGameObjectResource("BuildEffect");
        DestroyTower();
    }
    public void UpLevelTower()
    {
        gameController.PlayEffectMusic("NormalModel/Tower/TowerUpdata");
        gameController.ChangeCoin(-upLevelPrice);
        GameObject effectGo= gameController.GetGameObjectResource("UpLevelEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = transform.position;
        DestroyTower();
    }

    protected virtual void DestroyTower()
    {
        
        tower.DestroyTower();
    }
    protected virtual void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        animator.Play("Attack");
        gameController.PlayEffectMusic("NormalModel/Tower/Attack/"+tower.towerID);
        GameObject bullectGo = gameController.GetGameObjectResource("Tower/ID" + tower.towerID + "/Bullect/" + levelTower);
        bullectGo.transform.position = transform.position;
        bullectGo.GetComponent<Bullect>().targetTrans = targetTrans;
    }
}
