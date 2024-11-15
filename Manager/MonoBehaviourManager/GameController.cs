using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//负责游戏控制，控制游戏的整个逻辑
public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get => _instance; }
    //引用
    public Level level;
    private GameManager mGameManager;
    public List<int> mMonsterIDList;//当前波次的怪物id列表
    public MapMaker mapMaker;
    public Stage currentStage;//当前关卡信息
    public Transform targetTrans;//集火目标
    public GameObject targetSignal;//集火信号
    public GridPoint selectGrid;//当前选择的格子
    public Carrot carrot;//萝卜脚本
    public SpriteRenderer BGSpriteRenderer;//背景图
    public SpriteRenderer PathSpriteRenderer;//路径图

    public NormalModelPanel normalModelPanel;//游戏ui面板
    
    public int mMonsterIDIndex;//当前产生怪物索引
    //用于计数的成员变量
    public int killMonsterNum;//当前波次杀死怪物数量
    public int killMonsterTotalNum;//杀死怪物总数
    public int clearItemNum;//销毁道具数量
    public float timeMonster;//产怪间隔时间
    //属性值
    public int carrotHP = 10;//萝卜血量
    public int coin;//金钱
    public int gameSpeed;//游戏速度
    public bool isPause;//是否暂停

    public bool creatingMonster;//是否继续产生怪物
    public bool gameOver;
    public float[,] attackRadius;//攻击半径
    //建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;
    //建塔有关的成员变量
    public Dictionary<int, int> towerPriceDict;//建塔价格表
    public GameObject towerListGo;//建塔按钮列表
    public GameObject handleTowerCanvasGo;//处理塔的升级与买卖的画布

    //游戏资源
    public RuntimeAnimatorController[] controllers;//怪物的动画播放控制器
    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        isPause = true;//游戏刚开始要暂停，等待三秒
        gameSpeed = 1;
        killMonsterTotalNum = 0;
        timeMonster = 0.7f;
        monsterBuilder = new MonsterBuilder();
        towerBuilder = new TowerBuilder();
        BGSpriteRenderer = transform.Find("BG").GetComponent<SpriteRenderer>();
        PathSpriteRenderer = transform.Find("Square").GetComponent<SpriteRenderer>();
        attackRadius = new float[,]
        {
            {0,0,0,0 },
            {0,1.5f,2,2.5f },
            {0,1.5f,2,2.5f },
            {0,1.5f,2,2.5f },
            {0,1.5f,2,2.5f },
            {0,1.5f,2,2.5f },
        };
        towerPriceDict = new Dictionary<int, int>()
        {
            {1,100 },
            {2,120 },
            {3,140 },
            {4,160 },
            {5,160 },
        };
        //测试代码
        //currentStage = new Stage(true, 10, 5, new int[] { 1, 2, 3,4 ,5}, false, 1, 1, 1, false);
        currentStage = mGameManager.currentStage;
        normalModelPanel = mGameManager.uiManager.mUIFacade.currentSencePanelDist[StringManager.NormalModelPanel] as NormalModelPanel;
        //可建塔列表
        for(int i = 0; i < currentStage.mTowerIDListLenght; i++)
        {
            GameObject itemGo = mGameManager.GetGameObject(FactoryType.UIFactory, "Btn_TowerBuild");
            itemGo.transform.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
            itemGo.transform.SetParent(towerListGo.transform);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
            
        }
        
#endif
    }
    private void Start()
    {
#if Game
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.mBigLevelID, currentStage.mLevelID);
        level = new Level(mapMaker.roundNum,mapMaker.roundInfoList);
        controllers = new RuntimeAnimatorController[13];

        for(int i = 1; i <= controllers.Length-1; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("Animator/AnimatorController/Monster/" + mapMaker.bigLevelID.ToString() + "/" + i.ToString());
        }
        normalModelPanel.EnterPanel();
#endif
    }
#if Game
    private void Update()
    {

        if (!isPause)//游戏是否暂停
        {
            //产怪逻辑
            if (killMonsterNum >= mMonsterIDList.Count)
            {
                //当前回合怪物已经杀完，添加当前回合数的索引
                AddRoundNum();
                timeMonster = 2f;//两波怪物之间间隔三秒
            }
            else
            {
                timeMonster -= (Time.deltaTime*gameSpeed);
                if (timeMonster <= 0)
                {
                    InstantiateMonster();
                    timeMonster = 1f;
                }
            }
        }
    }
   
    //开始游戏
    public void StartGame()
    {
        isPause = false;
    }
    //判断道具是否全部清除
    public bool IfAllClear()
    {           
        int i,j;
        for (i = 1; i <= 8; i++)
        {
            for(j = 1; j <= 12; j++)
            {
                if (mapMaker.gridPoints[i, j].gridState.hasItem) break;
            }
            if (j <= 12) break;
        }
        if (i <= 8) return false;return true;
    }
    //判断当前萝卜状态
    public int GetCarrotState()
    {
        if (carrotHP == 10) return 1;
        if (carrotHP >= 6) return 2;
        return 3;
    }
    //更改金币数量
    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        //更新游戏UI显示
        normalModelPanel.topPage.UpdateCoinText();
    }
    //具体产怪的方法
    public void InstantiateMonster()
    {
        
        //产生怪物
        if (mMonsterIDIndex < mMonsterIDList.Count)
        {
            //产生特效
            GameObject effectGo = GetGameObject("CreateEffect");
            effectGo.transform.SetParent(transform);
            effectGo.transform.position = new Vector3( (float)mapMaker.monsterPoint[0].x,(float)mapMaker.monsterPoint[0].y,-3);
            //播放产生怪物音效
            GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Monster/Create");

            monsterBuilder.m_monsterID = mMonsterIDList[mMonsterIDIndex];
            mMonsterIDIndex++;

            GameObject monsterGo = monsterBuilder.GetProduct();
            monsterGo.transform.SetParent(transform);
            monsterGo.transform.position = new Vector3((float)mapMaker.monsterPoint[0].x, (float)mapMaker.monsterPoint[0].y, -3);
        }
        else
        {
            
        }
    }
    //增加当前回合数,进入下一个回合
    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
        //更新有关回合显示的ui
        normalModelPanel.topPage.UpdateRoundNum();
    }
    //格子处理的方法
    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canTower)
        {
            if (selectGrid == null)
            {
                selectGrid = grid;
                grid.ShowGrid();
            }
            else
            {
                
                    selectGrid.HideGrid();
                    selectGrid = null;
              
            }
        }
        else
        {
            if (selectGrid != null)
            {
                selectGrid.HideGrid();
                selectGrid = null;
            }
            else
            {
                grid.CantTower();
            }
        }

    }
    //打开礼物page
    public void ShowPrizePage()
    {
        normalModelPanel.ShowPrizePage();
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/GiftGot");
    }
    //显示最后一波怪
    public void ShowFinalWaveUI()
    {
        normalModelPanel.ShowFinalWaveUI();
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Finalwave");
    }
    //游戏胜利方法
    public void GameWin()
    {
        gameOver = true;
        normalModelPanel.ShowWinPage();
        isPause = true;
        //播放胜利音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Perfect");
    }
    public void GameOver()
    {
        gameOver = true;
        normalModelPanel.ShowGameOverPage();
        isPause = true;
        //播放失败音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Lose");
    }
    //与集火目标有关的方法
    //显示标记
    public void ShowSignal()
    {
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.gridHeight / 2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
        //播放音效
        PlayEffectMusic("AudioClips/NormalMordel/Tower/ShootSelect");
    }
    //隐藏标记
    public void HideSignal()
    {
        targetSignal.SetActive(false);
        targetTrans = null;
    }
    //获取资源的一些方法
    public Sprite GetSprite(string resorcesPath)
    {
        return mGameManager.GetSprite(resorcesPath);
    }
    public AudioClip GetAudioClip(string resorcesPath)
    {
        return mGameManager.GetAudioClip(resorcesPath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resorcesPath)
    {
        return mGameManager.GetRuntimeAnimatorController(resorcesPath);
    }
    public GameObject GetGameObject(string name)
    {
        return mGameManager.GetGameObject(FactoryType.GameFactory, name);
    }
    public void PushGameObjectToFactory(string name, GameObject gameObject)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.GameFactory, name, gameObject);
    }
    public void PlayEffectMusic(string path)
    {
        mGameManager.audioSourceManager.PlayEffectMusic(mGameManager.GetAudioClip(path));
    }
   
#endif
}
