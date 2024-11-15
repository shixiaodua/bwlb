using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPersonalProperty : MonoBehaviour
{
    //����ֵ
    public int towerLevel;//��ǰ���ĵȼ�
    protected float timeVal;//������ʱ��
    public float attackCD;//����cd
    public int sellPrice;//���ۼ۸�
    public int upLevelPrice;//�����۸�
    public int price;//��ǰ���ļ۸�
    //��Դ
    public GameObject bullectGo;//����Դ��Ϊ��ʹ�����ĳ�Ա�����뷽��
    //����
    public Tower tower;//�����tower����
    public Transform targetTrans;//����Ŀ���trans
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
        if (GameController.Instance.isPause)//��ͣ
        {
            return;
        }
        
        if (targetTrans == null)
        {
            if (timeVal < attackCD) timeVal += Time.deltaTime;
            return;
        }
        if (!targetTrans.gameObject.activeSelf)//����Żض����,��ȫУ��
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
        //��ת
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
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/Attack/"+tower.towerID.ToString());
    }
#endif
}
