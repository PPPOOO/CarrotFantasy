﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrizePage : MonoBehaviour
{
    private Image img_Prize;
    private Image img_Instruction;
    private Text tex_PrizeName;
    private Animator animator;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        animator = GetComponent<Animator>();
        img_Prize = transform.Find("Img_Prize").GetComponent<Image>();
        img_Instruction = transform.Find("Img_Instruction").GetComponent<Image>();
        tex_PrizeName = transform.Find("Tex_PrizeName").GetComponent<Text>();
    }

    private void OnEnable()
    {
        int randonNum = Random.Range(1, 5);
        string prizeName = "";
        if (randonNum >= 4&&GameManager.Instance.playerManager.monsterPetDataList.Count<3)
        {
            int randomEggNum = 0;
            do
            {
                randomEggNum = Random.Range(1, 4);
            }
            while (HasThePet(randomEggNum));
            MonsterPetData monsterPetData = new MonsterPetData
            {
                monsterLevel = 1,
                remainCookies = 0,
                remainMilk = 0,
                monsterID = randomEggNum
            };
            GameManager.Instance.playerManager.monsterPetDataList.Add(monsterPetData);
            prizeName = "宠物蛋";
        }
        else
        {
            switch (randonNum)
            {
                case 1:
                    prizeName = "牛奶";
                    GameManager.Instance.playerManager.milk += 20;
                    break;
                case 2:
                    prizeName = "饼干"; GameManager.Instance.playerManager.cookies += 20;
                    break;
                case 3:
                    prizeName = "窝"; GameManager.Instance.playerManager.nest += 1;
                    break;
            }
        }
        tex_PrizeName.text = prizeName;
        img_Instruction.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Instruction" + randonNum);
        img_Prize.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Prize" + randonNum);
        animator.Play("Enter");
    }


    private bool HasThePet(int monsterID)
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID==monsterID)
            {
                return true;
            }
        }
        return false;
    }

    public void ClosePrizePage()
    {
        normalModelPanel.ClosePrizePage();
        GameController.Instance.isPause = false;
    }
}
