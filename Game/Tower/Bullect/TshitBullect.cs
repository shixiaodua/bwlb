using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TshitBullect : Bullect
{
    BullectProperty bullectProperty;
    // Start is called before the first frame update
#if Game
    void Start()
    {
        bullectProperty = new BullectProperty();
        bullectProperty.debuffValue = 1+towerLevel;
        bullectProperty.debuffTime = 0.3f+towerLevel*0.2f;
    }

    // Update is called once per frame
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (collision.gameObject.activeSelf)//���ܴ��������ӵ�����һ֡������ͬһ����������������ֻ��һ��Ѫ
            {
                if (targetTrans == null || !targetTrans.gameObject.activeSelf)//���Ŀ������
                {
                    return;
                }
                if (collision.tag == "Monster")
                {

                    collision.SendMessage("DecreaseSpeed", bullectProperty);
                }
            }
        }
        base.OnTriggerEnter2D(collision);
    }
#endif
}

public struct BullectProperty{
    public float debuffTime;
    public float debuffValue;
}
