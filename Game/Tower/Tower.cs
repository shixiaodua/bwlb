using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���Ĺ�ͬ����
public class Tower : MonoBehaviour
{
    public int towerID;
    private CircleCollider2D circleCollider2D;//������ⷶΧ
    public TowerPersonalProperty towerPersonalProperty;//���������Թ��ܽű�
    private GameObject attackRangeSr;//���Ĺ�����Χ��Ⱦ
    public bool isTarget;//�Ǽ���Ŀ��
    public bool hasTarget;//��Ŀ��
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
        circleCollider2D.radius = GameController.Instance.attackRadius[towerID,towerPersonalProperty.towerLevel];//������Χ�İ뾶
        float bl = GameController.Instance.attackRadius[towerID, towerPersonalProperty.towerLevel] / GameController.Instance.attackRadius[towerID, 1];
        attackRangeSr.transform.localScale = new Vector3(bl+0.3f, bl+0.3f, 1);
    }
    public void GetTowerProperty()//��ȡ������������
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)//�����ⷽ��
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag != "Monster" && collision.tag != "Item")//���ǹ����Ҳ�������
        {
            return;
        }
        //��ȡ������Ŀ���trans
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null)//�м���Ŀ��
        {
            if (!isTarget)//��ǰ������Χû�м���Ŀ��
            {
                if (collision.gameObject.transform == targetTrans)//��ǰ���������Ǽ���Ŀ��
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
            if (!hasTarget&&collision.tag=="Monster")//��ǰû��Ŀ���ҽ��빥����Χ���ǹ���
            {
                hasTarget = true;
                towerPersonalProperty.targetTrans = collision.transform;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//ͣ������
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag != "Monster" && collision.tag != "Item")//���ǹ����Ҳ�������
        {
            return;
        }
        //��ȡ������Ŀ���trans
        Transform targetTrans = GameController.Instance.targetTrans;
        //Debug.Log(targetTrans.name);
        if (targetTrans&&targetTrans==collision.transform)//�м���Ŀ���Ҽ���Ŀ���Ǹ�����
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
    private void OnTriggerExit2D(Collider2D collision)//�뿪����
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (towerPersonalProperty.targetTrans == collision.transform)//����Ŀ���뿪��Χ
        {
            towerPersonalProperty.targetTrans = null;//Ŀ���ÿ�
            isTarget = false;
            hasTarget = false;
        }
    }
    //������
    public void DestroyTower()
    {
        towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/TowerSet/" + towerPersonalProperty.towerLevel.ToString(), gameObject);
    }
#endif
}
