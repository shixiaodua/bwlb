using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Round
{
    public struct RoundInfo
    {
        public List<int> mMonsterID;
    }
    public RoundInfo roundInfo;
    protected Round mNextRound;
    protected int mRoundID;
    protected Level mLevel;
    public Round(List<int>monsterIDList,int roundID,Level level)
    {
        roundInfo.mMonsterID = monsterIDList;
        mRoundID = roundID;
        mLevel = level;
    }
#if Game
    public void SetNextRound(Round round)
    {
        mNextRound = round;
    }

    public void Handler(int roundID)
    {
        if (roundID == mRoundID)
        {
            GameController.Instance.mMonsterIDList = roundInfo.mMonsterID;
        }
        else
        {
            mNextRound.Handler(roundID);
        }
    }
#endif
}
