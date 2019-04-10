﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SetPanel : BasePanel
{
    private GameObject optionPageGo;
    private GameObject statisticsPageGo;
    private GameObject producerPageGo;
    private GameObject Panel_ResetGo;
    private Tween SetPanelTween;
    private bool playBGMusic = true;
    private bool playEffectMusic = true;
    public Sprite[] btnSprites;//0.音效开 1.音效关 2.背景音乐开 3.背景音乐关
    private Image Img_Btn_EffectAudio;
    private Image Img_Btn_BGAudio;
    public Text[] statisticesTexts;

    protected override void Awake()
    {
        base.Awake();
        SetPanelTween = transform.DOLocalMoveX(0, 0.5f);
        SetPanelTween.SetAutoKill(false);
        SetPanelTween.Pause();
        optionPageGo = transform.Find("OptionPage").gameObject;
        statisticsPageGo = transform.Find("StatisticsPage").gameObject;
        producerPageGo = transform.Find("ProducerPage").gameObject;
        Img_Btn_EffectAudio = optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Image>();
        Img_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Image>();
        Panel_ResetGo = transform.Find("Panel_Reset").gameObject;
    }
    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1920, 0, 0);
        transform.SetSiblingIndex(2);
    }

    public void ShowOptionPage()
    {
        if (!optionPageGo.activeSelf)
        {
            mUIFacade.PlayButtonAudioClip();
            optionPageGo.SetActive(true);
        }
        
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(false);
    }
    public void ShowStatisticsPage()
    {
        mUIFacade.PlayButtonAudioClip();
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(true);
        producerPageGo.SetActive(false);
        ShowStatistics();
    }
    public void ShowProducerPage()
    {
        mUIFacade.PlayButtonAudioClip();
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(true);
    }

    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        SetPanelTween.PlayBackwards();
        mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }

    public void MoveToCenter()
    {
        SetPanelTween.PlayForward();
    }

    public void CloseOrOpenBGMusic()
    {
        mUIFacade.PlayButtonAudioClip();
        playBGMusic = !playBGMusic;
        mUIFacade.CloseOrOpenBGMusic();
        if (playBGMusic)
        {
            Img_Btn_BGAudio.sprite = btnSprites[2];
        }
        else
        {
            Img_Btn_BGAudio.sprite = btnSprites[3];

        }
    }
    public void CloseOrOpenEffectMusic()
    {
        mUIFacade.PlayButtonAudioClip();
        playEffectMusic = !playEffectMusic;
        mUIFacade.CloseOrOpenEffectMusic();
        if (playEffectMusic)
        {
            Img_Btn_EffectAudio.sprite = btnSprites[0];
        }
        else
        {

            Img_Btn_EffectAudio.sprite = btnSprites[1];
        }
    }

    public void ShowStatistics()
    {
        PlayerManager playerManager = mUIFacade.mPlayerManager;
        statisticesTexts[0].text = playerManager.adventrueModelNum.ToString(); statisticesTexts[1].text = playerManager.burriedLevelNum.ToString(); statisticesTexts[2].text = playerManager.bossModelNum.ToString(); statisticesTexts[3].text = playerManager.coin.ToString();
        statisticesTexts[4].text = playerManager.killMonsterNum.ToString(); statisticesTexts[5].text = playerManager.killBossNum.ToString(); statisticesTexts[6].text = playerManager.clearItemNum.ToString();
    }

    public void ResetGame()
    {
        mUIFacade.PlayButtonAudioClip();
        GameManager.Instance.initPlayerManager = true;
        GameManager.Instance.playerManager.ReadData();
        ShowStatistics();
        CloseResetPanel();
    }

    public void ShowResetPanel()
    {
        Panel_ResetGo.SetActive(true);
    }

    public void CloseResetPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        Panel_ResetGo.SetActive(false);
    }
}
