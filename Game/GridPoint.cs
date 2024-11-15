using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class GridPoint : MonoBehaviour
{
    

    //资源
    private Sprite gridSprite;
    private Sprite startSprite;//开始时格子的图片显示
    private Sprite cantBuildSprite;

    //引用
    private GameController gameController;
    private GameObject towerListGo;
    private GameObject handleTowerCanvasGo;//有塔的画布
    public GameObject attackRangeGo;//塔的攻击范围显示
    private Transform upLevelButtonTrans;
    private Transform sellTowerButtonTrans;
    private Vector3 upLevelButtonInitPos;
    private Vector3 sellTowerButtonInitPos;
    public Tower tower;//当前格子里面的塔
    private GameObject UPLevelSignal;//升级信号
#if Tool
    public Sprite monsterPointSprite;
    public GameObject[] itemPrefabs;//所有道具
    public GameObject currentItem;//当前道具
#endif
    public struct GridState
    {
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
        public bool canTower;
    }

    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
        public double x;
        public double y;
    }
    //属性
    public GridState gridState;
    public GridIndex gridIndex;
    public SpriteRenderer spriteRenderer;
    public bool hasTower;

    Tween tween;
    private void Awake()
    {
#if Tool
        gridSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPointSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/1-1");
        itemPrefabs = new GameObject[10];
        string loadPath = "Prefabs/Game/";
        for(int i = 0; i < 10; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(loadPath +MapMaker.Instance.bigLevelID.ToString()+"/Item/"+ i.ToString());
            if (itemPrefabs[i] == null) Debug.Log("加载失败路径："+ loadPath + MapMaker.Instance.bigLevelID.ToString() + "/Item/" + i.ToString());
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();//初始化
#if Game
        gameController = GameController.Instance;
        gridSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Grid");
        startSprite = gameController.GetSprite("Pictures/NormalMordel/Game/StartSprite");
        cantBuildSprite = gameController.GetSprite("Pictures/NormalMordel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0.2f), 3);
        t.OnComplete(ChangeSprite);
        towerListGo = gameController.towerListGo;
        handleTowerCanvasGo = gameController.handleTowerCanvasGo;
        sellTowerButtonTrans = handleTowerCanvasGo.transform.Find("Btn_SellTower").transform;
        upLevelButtonTrans = handleTowerCanvasGo.transform.Find("Btn_UpLevel").transform;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        upLevelButtonInitPos = upLevelButtonTrans.localPosition;
        UPLevelSignal = transform.Find("UPLevel").gameObject;
        UPLevelSignal.SetActive(false);
#endif
    }

#if Game
    private void Update()
    {
        if (UPLevelSignal != null)
        {
            if (tower != null)
            {
                if (tower.towerPersonalProperty.upLevelPrice <= gameController.coin&&tower.towerPersonalProperty.towerLevel<3)
                {
                    UPLevelSignal.SetActive(true);
                }
                else
                {
                    UPLevelSignal.SetActive(false);
                }
            }
            else
            {
                UPLevelSignal.SetActive(false);
            }
        }
    }
#endif
    //改回原来样式的sprite
    private void ChangeSprite()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        if (gridState.canTower)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBuildSprite;
        }
    }
    public void InitGrid()
    {
#if Tool
        spriteRenderer.sprite = gridSprite;
        if (currentItem != null) { Destroy(currentItem); currentItem = null; }
#endif
        gridState.canTower = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Game
        tower = null;
        hasTower = false;
#endif
    }
#if Game
    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canTower)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
    public void CreateItem()//创建道具
    {
        GameObject itemGo = GameController.Instance.GetGameObject(GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/"+gridState.itemID.ToString());
        itemGo.transform.SetParent(GameController.Instance.transform);
        itemGo.transform.position = transform.position+new Vector3(0,0,-3);
        if (gridState.itemID <= 2)
            itemGo.transform.position += new Vector3(GameController.Instance.mapMaker.gridWidth / 2, GameController.Instance.mapMaker.gridHeight / 2, 0);
        else
            if (gridState.itemID <= 4) itemGo.transform.position += new Vector3(GameController.Instance.mapMaker.gridWidth / 2, 0, 0);

        itemGo.GetComponent<Item>().gridPoint = this;
    }
    //建塔后的处理方法
    public void AfterBuild()
    {
        spriteRenderer.enabled = false;
        //对塔的后续处理
    }
    private void OnMouseDown()
    {
        //选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())//判断是否点击到ui
        {
            return;
        }
        else
        {
            gameController.HandleGrid(this);
        }
    }
    //显示格子
    public void ShowGrid()
    {
        if (!hasTower)
        {
            spriteRenderer.enabled = true;
            
            //显示建塔列表
            towerListGo.transform.position = transform.position+CorrectTowerListGoPos();
            towerListGo.SetActive(true);
        }
        else
        {
            CorrectHandleTowerCanvasGoPos();
            handleTowerCanvasGo.transform.position = transform.position;
            handleTowerCanvasGo.SetActive(true);

            attackRangeGo.SetActive(true);
        }
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Grid/GridSelect");
    }
    //隐藏格子
    public void HideGrid()
    {
        if (!hasTower)
        {
            //隐藏建塔列表
            towerListGo.SetActive(false);
        }
        else
        {
            handleTowerCanvasGo.SetActive(false);
            attackRangeGo.SetActive(false);
        }
        spriteRenderer.enabled = false;
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Grid/GridDeselect");
    }
    //显示不能建塔图片
    public void CantTower()
    {
        tween.Kill();//每次进来都先停止tween动画，不管tween动画有没有，连续点击时可以清楚上一次的动画
        spriteRenderer.color = new Color(1, 1, 1, 1);
        spriteRenderer.enabled = true;
        tween = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 1f);
        tween.OnComplete(CantTowerTween);
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Grid/SelectFault");
    }

    private void CantTowerTween()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private Vector3 CorrectTowerListGoPos()
    {
        Vector3 w = new Vector3();
        w = Vector3.zero;
        //判断塔列表的数量是否大于5个
        int k = 1;
        if (gameController.currentStage.mTowerIDListLenght > 5) k = 2;
        if (gridIndex.xIndex <= 4 && gridIndex.xIndex >= 1)
        {
            w += new Vector3(k*gameController.mapMaker.gridWidth, 0, 0);
        }
        if (gridIndex.xIndex >= 9 && gridIndex.xIndex <= 12)
        {
            w += new Vector3(-k*gameController.mapMaker.gridWidth, 0, 0);
        }
        if (gridIndex.yIndex <= 4 && gridIndex.yIndex >= 1)
        {
            w += new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }
        if (gridIndex.yIndex >=5 && gridIndex.yIndex <= 8)
        {
            w += new Vector3(0, -gameController.mapMaker.gridHeight, 0);
        }
        return w;
    }
    private void CorrectHandleTowerCanvasGoPos()
    {
        upLevelButtonTrans.localPosition = upLevelButtonInitPos;
        sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        if (gridIndex.yIndex == 1)
        {
            if (gridIndex.xIndex <= 6) sellTowerButtonTrans.localPosition += new Vector3(100, 100, 0);
            if (gridIndex.xIndex > 6) sellTowerButtonTrans.localPosition += new Vector3(-100, 100, 0);
        }
        if (gridIndex.yIndex == 7)
        {
            if (gridIndex.xIndex <= 6) upLevelButtonTrans.localPosition += new Vector3(100, -100, 0);
            if (gridIndex.xIndex > 6) upLevelButtonTrans.localPosition += new Vector3(-100, -100, 0);
        }
    }
#endif

#if Tool
    public GameObject CreateItem()//创建道具
    {
        Vector2 itemPos = transform.position;
        GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID],itemPos,transform.rotation);
        itemGo.transform.position += new Vector3(0, 0, 1);
        if(gridState.itemID<=2)
        itemGo.transform.position += new Vector3(MapMaker.Instance.gridWidth / 2, MapMaker.Instance.gridHeight / 2,0);
        else
            if(gridState.itemID <= 4) itemGo.transform.position += new Vector3(MapMaker.Instance.gridWidth / 2,0,0);

        currentItem = itemGo;//注意赋值
        return itemGo;
    }
#endif
#if Tool
    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.A))//怪物路点
        {
            if (gridState.hasItem) return;//如果已经有物体就跳出
            gridState.isMonsterPoint = !gridState.isMonsterPoint;

            if (gridState.isMonsterPoint)//当前是怪物路点
            {
                spriteRenderer.enabled = true;
                MapMaker.Instance.monsterPoint.Add(gridIndex);
                spriteRenderer.sprite = monsterPointSprite;
                gridState.canTower = false;
            }
            else
            {
                MapMaker.Instance.monsterPoint.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                spriteRenderer.enabled = false;
                gridState.canTower = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))//建道具
            {
                if (gridState.isMonsterPoint)//如果要建的地方是怪物路点
                {


                    return ;
                }
                if (currentItem != null)//如果当前点已经有建筑
                {
                    Destroy(currentItem);
                }
                gridState.itemID = (gridState.itemID + 1) % itemPrefabs.Length;
                CreateItem();//创建道具
                gridState.hasItem = true;
            }
            else
            if(Input.GetKey(KeyCode.Space))
            {
                if (gridState.hasItem)
                {
                    gridState.hasItem = false;
                    gridState.itemID = -1;
                    Destroy(currentItem);
                }
            }
            else
            {
                if (!gridState.isMonsterPoint)
                {
                    gridState.canTower = !gridState.canTower;
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                }
            }
        }
    }
#endif
}
