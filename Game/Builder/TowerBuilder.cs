using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : IBuilder<Tower>
{
    public int m_TowerID;//����ID
    private GameObject towerGo;//��
    public int m_TowerLevel;//�����ĵȼ�
#if Game
    public void GetData(Tower ProductClass)
    {
        ProductClass.towerID = m_TowerID;
        ProductClass.towerPersonalProperty.towerLevel = m_TowerLevel;
    }

    public void GetOtherResource(Tower ProductClassGo)
    {
        ProductClassGo.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
       GameObject itemGo = GameController.Instance.GetGameObject("Tower/ID"+m_TowerID.ToString()+"/TowerSet/"+m_TowerLevel.ToString());
        Tower tower = GetProductClass(itemGo);
        GetData(tower);
        GetOtherResource(tower);
        return itemGo;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Tower>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#endif
}
