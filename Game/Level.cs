using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
#if Game
    public int totalRound;
    public Round[] roundList;
    public int currentRound;
    public Level(int roundNum,Round.RoundInfo[] roundInfoList)
    {
        totalRound = roundNum;
        currentRound =0;
        roundList = new Round[totalRound+1];
        for(int i = 1; i <= totalRound; i++)
        {
            roundList[i] = new Round(roundInfoList[i].mMonsterID, i, this);
        }

        //����������
        for(int i = 1; i < totalRound; i++)
        {
            roundList[i].SetNextRound(roundList[i + 1]);
        }
    }

    public void HandleRound()
    {
        if (currentRound > totalRound)//��Ϸʤ��
        {
            currentRound = totalRound;
            GameController.Instance.GameWin();
        }
        else
            if (currentRound == totalRound)//���һ������
        {
            GameController.Instance.ShowFinalWaveUI();
            roundList[currentRound].Handler(currentRound);
        }
        else//��������
        {
            roundList[currentRound].Handler(currentRound);
        }
    }
    public void AddRoundNum()
    {
        currentRound++;
    }
#endif
}
