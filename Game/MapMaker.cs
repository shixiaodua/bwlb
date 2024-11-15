using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
public class MapMaker : MonoBehaviour
{
#if Tool
    public GameObject gridGo;
    public bool drawLine=true;
    private static MapMaker _instance;
    public static MapMaker Instance { get => _instance; }

#endif
    //��С�ؿ�����
    public int bigLevelID = 1;
    public int LevelID = 1;

    //ȫ���ĸ��Ӷ���
    public GridPoint[,] gridPoints;

    //����·����
    public List<GridPoint.GridIndex> monsterPoint;

    //����·����ľ���λ��?
    public Vector3[,] PointPos = new Vector3[10, 16];



    //��ǰ�ؿ�������·��
    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    [HideInInspector]
    public float gridWidth;//���ӿ�
    [HideInInspector]
    public float gridHeight;//���Ӹ�
    private float mapWidth;//��ͼ��
    private float mapHeight;//��ͼ��

    private int xColumn = 12;//������
    private int yRow = 8;//������

    //���ﲨ����Ϣ
    public int roundNum;//��������
    public Round.RoundInfo[] roundInfoList;//ID

    public Carrot carrot;

    private void Awake()
    {
#if Tool
        _instance = this;
#endif
        CalcultateSize();
        gridPoints = new GridPoint[10, 15];
        monsterPoint = new List<GridPoint.GridIndex>();
        roundInfoList = new Round.RoundInfo[100];
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Square").GetComponent<SpriteRenderer>();
        for (int i = 1; i <= yRow; i++)
        {
            for (int j = 1; j <= xColumn; j++)
            {
#if Tool
                GameObject grid = Instantiate(gridGo, transform.position, transform.rotation);
#endif

#if Game
                GameObject grid = GameController.Instance.GetGameObject("grid");
                Debug.Log(grid);
#endif
                grid.transform.SetParent(transform);
                grid.transform.position = new Vector2(-mapWidth / 2 + gridWidth / 2 + (j - 1) * gridWidth, -mapHeight / 2 + gridHeight / 2 + (i - 1) * gridHeight);
                GridPoint.GridIndex gridIndex;
                gridIndex.xIndex = j;
                gridIndex.yIndex = i;
                gridIndex.x = grid.transform.position.x;
                gridIndex.y = grid.transform.position.y;
                grid.GetComponent<GridPoint>().gridIndex = gridIndex;
                gridPoints[i, j] = grid.GetComponent<GridPoint>();
                PointPos[i, j] = new Vector2(-mapWidth / 2 + gridWidth / 2 + (j - 1) * gridWidth, -mapHeight / 2 + gridHeight / 2 + (i - 1) * gridHeight);

            }
        }
        InitMapMaker();
        //InitInfo();
#if Tool
        LoadCurrentLevel(1, 1);
#endif
    }

#if Game
    //���ص�ͼ
    public void LoadMap(int bigLev, int lev)
    {
        //���ùؿ�����
        bigLevelID = bigLev;
        LevelID = lev;
        //���عؿ�ͼƬ
        GameController.Instance.BGSpriteRenderer.sprite = GameController.Instance.GetSprite("Pictures/NormalMordel/Game/" + bigLev.ToString() + "/BG" + lev.ToString());
        GameController.Instance.PathSpriteRenderer.sprite = GameController.Instance.GetSprite("Pictures/NormalMordel/Game/" + bigLev.ToString() + "/Road" + lev.ToString());
        //�����ļ���Դ
       // string path = "D:/unity duqi/bwlb/Assets/StreamingAssets/Json/Level/" + bigLev.ToString() + "/" + lev.ToString() + ".json";
        string relativePath = "/Json/Level/" + bigLev.ToString() + "/" + lev.ToString() + ".json";
        Debug.Log(Application.streamingAssetsPath);
string path = Application.streamingAssetsPath+relativePath;
        string json = File.ReadAllText(path);
        LevelInfo levelInfo = new LevelInfo();
        levelInfo = JsonMapper.ToObject<LevelInfo>(json);
        //���¹ؿ�������?
        monsterPoint = levelInfo.monsterPoint;
        roundNum = levelInfo.roundNum;
        roundInfoList = levelInfo.roundInfos;
        UpdateGrid(levelInfo);
        //foreach(var i in monsterPoint)
        //{
        //    Debug.Log(i.x+" "+ i.y);
        //}
        //������յ�?
        GameObject startPointGo = GameController.Instance.GetGameObject("StartPoint");
        startPointGo.transform.position = new Vector3((float)(monsterPoint[0].x), (float)(monsterPoint[0].y), 0);
        startPointGo.transform.SetParent(transform);
        GameObject endPointGo = GameController.Instance.GetGameObject("Carrot");
        GameController.Instance.carrot = endPointGo.GetComponent<Carrot>();//��ֵ
        endPointGo.transform.position = new Vector3((float)(monsterPoint[monsterPoint.Count - 1].x), (float)(monsterPoint[monsterPoint.Count - 1].y), -4);
        endPointGo.transform.SetParent(transform);
        carrot = endPointGo.GetComponent<Carrot>();
    }
#endif

    //���뵱ǰ�ؿ�
    public void LoadCurrentLevel(int bigLev, int lev)
    {

        bigLevelID = bigLev;
        LevelID = lev;
        //��ȡ���ļ���Ϣ
        string path = "D:/unity duqi/bwlb/Assets/StreamingAssets/Json/Level/" + bigLev.ToString() + "/" + lev.ToString() + ".json";
        string json = File.ReadAllText(path);
        LevelInfo levelInfo = new LevelInfo();
        levelInfo = JsonMapper.ToObject<LevelInfo>(json);


        //���µ�ǰ��ͼ
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLev.ToString() + "/BG" + lev.ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLev.ToString() + "/Road" + lev.ToString());

        //���¸���״̬����·���Լ�������ʾ
        for (int y = 1; y <= yRow; y++)
        {
            for (int x = 1; x <= xColumn; x++)
            {
                gridPoints[y, x].gridState = levelInfo.gridStates[(y - 1) * 12 + x];
#if Tool
                if (gridPoints[y,x].gridState.isMonsterPoint)//�ǹ���·��
                {
                    gridPoints[y,x].spriteRenderer.sprite = gridPoints[y,x].monsterPointSprite;
                }
                if(gridPoints[y,x].gridState.hasItem)//�е���
                {
                    gridPoints[y,x].CreateItem();
                }
                if (gridPoints[y, x].gridState.canTower) gridPoints[y, x].spriteRenderer.enabled = true;
                else
                    if(gridPoints[y,x].gridState.isMonsterPoint)
                    gridPoints[y, x].spriteRenderer.enabled = true;
                else
                gridPoints[y, x].spriteRenderer.enabled = false;

#endif
            }
        }

        //����������Ϣ
        monsterPoint = levelInfo.monsterPoint;
        roundNum = levelInfo.roundNum;
        roundInfoList = levelInfo.roundInfos;
        foreach (var i in monsterPoint)
        {
            Debug.Log(i.x + " " + i.y);
        }
    }

    public void UpdateGrid(LevelInfo levelInfo)
    {
        //���¸���״̬�Լ�������ʾ
        for (int y = 1; y <= yRow; y++)
        {
            for (int x = 1; x <= xColumn; x++)
            {
                gridPoints[y, x].gridState = levelInfo.gridStates[(y - 1) * 12 + x];

                if (gridPoints[y, x].gridState.hasItem)//�е���
                {
                    gridPoints[y, x].CreateItem();
                }
                if (gridPoints[y, x].gridState.canTower) gridPoints[y, x].spriteRenderer.enabled = true;
                else
                    if (gridPoints[y, x].gridState.isMonsterPoint)
                    gridPoints[y, x].spriteRenderer.enabled = true;
                else
                    gridPoints[y, x].spriteRenderer.enabled = false;
            }
        }
    }
    public void ExitCurrentLevel()
    {
        SaveInfo();//�����ļ���Ϣ
        InitMapMaker();
        InitMapPoint();
    }

    public void SaveInfo()
    {
        LevelInfo levelInfo = new LevelInfo();
        levelInfo.bigLevelID = bigLevelID;
        levelInfo.LevelID = LevelID;
        levelInfo.roundNum = roundNum;
        levelInfo.roundInfos = roundInfoList;

        levelInfo.monsterPoint = monsterPoint;
        for (int i = 1; i <= yRow; i++)
        {
            for (int j = 1; j <= xColumn; j++)
            {
                levelInfo.gridStates[(i - 1) * 12 + j] = gridPoints[i, j].gridState;
            }
        }
        //д���ļ�
        WriteToFile("D:/unity duqi/bwlb/Assets/StreamingAssets/Json/Level/" + bigLevelID.ToString() + "/" + LevelID.ToString() + ".json", levelInfo);
    }

    public void WriteToFile(string path, LevelInfo levelInfo)
    {
        StreamWriter sw = new StreamWriter(path);
        string levelInfoToJson = JsonMapper.ToJson(levelInfo);
        sw.Write(levelInfoToJson);
        sw.Close();
    }
    //��ʼ����ͼ���?
    public void InitMapPoint()
    {
        for (int y = 1; y <= yRow; y++)
        {
            for (int x = 1; x <= xColumn; x++)
            {
                gridPoints[y, x].InitGrid();
            }
        }
    }

    //��ʼ����ͼ�༭���е�����
    public void InitMapMaker()
    {
        monsterPoint.Clear();
        Array.Clear(roundInfoList, 0, roundInfoList.Length);
    }

    //�����ͼ������һ�����ӵĿ���?
    private void CalcultateSize()
    {
        Vector2 leftDown = new Vector2(0, 0);
        Vector2 RightUp = new Vector2(1, 1);
        Vector2 posOne = Camera.main.ViewportToWorldPoint(leftDown);
        Vector2 posTow = Camera.main.ViewportToWorldPoint(RightUp);
        mapHeight = posTow.y - posOne.y;
        mapWidth = posTow.x - posOne.x;

        gridHeight = mapHeight / yRow;
        gridWidth = mapWidth / xColumn;

    }

#if Tool
    private void OnDrawGizmos()
    {
        if (drawLine)
        {
            CalcultateSize();

            Gizmos.color = Color.red;
            
            for(int i = 0; i <= yRow; i++)
            {
                Vector2 start = new Vector2(-mapWidth / 2, -mapHeight / 2 + i * gridHeight);
                Vector2 end = new Vector2(mapWidth / 2, -mapHeight / 2 + i * gridHeight);
                Gizmos.DrawLine(start, end);
            }

            for(int i = 0; i <= xColumn; i++)
            {
                Vector2 start = new Vector2(-mapWidth / 2 + i * gridWidth, -mapHeight / 2);
                Vector2 end = new Vector2(-mapWidth / 2 + i * gridWidth, mapHeight / 2);
                Gizmos.DrawLine(start, end);
            }
        }
    }
#endif
    //�������·��?
    public void ClearMonsterPath()
    {
        for (int i = 0; i < monsterPoint.Count; i++)
        {
            gridPoints[monsterPoint[i].yIndex, monsterPoint[i].xIndex].InitGrid();
        }
        monsterPoint.Clear();
    }

    //�ָ���ʼ״̬
    public void RecoverTowerPoint()
    {
        ClearMonsterPath();
        for (int i = 1; i <= yRow; i++)
        {
            for (int j = 1; j <= xColumn; j++)
            {
                gridPoints[i, j].InitGrid();
            }
        }
    }

#if Tool
    public void InitInfo()
{
    LevelInfo levelInfo = new LevelInfo();
        for (int y = 1; y <= yRow; y++)
    {
        for (int x = 1; x <= xColumn; x++)
        {
            levelInfo.gridStates[(y-1)*12+x] = gridPoints[y, x].gridState;
        }
    }

    for (int i = 1; i <= 15; i++)
    {
        int biglev = (i - 1) / 5 + 1;
        int lev = i - (biglev - 1) * 5;

        string path = "D:/unity duqi/bwlb/Assets/StreamingAssets/Json/Level/" + biglev.ToString() + "/" + lev.ToString() + ".json";
        levelInfo.bigLevelID = biglev;
        levelInfo.LevelID = lev;
        StreamWriter sw = new StreamWriter(path);
        string levelInfoToJson = JsonMapper.ToJson(levelInfo);
        sw.Write(levelInfoToJson);
        sw.Close();
    }

}
#endif
}

