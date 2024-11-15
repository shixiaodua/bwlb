using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//������Ϸ���ƣ�������Ϸ�������߼�
public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get => _instance; }
    //����
    public Level level;
    private GameManager mGameManager;
    public List<int> mMonsterIDList;//��ǰ���εĹ���id�б�
    public MapMaker mapMaker;
    public Stage currentStage;//��ǰ�ؿ���Ϣ
    public Transform targetTrans;//����Ŀ��
    public GameObject targetSignal;//�����ź�
    public GridPoint selectGrid;//��ǰѡ��ĸ���
    public Carrot carrot;//�ܲ��ű�
    public SpriteRenderer BGSpriteRenderer;//����ͼ
    public SpriteRenderer PathSpriteRenderer;//·��ͼ

    public NormalModelPanel normalModelPanel;//��Ϸui���
    
    public int mMonsterIDIndex;//��ǰ������������
    //���ڼ����ĳ�Ա����
    public int killMonsterNum;//��ǰ����ɱ����������
    public int killMonsterTotalNum;//ɱ����������
    public int clearItemNum;//���ٵ�������
    public float timeMonster;//���ּ��ʱ��
    //����ֵ
    public int carrotHP = 10;//�ܲ�Ѫ��
    public int coin;//��Ǯ
    public int gameSpeed;//��Ϸ�ٶ�
    public bool isPause;//�Ƿ���ͣ

    public bool creatingMonster;//�Ƿ������������
    public bool gameOver;
    public float[,] attackRadius;//�����뾶
    //������
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;
    //�����йصĳ�Ա����
    public Dictionary<int, int> towerPriceDict;//�����۸��
    public GameObject towerListGo;//������ť�б�
    public GameObject handleTowerCanvasGo;//�������������������Ļ���

    //��Ϸ��Դ
    public RuntimeAnimatorController[] controllers;//����Ķ������ſ�����
    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        isPause = true;//��Ϸ�տ�ʼҪ��ͣ���ȴ�����
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
        //���Դ���
        //currentStage = new Stage(true, 10, 5, new int[] { 1, 2, 3,4 ,5}, false, 1, 1, 1, false);
        currentStage = mGameManager.currentStage;
        normalModelPanel = mGameManager.uiManager.mUIFacade.currentSencePanelDist[StringManager.NormalModelPanel] as NormalModelPanel;
        //�ɽ����б�
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

        if (!isPause)//��Ϸ�Ƿ���ͣ
        {
            //�����߼�
            if (killMonsterNum >= mMonsterIDList.Count)
            {
                //��ǰ�غϹ����Ѿ�ɱ�꣬��ӵ�ǰ�غ���������
                AddRoundNum();
                timeMonster = 2f;//��������֮��������
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
   
    //��ʼ��Ϸ
    public void StartGame()
    {
        isPause = false;
    }
    //�жϵ����Ƿ�ȫ�����
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
    //�жϵ�ǰ�ܲ�״̬
    public int GetCarrotState()
    {
        if (carrotHP == 10) return 1;
        if (carrotHP >= 6) return 2;
        return 3;
    }
    //���Ľ������
    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        //������ϷUI��ʾ
        normalModelPanel.topPage.UpdateCoinText();
    }
    //������ֵķ���
    public void InstantiateMonster()
    {
        
        //��������
        if (mMonsterIDIndex < mMonsterIDList.Count)
        {
            //������Ч
            GameObject effectGo = GetGameObject("CreateEffect");
            effectGo.transform.SetParent(transform);
            effectGo.transform.position = new Vector3( (float)mapMaker.monsterPoint[0].x,(float)mapMaker.monsterPoint[0].y,-3);
            //���Ų���������Ч
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
    //���ӵ�ǰ�غ���,������һ���غ�
    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
        //�����йػغ���ʾ��ui
        normalModelPanel.topPage.UpdateRoundNum();
    }
    //���Ӵ���ķ���
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
    //������page
    public void ShowPrizePage()
    {
        normalModelPanel.ShowPrizePage();
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/GiftGot");
    }
    //��ʾ���һ����
    public void ShowFinalWaveUI()
    {
        normalModelPanel.ShowFinalWaveUI();
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Finalwave");
    }
    //��Ϸʤ������
    public void GameWin()
    {
        gameOver = true;
        normalModelPanel.ShowWinPage();
        isPause = true;
        //����ʤ����Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Perfect");
    }
    public void GameOver()
    {
        gameOver = true;
        normalModelPanel.ShowGameOverPage();
        isPause = true;
        //����ʧ����Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Lose");
    }
    //�뼯��Ŀ���йصķ���
    //��ʾ���
    public void ShowSignal()
    {
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.gridHeight / 2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
        //������Ч
        PlayEffectMusic("AudioClips/NormalMordel/Tower/ShootSelect");
    }
    //���ر��
    public void HideSignal()
    {
        targetSignal.SetActive(false);
        targetTrans = null;
    }
    //��ȡ��Դ��һЩ����
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
