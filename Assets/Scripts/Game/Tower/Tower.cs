using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{

    public int towerID;
    private CircleCollider2D circleCollider2D;
    private TowerPersonalProperty towerPersonalProperty;
    private SpriteRenderer attackRangeSR;
    public bool isTarget;
    public bool hasTarget;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        towerPersonalProperty.tower = this;
        attackRangeSR = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.gameObject.SetActive(false);
        circleCollider2D.radius = towerPersonalProperty.levelTower*0.5f+2;
        attackRangeSR.transform.localScale = Vector3.one * circleCollider2D.radius;
        
        isTarget = false;
        hasTarget = false;
    }

    private void Update()
    {
        if (GameController.Instance.isPause || GameController.Instance.gameOver)
        {
            return;
        }
        if (isTarget)
        {
            if (towerPersonalProperty.targetTrans!=GameController.Instance.targetTrans)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
        if (hasTarget)
        {
            if (!towerPersonalProperty.targetTrans.gameObject.activeSelf)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
    }

    public void GetTowerProperty()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Monster" && collision.tag != "Item" && isTarget)
        {
            return;
        }
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null)
        {
            if (!isTarget)
            {
                if (collision.tag == "Item" && collision.transform == targetTrans)
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;
                    hasTarget = true;
                }
                else if (collision.tag == "Monster")
                {
                    if (collision.transform == targetTrans)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    else if (collision.transform != targetTrans&&!hasTarget)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else
        {
            if (!hasTarget && collision.tag == "Monster")
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Monster" && collision.tag != "Item" && isTarget)
        {
            return;
        }
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null)
        {
            if (!isTarget)
            {
                if (collision.tag == "Item" && collision.transform == targetTrans)
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;
                    hasTarget = true;
                }
                else if (collision.tag == "Monster")
                {
                    if (collision.transform == targetTrans)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    else if (collision.transform != targetTrans && !hasTarget)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else
        {
            if (!hasTarget && collision.tag == "Monster")
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (towerPersonalProperty.targetTrans==collision.transform)
        {
            towerPersonalProperty.targetTrans = null;
            hasTarget = false;
            isTarget = false;
        }
    }

    public void DestroyTower()
    {
        towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID + "/TowerSet/" + towerPersonalProperty.levelTower,gameObject);
    }
}
