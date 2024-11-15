using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject monster;

    private Stack<GameObject> monsterPool;

    private Stack<GameObject> activeMonsterList;

    int size = 10;
    // Start is called before the first frame update
    void Start()
    {
        monsterPool = new Stack<GameObject>();
        activeMonsterList = new Stack<GameObject>();
        for (int i=1; i <= size; i++)
        {
            PushMonster(Instantiate(monster));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject itemGo;
            itemGo = GetMonster();
            itemGo.transform.position = Vector3.one;
            activeMonsterList.Push(itemGo);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (activeMonsterList.Count > 0)
            {

                PushMonster(activeMonsterList.Pop());
            }
        }
    }

    private GameObject GetMonster()
    {
        GameObject monsterGo = null;

        if (monsterPool.Count > 0)
        {
            monsterGo = monsterPool.Pop();
        }
        else
        {
            monsterGo = Instantiate(monster);
        }
        monsterGo.transform.SetParent(transform);
        monsterGo.SetActive(true);
        return monsterGo;
    }

    private void PushMonster(GameObject monsterGo)
    {
        monsterGo.transform.SetParent(transform);
        monsterGo.SetActive(false);
        monsterPool.Push(monsterGo);
        return ;
    }
}
