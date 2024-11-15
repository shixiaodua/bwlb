using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmilBullect : Bullect
{
    public Vector3 targetVector;
    private float timeVal;
#if Game
    private void Start()
    {
    }
    private void OnEnable()
    {
        StartCoroutine(InitBullect());
    }
    private IEnumerator InitBullect()
    {
        yield return null;
        targetVector = (targetTrans.position - transform.position);
        targetVector = new Vector3(targetVector.x, targetVector.y, 0);
        targetVector = targetVector.normalized;
    }
    protected override void Update()
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
        if (timeVal >= 3)
        {
            
            DestroyBullect();
            return;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
        
        transform.position += targetVector * moveSpeed * GameController.Instance.gameSpeed * Time.deltaTime*3;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf == false)
        {
            return;
        }
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
            CreateEffect();
        }
    }
    protected override void DestroyBullect()
    {
        timeVal = 0;
        base.DestroyBullect();
    }
#endif
}
