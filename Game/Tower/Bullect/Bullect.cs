using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour
{
    public Transform targetTrans;//�ӵ���׷��Ŀ��
    public float moveSpeed;//�ӵ��ƶ��ٶ�
    public int attackValue;
    public int towerID;
    public int towerLevel;

#if Game
    protected virtual void Update()
    {
        if (GameController.Instance.gameOver)//�����Ϸ����
        {
            DestroyBullect();
            return;
        }
        if (GameController.Instance.isPause)//�����Ϸ��ͣ
        {
            return;
        }
        if (targetTrans == null||!targetTrans.gameObject.activeSelf)//Ŀ��Ϊ�ջ���Ŀ���Ѿ�����
        {
            DestroyBullect();
            return;
        }
        //�ӵ�����
        LookAt(transform.position, targetTrans.position);
        transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3), 1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * moveSpeed * GameController.Instance.gameSpeed*Time.deltaTime);
    }
    //�ӵ�����
    protected void LookAt(Vector2 oriPos, Vector2 targetPos)
    {
        Vector2 v = targetPos - oriPos;
        transform.forward = v;
    }
    //�����ӵ�
    protected virtual void DestroyBullect()
    {
        targetTrans = null;
        transform.SetParent(null);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullect/" + towerLevel.ToString(),gameObject);
    } 
    //������Ч
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
            if (collision.gameObject.activeSelf)//���ܴ��������ӵ�����һ֡������ͬһ����������������ֻ��һ��Ѫ
            {
                if (targetTrans == null||!targetTrans.gameObject.activeSelf)//���Ŀ������
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
