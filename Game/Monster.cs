using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    //属性值
    public int monsterID;//怪物ID
    public float HP;//总血量
    public float currentHP;//当前血量
    public float moveSpeed;//当前速度
    public float initMoveSpeed;//初始速度
    public int prize;//奖励金钱
    //引用
    private Animator animator;
    private Slider slider;
    public GameObject TshitGo;//便便
    private GameController gameController;
    private List<Vector3> monsterPointList;
    //用于计数的属性或开关
    private int roadPointIndex;//当前走到哪个怪物路点
    private bool reachCarrot;//是否到达终点
    private bool hasDecreasSpeed;//是否减速
    private float decreasSpeedTimeVal;//减速计时器
    private float decreasTime;//减速持续的时间
    private float decreaseSpeed;//减速多少

    //资源
    public AudioClip dieAudio;//死亡音效
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
        if (gameController.isPause&&moveSpeed!=0)//暂停
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
        if (reachCarrot)//到达终点
        {
            DestoryMonster();//扔进对象池
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
    //初始化怪物方法
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
        CancelDecreasDebuff();//取消减速
    }
    //受到伤害的方法
    public void TakeDamage(float attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)//怪物死亡
        {
            DestoryMonster();
            return;
        }
        slider.value = (float)currentHP / HP;
    }
    //减速buff
    public void DecreaseSpeed(BullectProperty bullectProperty)
    {
        hasDecreasSpeed = true;
        decreasTime = bullectProperty.debuffTime;
        decreaseSpeed = bullectProperty.debuffValue;
        decreasSpeedTimeVal = 0;
        TshitGo.SetActive(true);
    }
    //去除减速buff
    private void CancelDecreasDebuff()
    {
        hasDecreasSpeed = false;
        decreasSpeedTimeVal = 0;
        decreaseSpeed = 1;
        decreasTime = 0;
        TshitGo.SetActive(false);
    }
    private void DestoryMonster()//怪物死亡方法
    {

        if (!reachCarrot)//被杀死
        {
            //生成金币以及数目
            GameObject coinGo = GameController.Instance.GetGameObject("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(gameController.transform);
            coinGo.transform.position = transform.position;
            //增加玩家的金币数量
            gameController.ChangeCoin(prize);
            //关于奖品的掉落
            if (Random.Range(1, 40) ==2)
            {
                GameObject prizeGo=gameController.GetGameObject("Prize");
                prizeGo.transform.SetParent(gameController.transform);
                prizeGo.transform.position = transform.position;
                //播放音效
                GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/GiftCreate");
            }
            //播放死亡音效
            GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Monster/" + GameController.Instance.currentStage.mBigLevelID.ToString() + "/" + (monsterID).ToString());
        }
        //判断是否是目标物体
        if (transform == gameController.targetTrans)
        {
            gameController.targetTrans = null;
            gameController.targetSignal.transform.SetParent(null);
            gameController.targetSignal.SetActive(false);
        }
        //产生销毁特效
        GameObject effectGo = gameController.GetGameObject("DestoryEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;
        
        //更改数据
        gameController.killMonsterNum++;
        gameController.killMonsterTotalNum++;
        InitMonsterGo();
        gameController.PushGameObjectToFactory("MonsterPrefabs", gameObject);
    }
    //获取特异性属性的方法
    public void GetMonsterProperty()
    {
        runtimeAnimatorController = gameController.controllers[monsterID];
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())//判断是否点击到ui
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
