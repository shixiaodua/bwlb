using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIPanel;

//玩家的管理，负责保存以及加载玩家游戏的各种信息
public class PlayerManager
{
    public int adventrueModelNum;//冒险模式的数量，游戏中玩家可以参与的冒险模式的数目
    public int burriedLevelNum;//埋藏的关卡数量，游戏中隐藏或尚未解锁的关卡数目
    public int bossModelNum;//Boss模式的数量，游戏中含有Boss战斗的模式数
    public int coin;//金币数量，游戏中累积的货币单位，通常用于购买升级、装备等
    public int killMonsterNum;//杀死的怪物数量，游戏中已经击败的普通怪物总数
    public int killBossNum;//杀死的Boss数量，击败的Boss怪物总数
    public int clearItemNum;//清除的物品数量，清除障碍或收集的特定物品的数目
    public List<bool> unLpckdeNormalModelBigLevelList;//所有的大关卡信息
    public List<Stage> unLpckdeNormalModelLevel;//所有的小关卡信息
    public List<int> unLpckdeNormalModelLevelNum;//每个大关卡解锁了几个小关卡
    public List<int> BigTotalLeveNum;//每个大关卡有多少个小关卡

    //怪物窝
    public int milk;//牛奶数量，喂养宠物的物品
    public int cookies;//饼干数量，喂养宠物
    public int nest;//巢穴数量
    public int diamands;//钻石数量
    public List<MonsterPetDate> monsterPetDatesList;//宠物喂养信息
    public  Dictionary<string, List<SendData>> AIDataList;

    public PlayerManager()
    {
        
    }

    public void SaveData()
    {
        Mement mement = new Mement();
        mement.SavePlayerManager();
    }
    public void ReadData()
    {
        //读取和存储角色信息的类
        Mement mement = new Mement();
        //读取角色信息
        PlayerManager playerManager = mement.LoadPlayerManager();
        //
        adventrueModelNum = playerManager.adventrueModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        killMonsterNum = playerManager.killMonsterNum;
        killBossNum = playerManager.killBossNum;
        clearItemNum = playerManager.clearItemNum;
        unLpckdeNormalModelBigLevelList = playerManager.unLpckdeNormalModelBigLevelList;
        unLpckdeNormalModelLevel = playerManager.unLpckdeNormalModelLevel;
        unLpckdeNormalModelLevelNum = playerManager.unLpckdeNormalModelLevelNum;
        BigTotalLeveNum = playerManager.BigTotalLeveNum;

        //怪物窝
        milk = playerManager.milk;
        cookies = playerManager.cookies;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        monsterPetDatesList = playerManager.monsterPetDatesList;
        AIDataList=playerManager.AIDataList;
        // //测试添加怪物数据
        // monsterPetDatesList = new List<MonsterPetDate>();
        // MonsterPetDate monsterPetDate = new MonsterPetDate
        // {
        //    monsterID = 1,
        //    monsterLevel = 1,
        //    remainCookies = 10,
        //    remainMilk = 10,
        // };
        // monsterPetDatesList.Add(monsterPetDate);
        // //保存添加的怪物数据
        // mement.SavePlayerManager();
    }
}
