using UnityEngine;
using System.Collections;

public class Crystal : TowerPersonalProperty
{

    private float distance;
    private float bullectWidth;
    private float bullectLength;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        bullectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID + "/Bullect/" + levelTower);
        bullectGo.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip= GameController.Instance.GetAudio("NormalModel/Tower/Attack/" + tower.towerID);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (animator == null)
        {
            return;
        }
        bullectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID + "/Bullect/" + levelTower);
        bullectGo.SetActive(false);
    }
    protected override void Update()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.gameOver)
        {
            if (targetTrans == null)
            {
                bullectGo.SetActive(false);
            }
            return;
        }
        Attack();
    }
    protected override void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        animator.Play("Attack");
        if (targetTrans.gameObject.tag == "Item")
        {
            distance = Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            distance = Vector3.Distance(transform.position, targetTrans.position);
        }
        bullectWidth = 3 / distance;
        bullectLength = distance / 2 ;
        bullectWidth = Mathf.Clamp(bullectWidth, 0.5f, 1);
        bullectGo.transform.position = new Vector3((targetTrans.position.x + transform.position.x) / 2, (targetTrans.position.y + transform.position.y) / 2, 0);
        bullectGo.transform.localScale = new Vector3(1, bullectWidth, bullectLength);
        bullectGo.SetActive(true);
        bullectGo.GetComponent<Bullect>().targetTrans = targetTrans;
    }

    protected override void DestroyTower()
    {
        bullectGo.SetActive(false);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID + "/Bullect/" + levelTower, bullectGo);
        bullectGo = null;

        base.DestroyTower();
    }

}
