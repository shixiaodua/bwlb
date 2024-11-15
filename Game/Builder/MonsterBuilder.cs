using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int m_monsterID;
    private GameObject monsterGo;
#if Game
    public void GetData(Monster ProductClass)
    {
        ProductClass.monsterID = m_monsterID;
        ProductClass.HP = m_monsterID * 100;
        ProductClass.currentHP = m_monsterID*100;
        ProductClass.moveSpeed = 1+m_monsterID*0.15f;
        ProductClass.prize = m_monsterID * 50;
        ProductClass.initMoveSpeed = 1 + m_monsterID * 0.15f;
    }

    public void GetOtherResource(Monster ProductClassGo)//设置动画控制器组件
    {
        ProductClassGo.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject itemGo=GameController.Instance.GetGameObject("MonsterPrefabs");
        Monster monster = GetProductClass(itemGo);
        monster.monsterID = m_monsterID;
        GetData(monster);
        GetOtherResource(monster);
        
        return itemGo;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
#endif
}
