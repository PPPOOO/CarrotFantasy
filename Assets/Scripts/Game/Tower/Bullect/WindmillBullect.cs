using UnityEngine;
using System.Collections;

public class WindmillBullect : Bullect
{
    private bool hasTarget;
    private float timeVal;

    private void OnEnable()
    {
        hasTarget = false;
        timeVal = 0;
    }

    private void InitTarget()
    {
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
    }

    void Update()
    {
        if (GameController.Instance.gameOver || timeVal >= 2.5f)
        {
            DestroyBullect();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (timeVal < 2.5f)
        {
            timeVal += Time.deltaTime;
        }
        if (hasTarget)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime * GameController.Instance.gameSpeed,Space.World);
        }
        else
        {
            if (targetTrans != null && targetTrans.gameObject.activeSelf)
            {
                hasTarget = true;
                InitTarget();
            }
        }
    }



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (targetTrans.position == null || (collision.tag == "Item" && GameController.Instance.targetTrans == null))
            {
                return;
            }
            if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.Instance.targetTrans == collision.transform))
            {
                if (collision.gameObject.activeSelf)
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }

            }
        }
    }
}
