using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    public GridPoint gridPoint;//������һ������
    public int itemID;
    private GameController gameController;
    public int prize;
    private float HP;
    private float currentHP;
    private Slider slider;
    private float timeVal;//��ʾ������Ѫ����ʱ��
    private bool showHP;//�Ƿ���ʾѪ��
#if Game
    private void Awake()
    {
        slider = transform.Find("ItemCanvas/HpSlider").GetComponent<Slider>();
    }
    private void OnEnable()
    {
        gameController = GameController.Instance;
        InitItem();
    }
    // Update is called once per frame
    void Update()
    {
        if (showHP)
        {
            timeVal -= Time.deltaTime;
            if (timeVal <= 0)
            {
                showHP = false;
                timeVal = 0;
                slider.gameObject.SetActive(false);
            }
        }
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())//�ж��Ƿ�����ui
        {
            return;
        }
        if (GameController.Instance.selectGrid!=null)
        {
            GameController.Instance.selectGrid.HideGrid();
            GameController.Instance.selectGrid = null;
            return;
        }
        if (gameController.targetTrans == null || gameController.targetTrans != gameObject.transform)
        {
            gameController.targetTrans = gameObject.transform;
            gameController.ShowSignal();
        }
        else
        {
            gameController.HideSignal();
        }
    }
    public void TakeDamage(float attackValue)
    {
        slider.gameObject.SetActive(true);
        showHP = true;
        timeVal = 3;
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            DestoryItem();
            return;
        }
        slider.value = (float)currentHP / HP;
    }

    private void DestoryItem()//������������
     {
            //���ɽ���Լ���Ŀ
            GameObject coinGo = GameController.Instance.GetGameObject("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(gameController.transform);
            coinGo.transform.position = transform.position;
            //������ҵĽ������
            gameController.ChangeCoin(prize);
            //���ڽ�Ʒ�ĵ���

        //�ж��Ƿ���Ŀ������
        if (transform==gameController.targetTrans)
        {
            gameController.targetTrans = null;
            gameController.targetSignal.transform.SetParent(null);
            gameController.targetSignal.SetActive(false);
        }
        //����������Ч
        GameObject effectGo = gameController.GetGameObject("DestoryEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;
        gameController.clearItemNum++;
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Item");
        //����Ӧ���ӵ�״̬�޸�
        gridPoint.gridState.hasItem = false;
        gridPoint.gridState.itemID = 0;
        gameController.PushGameObjectToFactory(GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/"+itemID.ToString(), gameObject);
    }
    private void InitItem()
    {
        prize = 100 - itemID * 3;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        timeVal = 0;
        showHP = false;
        slider.gameObject.SetActive(false);
    }
#endif
}
