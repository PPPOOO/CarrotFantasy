using UnityEngine;
using System.Collections;

public class GameLoadPanel : BasePanel
{
    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        transform.SetSiblingIndex(8);
    }

    //public override void ExitPanel()
    //{
    //    base.ExitPanel();
    //    gameObject.SetActive(false);
    //}
}
