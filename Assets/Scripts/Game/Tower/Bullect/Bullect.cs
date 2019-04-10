using UnityEngine;
using System.Collections;

public class Bullect : MonoBehaviour
{
    public Transform targetTrans;
    public int moveSpeed;
    public int attackValue;
    public int towerID;
    public int towerLevel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameController.Instance.gameOver)
        {
            DestroyBullect();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans == null||!targetTrans.gameObject.activeSelf)
        {
            DestroyBullect();
            return;
        }

        if (targetTrans.gameObject.tag == "Item")
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3), 1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position , 1 / Vector3.Distance(transform.position, targetTrans.position ) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position );
        }
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }

    }

    protected virtual void DestroyBullect()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID + "/Bullect/" + towerLevel, gameObject);

    }

    protected virtual void CreateEffect()
    {
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + towerID + "/Effect/" + towerLevel);
        effectGo.transform.position = transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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
                        DestroyBullect();
                    }
                   
                }
        }
    }
}
