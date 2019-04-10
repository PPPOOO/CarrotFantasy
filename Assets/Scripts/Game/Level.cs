using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level 
{
    public int totalRound;
    public Round[] roundList;
    public int currentRound;

    public Level(int roundNum, List<Round.RoundInfo> roundInfos)
    {
        totalRound = roundNum;
        roundList = new Round[totalRound];
        for (int i = 0; i < totalRound; i++)
        {
            roundList[i] = new Round(roundInfos[i].mMonsterIDList, i, this);
        }
        for (int i = 0; i < totalRound; i++)
        {
            if (i == totalRound - 1)
            {
                break;
            }
            roundList[i].SetNextRound(roundList[i + 1]);
        }
    }

    public void HandleRound()
    {
        if (currentRound >= totalRound)
        {
            GameController.Instance.creatingMonster = true;
            GameController.Instance.isWin = true;
            currentRound--;
            GameController.Instance.normalModelPanel.ShowGameWinPage();
            
        }
        else if (currentRound == totalRound - 1)
        {
            GameController.Instance.normalModelPanel.ShowFinalWaveUI();
            roundList[currentRound].Handle(currentRound);
        }
        else
        {
            roundList[currentRound].Handle(currentRound);
        }
    }
    public void HandleLastRound()
    {
        roundList[currentRound].Handle(currentRound);
    }

    public void AddRoundNum()
    {
        currentRound++;
    }

}
