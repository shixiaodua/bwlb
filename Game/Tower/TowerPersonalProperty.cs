using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPersonalProperty : MonoBehaviour
{
    //属性值
    public int towerLevel;//当前塔的等级
    protected float timeVal;//攻击计时器
    public float attackCD;//攻击cd
    public int sellPrice;//销售价格
    public int upLevelPrice;//升级价格
    public int price;//当前塔的价格
    //资源
    public GameObject bullectGo;//空资源，为了使用它的成员变量与方法
    //引用
    public Tower tower;//自身的tower对象
    public Transform targetTrans;//攻击目标的trans
    public Animator animator;
#if Game

    protected virtual void Start()
    {
        upLevelPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
    }

    protected virtual void Update()
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
        //旋转
        Transform towerItemGo = transform.Find("tower");
        LookAt(towerItemGo.position,targetTrans.position);
    }

    private void LookAt(Vector2 oriPos, Vector2 targetPos)
    {
        Vector2 v = targetPos - oriPos;
        Transform towerItemGo = transform.Find("tower");
        towerItemGo.right = v;
    }
    public void Init()
    {
    }
    public void SellTower()
    {
        GameController.Instance.ChangeCoin(sellPrice);
        GameObject effect= GameController.Instance.GetGameObject("BuildEffect");
        effect.transform.position = this.gameObject.transform.position;
        tower.DestroyTower();
    }
    public void UPLevelTower()
    {
        GameController.Instance.ChangeCoin(-upLevelPrice);
        GameObject effect= GameController.Instance.GetGameObject("UpLevelEffect");
        effect.transform.position = this.gameObject.transform.position;
        tower.DestroyTower();
    }
    protected virtual void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        animator.Play("Attack");
        bullectGo = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
        bullectGo.transform.position = transform.position;
        Bullect bullect = bullectGo.GetComponent<Bullect>();
        bullect.targetTrans = targetTrans;
        bullect.towerLevel = towerLevel;
        bullect.towerID = tower.towerID;
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/Attack/"+tower.towerID.ToString());
    }
#endif
}
