using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIPanel;

//��ҵĹ������𱣴��Լ����������Ϸ�ĸ�����Ϣ
public class PlayerManager
{
    public int adventrueModelNum;//ð��ģʽ����������Ϸ����ҿ��Բ����ð��ģʽ����Ŀ
    public int burriedLevelNum;//��صĹؿ���������Ϸ�����ػ���δ�����Ĺؿ���Ŀ
    public int bossModelNum;//Bossģʽ����������Ϸ�к���Bossս����ģʽ��
    public int coin;//�����������Ϸ���ۻ��Ļ��ҵ�λ��ͨ�����ڹ���������װ����
    public int killMonsterNum;//ɱ���Ĺ�����������Ϸ���Ѿ����ܵ���ͨ��������
    public int killBossNum;//ɱ����Boss���������ܵ�Boss��������
    public int clearItemNum;//�������Ʒ����������ϰ����ռ����ض���Ʒ����Ŀ
    public List<bool> unLpckdeNormalModelBigLevelList;//���еĴ�ؿ���Ϣ
    public List<Stage> unLpckdeNormalModelLevel;//���е�С�ؿ���Ϣ
    public List<int> unLpckdeNormalModelLevelNum;//ÿ����ؿ������˼���С�ؿ�
    public List<int> BigTotalLeveNum;//ÿ����ؿ��ж��ٸ�С�ؿ�

    //������
    public int milk;//ţ��������ι���������Ʒ
    public int cookies;//����������ι������
    public int nest;//��Ѩ����
    public int diamands;//��ʯ����
    public List<MonsterPetDate> monsterPetDatesList;//����ι����Ϣ
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
        //��ȡ�ʹ洢��ɫ��Ϣ����
        Mement mement = new Mement();
        //��ȡ��ɫ��Ϣ
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

        //������
        milk = playerManager.milk;
        cookies = playerManager.cookies;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        monsterPetDatesList = playerManager.monsterPetDatesList;
        AIDataList=playerManager.AIDataList;
        // //������ӹ�������
        // monsterPetDatesList = new List<MonsterPetDate>();
        // MonsterPetDate monsterPetDate = new MonsterPetDate
        // {
        //    monsterID = 1,
        //    monsterLevel = 1,
        //    remainCookies = 10,
        //    remainMilk = 10,
        // };
        // monsterPetDatesList.Add(monsterPetDate);
        // //������ӵĹ�������
        // mement.SavePlayerManager();
    }
}
