using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmill : TowerPersonalProperty
{
#if Game
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameController.Instance.isPause)//暂停
        {
            return;
        }

        if (targetTrans == null)
        {
            if (timeVal < attackCD) timeVal += Time.deltaTime;
            return;
        }
        if (!targetTrans.gameObject.activeSelf)//物体放回对象池,安全校验
        {
            targetTrans = null;
            return;
        }
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
#endif
}
