using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//塔的共同特性
public class Tower : MonoBehaviour
{
    public int towerID;
    private CircleCollider2D circleCollider2D;//攻击检测范围
    public TowerPersonalProperty towerPersonalProperty;//塔的特异性功能脚本
    private GameObject attackRangeSr;//塔的攻击范围渲染
    public bool isTarget;//是集火目标
    public bool hasTarget;//有目标
#if Game
    private void Start()
    {
    }
    private void OnEnable()
    {
        Init();
        Invoke("InvokeInit",0.05f);
    }
    private void Update()
    {
        if (isTarget)
        {
            if ((towerPersonalProperty.targetTrans != GameController.Instance.targetTrans))
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
        if (hasTarget)
        {
            if (!towerPersonalProperty.targetTrans.gameObject.activeSelf)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
    }
    private void Init()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        towerPersonalProperty.tower = this;
        attackRangeSr = transform.Find("attackRange").gameObject;
        attackRangeSr.gameObject.SetActive(false);
        towerPersonalProperty.targetTrans = null;
        isTarget = false;
        hasTarget = false;
    }
    private void InvokeInit()
    {
        circleCollider2D.radius = GameController.Instance.attackRadius[towerID,towerPersonalProperty.towerLevel];//攻击范围的半径
        float bl = GameController.Instance.attackRadius[towerID, towerPersonalProperty.towerLevel] / GameController.Instance.attackRadius[towerID, 1];
        attackRangeSr.transform.localScale = new Vector3(bl+0.3f, bl+0.3f, 1);
    }
    public void GetTowerProperty()//获取塔的特异属性
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)//进入检测方法
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag != "Monster" && collision.tag != "Item")//不是怪物且不是物体
        {
            return;
        }
        //获取到集火目标的trans
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null)//有集火目标
        {
            if (!isTarget)//当前攻击范围没有集火目标
            {
                if (collision.gameObject.transform == targetTrans)//当前检测的物体是集火目标
                {
                    isTarget = true;
                    hasTarget = true;
                    towerPersonalProperty.targetTrans = collision.transform;
                }
                else
                    if (!hasTarget&&collision.tag=="Monster")
                {
                    hasTarget = true;
                    towerPersonalProperty.targetTrans = collision.transform;
                }
            }
        }
        else
        {
            if (!hasTarget&&collision.tag=="Monster")//当前没有目标且进入攻击范围的是怪物
            {
                hasTarget = true;
                towerPersonalProperty.targetTrans = collision.transform;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//停留方法
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag != "Monster" && collision.tag != "Item")//不是怪物且不是物体
        {
            return;
        }
        //获取到集火目标的trans
        Transform targetTrans = GameController.Instance.targetTrans;
        //Debug.Log(targetTrans.name);
        if (targetTrans&&targetTrans==collision.transform)//有集火目标且集火目标是该物体
        {
            
            towerPersonalProperty.targetTrans = collision.transform;
            hasTarget = true;
            isTarget = true;
        }
        else
        if (!hasTarget && collision.tag == "Monster")
        {
            towerPersonalProperty.targetTrans = collision.transform;
            hasTarget = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//离开方法
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (towerPersonalProperty.targetTrans == collision.transform)//攻击目标离开范围
        {
            towerPersonalProperty.targetTrans = null;//目标置空
            isTarget = false;
            hasTarget = false;
        }
    }
    //销毁塔
    public void DestroyTower()
    {
        towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/TowerSet/" + towerPersonalProperty.towerLevel.ToString(), gameObject);
    }
#endif
}
