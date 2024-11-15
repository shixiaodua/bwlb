using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    //����ֵ
    public int monsterID;//����ID
    public float HP;//��Ѫ��
    public float currentHP;//��ǰѪ��
    public float moveSpeed;//��ǰ�ٶ�
    public float initMoveSpeed;//��ʼ�ٶ�
    public int prize;//������Ǯ
    //����
    private Animator animator;
    private Slider slider;
    public GameObject TshitGo;//���
    private GameController gameController;
    private List<Vector3> monsterPointList;
    //���ڼ��������Ի򿪹�
    private int roadPointIndex;//��ǰ�ߵ��ĸ�����·��
    private bool reachCarrot;//�Ƿ񵽴��յ�
    private bool hasDecreasSpeed;//�Ƿ����
    private float decreasSpeedTimeVal;//���ټ�ʱ��
    private float decreasTime;//���ٳ�����ʱ��
    private float decreaseSpeed;//���ٶ���

    //��Դ
    public AudioClip dieAudio;//������Ч
    public RuntimeAnimatorController runtimeAnimatorController;
#if Game
    private void Awake()
    {
        monsterPointList = new List<Vector3>();
        animator = GetComponent<Animator>();
        TshitGo = transform.Find("Tshit").gameObject;
        slider = transform.Find("MonsterCanvas/HPSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        decreaseSpeed = 1;
    }
    private void OnEnable()
    {
        gameController = GameController.Instance;
        slider.gameObject.SetActive(false);
        foreach(var i in gameController.mapMaker.monsterPoint)
        {
            Vector3 w = new Vector3((float)i.x, (float)i.y, -3);
            monsterPointList.Add(w);
        }
        roadPointIndex = 1;
    }

    private void Update()
    {
        if (GameController.Instance.gameOver)
        {
            DestoryMonster();return;
        }
        if (gameController.isPause&&moveSpeed!=0)//��ͣ
        {
            moveSpeed = 0;
        }
        if(!gameController.isPause && moveSpeed == 0)
        {
            moveSpeed = initMoveSpeed;
        }
        if (hasDecreasSpeed)
        {
            if (decreasSpeedTimeVal >= decreasTime)
            {
                CancelDecreasDebuff();
            }
            else
            {
                decreasSpeedTimeVal += Time.deltaTime;
            }
        }
        if (reachCarrot)//�����յ�
        {
            DestoryMonster();//�ӽ������
            gameController.carrotHP--;
            gameController.carrot.TakeDamage();
        }
        else
        {
            if (monsterPointList[roadPointIndex].x - monsterPointList[roadPointIndex-1].x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                slider.gameObject.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            transform.position = Vector3.Lerp(transform.position, monsterPointList[roadPointIndex], 1/Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) * Time.deltaTime * (moveSpeed/decreaseSpeed) * gameController.gameSpeed);
            if(Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) <= 0.000001)
            {
                roadPointIndex++;
                if (roadPointIndex >= monsterPointList.Count)
                {
                    
                    reachCarrot = true;
                }
                
            }
        }
    } 
    //��ʼ�����﷽��
    private void InitMonsterGo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        roadPointIndex = 1;
        moveSpeed = 0;
        reachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = Vector3.zero;
        monsterPointList.Clear();
        CancelDecreasDebuff();//ȡ������
    }
    //�ܵ��˺��ķ���
    public void TakeDamage(float attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)//��������
        {
            DestoryMonster();
            return;
        }
        slider.value = (float)currentHP / HP;
    }
    //����buff
    public void DecreaseSpeed(BullectProperty bullectProperty)
    {
        hasDecreasSpeed = true;
        decreasTime = bullectProperty.debuffTime;
        decreaseSpeed = bullectProperty.debuffValue;
        decreasSpeedTimeVal = 0;
        TshitGo.SetActive(true);
    }
    //ȥ������buff
    private void CancelDecreasDebuff()
    {
        hasDecreasSpeed = false;
        decreasSpeedTimeVal = 0;
        decreaseSpeed = 1;
        decreasTime = 0;
        TshitGo.SetActive(false);
    }
    private void DestoryMonster()//������������
    {

        if (!reachCarrot)//��ɱ��
        {
            //���ɽ���Լ���Ŀ
            GameObject coinGo = GameController.Instance.GetGameObject("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(gameController.transform);
            coinGo.transform.position = transform.position;
            //������ҵĽ������
            gameController.ChangeCoin(prize);
            //���ڽ�Ʒ�ĵ���
            if (Random.Range(1, 40) ==2)
            {
                GameObject prizeGo=gameController.GetGameObject("Prize");
                prizeGo.transform.SetParent(gameController.transform);
                prizeGo.transform.position = transform.position;
                //������Ч
                GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/GiftCreate");
            }
            //����������Ч
            GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Monster/" + GameController.Instance.currentStage.mBigLevelID.ToString() + "/" + (monsterID).ToString());
        }
        //�ж��Ƿ���Ŀ������
        if (transform == gameController.targetTrans)
        {
            gameController.targetTrans = null;
            gameController.targetSignal.transform.SetParent(null);
            gameController.targetSignal.SetActive(false);
        }
        //����������Ч
        GameObject effectGo = gameController.GetGameObject("DestoryEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;
        
        //��������
        gameController.killMonsterNum++;
        gameController.killMonsterTotalNum++;
        InitMonsterGo();
        gameController.PushGameObjectToFactory("MonsterPrefabs", gameObject);
    }
    //��ȡ���������Եķ���
    public void GetMonsterProperty()
    {
        runtimeAnimatorController = gameController.controllers[monsterID];
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())//�ж��Ƿ�����ui
        {
            return;
        }
        if (GameController.Instance.selectGrid != null)
        {
            GameController.Instance.selectGrid.HideGrid();
            GameController.Instance.selectGrid = null;
            return;
        }
        if (gameController.targetTrans == null||gameController.targetTrans!=gameObject.transform)
        {
            gameController.targetTrans = gameObject.transform;
            gameController.ShowSignal();
        }
        else
        {
            gameController.HideSignal();
        }
    }
#endif
}
