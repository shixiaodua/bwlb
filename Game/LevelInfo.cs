using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    //��ͼ��Ϣ
    public int bigLevelID;
    public int LevelID;
    public GridPoint.GridState[] gridStates=new GridPoint.GridState[300];
    public List<GridPoint.GridIndex> monsterPoint = new List<GridPoint.GridIndex>();
    //���ﲨ����Ϣ
    public int roundNum;//��������
    public Round.RoundInfo[] roundInfos=new Round.RoundInfo[100];//ÿһ�ֲ����Ĺ���ID 
    public LevelInfo()
    {
        roundNum = 0;
        for (int i = 0; i < 100; i++) { roundInfos[i].mMonsterID=new List<int>(); roundInfos[i].mMonsterID.Add(0); }
    }
}
