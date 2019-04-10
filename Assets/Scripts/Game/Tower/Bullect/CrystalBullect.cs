using UnityEngine;
using System.Collections;

public class CrystalBullect : Bullect
{

    private float attackTimeVal;
    private bool canTakeDamage;

    protected override void Update()
    {
        if (GameController.Instance.gameOver)
        {
            DestroyBullect();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestroyBullect();
            return;
        }

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
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        if (!canTakeDamage)
        {
            attackTimeVal += Time.deltaTime;
            if (attackTimeVal >= 0.5f - towerLevel * 0.15f)
            {
                canTakeDamage = true;
                attackTimeVal = 0;
                DecreaseHP();
            }
        }
    }

    private void DecreaseHP()
    {
        if (!canTakeDamage||targetTrans==null)
        {
            return;
        }
        if (targetTrans.gameObject.activeSelf)
        {
            if (targetTrans.position == null || (targetTrans.tag == "Item" && GameController.Instance.targetTrans == null))
            {
                return;
            }
            if (targetTrans.tag == "Monster" || (targetTrans.tag == "Item" && GameController.Instance.targetTrans.gameObject.tag == "Item"))
            {
                targetTrans.SendMessage("TakeDamage", attackValue);
                CreateEffect();
                canTakeDamage = false;
            }
        }
    }

    protected override void CreateEffect()
    {
        if (targetTrans.position == null)
        {
            return;
        }
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + towerID + "/Effect/" + towerLevel);
        effectGo.transform.position = targetTrans.position;
    }
}
