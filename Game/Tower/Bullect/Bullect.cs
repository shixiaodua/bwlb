using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour
{
    public Transform targetTrans;//子弹的追踪目标
    public float moveSpeed;//子弹移动速度
    public int attackValue;
    public int towerID;
    public int towerLevel;

#if Game
    protected virtual void Update()
    {
        if (GameController.Instance.gameOver)//如果游戏结束
        {
            DestroyBullect();
            return;
        }
        if (GameController.Instance.isPause)//如果游戏暂停
        {
            return;
        }
        if (targetTrans == null||!targetTrans.gameObject.activeSelf)//目标为空或者目标已经死亡
        {
            DestroyBullect();
            return;
        }
        //子弹方向
        LookAt(transform.position, targetTrans.position);
        transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3), 1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * moveSpeed * GameController.Instance.gameSpeed*Time.deltaTime);
    }
    //子弹朝向
    protected void LookAt(Vector2 oriPos, Vector2 targetPos)
    {
        Vector2 v = targetPos - oriPos;
        transform.forward = v;
    }
    //销毁子弹
    protected virtual void DestroyBullect()
    {
        targetTrans = null;
        transform.SetParent(null);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullect/" + towerLevel.ToString(),gameObject);
    } 
    //创建特效
    protected void CreateEffect()
    {
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            return;
        }
        GameObject effect=GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effect.transform.position = transform.position;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (collision.gameObject.activeSelf)//可能存在两个子弹在这一帧都碰到同一个怪物，并且这个怪物只有一滴血
            {
                if (targetTrans == null||!targetTrans.gameObject.activeSelf)//如果目标死亡
                {
                    return;
                }
                if (collision.tag=="Monster"||(targetTrans!=null&&collision.tag=="Item"&&collision.transform==targetTrans))
                {
                    CreateEffect();
                    collision.SendMessage("TakeDamage",attackValue);
                    DestroyBullect();
                }
            }
        }
    }
#endif
}
