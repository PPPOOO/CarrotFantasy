using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Round 
{
    [System.Serializable]
    public struct RoundInfo
    {
        public int[] mMonsterIDList;
    }

    protected Round mNextRound;
    protected int mRoundID;
    protected Level mLevel;

    public RoundInfo roundInfo;

    public Round(int[] monsterIDList,int roundID,Level level)
    {
        mLevel = level;
        roundInfo.mMonsterIDList = monsterIDList;
        mRoundID = roundID;
    }

    public void SetNextRound(Round nextRound)
    {
        mNextRound = nextRound;
    }

    public void Handle(int roundID)
    {
        if (mRoundID < roundID)
        {
            mNextRound.Handle(roundID);
        }
        else
        {
            GameController.Instance.mMonsterIDList = roundInfo.mMonsterIDList;
            GameController.Instance.CreateMonster();
            GameController.Instance.creatingMonster = true;
        }
    }
}
