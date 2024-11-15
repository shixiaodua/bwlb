using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    //地图信息
    public int bigLevelID;
    public int LevelID;
    public GridPoint.GridState[] gridStates=new GridPoint.GridState[300];
    public List<GridPoint.GridIndex> monsterPoint = new List<GridPoint.GridIndex>();
    //怪物波次信息
    public int roundNum;//怪物轮数
    public Round.RoundInfo[] roundInfos=new Round.RoundInfo[100];//每一轮产生的怪物ID 
    public LevelInfo()
    {
        roundNum = 0;
        for (int i = 0; i < 100; i++) { roundInfos[i].mMonsterID=new List<int>(); roundInfos[i].mMonsterID.Add(0); }
    }
}
