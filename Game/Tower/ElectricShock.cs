using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ElectricShock : TowerPersonalProperty
{
    private float distance;
    private float bullectWidth;
    private float bullectLenght;
    public float attackTimeVal;//攻击特效计时器
    public float attackValue;
#if Game
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bullectGo = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
        bullectGo.transform.SetParent(transform);//保证该子弹跟随父亲销毁而隐藏
        bullectGo.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameController.Instance.isPause)//暂停
        {
            return;
        }
        attackTimeVal += Time.deltaTime;
        if (targetTrans == null)//没有目标
        {
            if (bullectGo.activeSelf)
            {
                bullectGo.SetActive(false);
            }
            return;
        }
        Attack();
    }

    protected override void Attack()
    {
        if (targetTrans == null)//没有目标
        {
            return;
        }
        animator.Play("Attack");
        Vector3 attackPos = transform.position + new Vector3(0, 0.2f, 0);
        distance = Vector3.Distance(targetTrans.position + new Vector3(0, 0, 4), attackPos);
        bullectWidth = 1 / distance*towerLevel;
        bullectLenght = distance / 2 - distance * 0.1f;
        if (bullectWidth <= 0.5f)
        {
            bullectWidth = 0.5f;
        }
        if (bullectWidth >= 1.5f)
        {
            bullectWidth = 1.5f;
        }
        
        bullectGo.transform.position = new Vector3((targetTrans.position.x + attackPos.x) / 2, (targetTrans.position.y + attackPos.y) / 2, 0);
        bullectGo.transform.localScale = new Vector3(bullectWidth, bullectLenght, 1);
        LookAt(transform.position, targetTrans.position);
        bullectGo.SetActive(true);
        if (attackTimeVal >= 0.35f)
        {
            CreateEffect();
            attackTimeVal = 0;
        }
        if (targetTrans.tag == "Monster")
            targetTrans.gameObject.GetComponent<Monster>().TakeDamage((attackValue * Time.deltaTime));
        if (targetTrans.tag == "Item")
            targetTrans.gameObject.GetComponent<Item>().TakeDamage((attackValue * Time.deltaTime));
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/Attack/5");
    }

    private void CreateEffect()
    {
            if (targetTrans == null || !targetTrans.gameObject.activeSelf)
            {
                return;
            }
            GameObject effect = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Effect/" + towerLevel.ToString());
            effect.transform.position = targetTrans.position;
    }
    private void LookAt(Vector2 oriPos, Vector2 targetPos)
    {
        Vector2 v = targetPos - oriPos;
        bullectGo.transform.up = v;
    }
#endif
}
