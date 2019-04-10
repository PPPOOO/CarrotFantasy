using UnityEngine;
using System.Collections;

public class TShit : TowerPersonalProperty
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (GameController.Instance.isPause||targetTrans==null || GameController.Instance.gameOver)
        {
            return;
        }
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

}
