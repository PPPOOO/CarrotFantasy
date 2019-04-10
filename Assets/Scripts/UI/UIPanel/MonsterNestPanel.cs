﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class MonsterNestPanel : BasePanel
{
    private GameObject shopPanelGo;
    private Text tex_CookiesNum;
    private Text tex_Milk;
    private Text tex_Nest;
    private Text tex_DiamandsNum;
    private List<GameObject> monsterPetGoList;
    private Transform Emp_MonsterGroupTrans;

    protected override void Awake()
    {
        base.Awake();
        shopPanelGo = transform.Find("ShopPage").gameObject;
        tex_CookiesNum = transform.Find("Img_TopPage").Find("Tex_Cookies").GetComponent<Text>();
        tex_Milk = transform.Find("Img_TopPage").Find("Tex_Milk").GetComponent<Text>();
        tex_Nest = transform.Find("Img_TopPage").Find("Tex_Nest").GetComponent<Text>();
        Emp_MonsterGroupTrans = transform.Find("Emp_MonsterGroup");
        tex_DiamandsNum = transform.Find("ShopPage").Find("Img_Diamands").Find("Tex_Diamands").GetComponent<Text>();
        for (int i = 1; i < 4; i++)
        {
            mUIFacade.GetSprite("MonsterNest/Monster/Egg/" + i.ToString());
            mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + i.ToString());
            mUIFacade.GetSprite("MonsterNest/Monster/Normal/" + i.ToString());
        }
        monsterPetGoList = new List<GameObject>();

    }

    public override void InitPanel()
    {
        base.InitPanel();

        for (int i = 0; i < monsterPetGoList.Count; i++)
        {
            mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Emp_Monsters", monsterPetGoList[i]);
        }
        monsterPetGoList.Clear();
        for (int i = 0; i < mUIFacade.mPlayerManager.monsterPetDataList.Count; i++)
        {
            if (mUIFacade.mPlayerManager.monsterPetDataList[i].monsterID != 0)
            {
                GameObject monsterPetGo = mUIFacade.GetGameOjectResource(FactoryType.UIFactory, "Emp_Monsters");
                monsterPetGo.GetComponent<MonsterPet>().monsterPetData = mUIFacade.mPlayerManager.monsterPetDataList[i];
                monsterPetGo.GetComponent<MonsterPet>().monsterNestPanel = this;
                monsterPetGo.GetComponent<MonsterPet>().InitMonsterPet();
                monsterPetGo.transform.SetParent(Emp_MonsterGroupTrans);
                monsterPetGo.transform.localScale = Vector3.one;
                monsterPetGoList.Add(monsterPetGo);
            }
        }
        UpdateText();
    }

    //topPanel
    public void ShowShopPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        shopPanelGo.SetActive(true);
    }

    public void CloseShopPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        shopPanelGo.SetActive(false);
    }

    public void ReturnToMain()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
    }

    //更新文本
    public void UpdateText()
    {
        tex_CookiesNum.text = GameManager.Instance.playerManager.cookies.ToString();
        tex_Milk.text = GameManager.Instance.playerManager.milk.ToString();
        tex_Nest.text = GameManager.Instance.playerManager.nest.ToString();
        tex_DiamandsNum.text = GameManager.Instance.playerManager.diamands.ToString();
    }

    //ShopPage
    public void BuyNest()
    {
        if (GameManager.Instance.playerManager.diamands >= 60)
        {
            GameManager.Instance.playerManager.diamands -= 60;
            mUIFacade.PlayButtonAudioClip();
        }
        GameManager.Instance.playerManager.nest++;
        UpdateText();
    }

    public void BuyMilk()
    {
        if (GameManager.Instance.playerManager.diamands >= 1)
        {
            GameManager.Instance.playerManager.diamands -= 1;
            mUIFacade.PlayButtonAudioClip();
        }
        GameManager.Instance.playerManager.milk += 10;
        UpdateText();
    }

    public void BuyCookie()
    {
        if (GameManager.Instance.playerManager.diamands >= 10)
        {
            GameManager.Instance.playerManager.diamands -= 10;
            mUIFacade.PlayButtonAudioClip();
        }
        GameManager.Instance.playerManager.cookies += 15;
        UpdateText();
    }

    public void SetCanvasTrans(Transform uiTrans)
    {
        uiTrans.SetParent(mUIFacade.canvasTransform);
    }
}