using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameNormalLevelPanel : BasePanel
{
    public int currentBigLevelID;

    private string filePath;
    public int currentLevelID;
    private Transform levelContentTrans;
    private GameObject img_LockBtnGo;
    private Transform emp_TowerTrans;
    private Image img_BGLeft;
    private Image img_BGRight;
    private Text tex_TotalWaves;
    private PlayerManager playerManager;
    private SlideScrollView slideScrollView;
    private List<GameObject> levelContentImageGos;//��ǰ��ؿ���Ӧ��С�ؿ��ĵ�ͼ��Ƭui
    private List<GameObject> towerContentImageGos;//�����б�ui

    protected override void Awake()
    {
        base.Awake();
        filePath = "Pictures/GameOption/Normal/Level/";
        playerManager = mUIFacade.mPlayerManager;
       
        levelContentImageGos = new List<GameObject>();
        towerContentImageGos = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View/Viewport/Content");
        img_LockBtnGo = transform.Find("Img_Btn").gameObject;
        emp_TowerTrans = transform.Find("Emp_Tower");
        img_BGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        img_BGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        tex_TotalWaves = transform.Find("Img_TotalWaves/Text (Legacy)").GetComponent<Text>();
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        currentBigLevelID = 1;
        currentLevelID = 1;
        LoadResource();
    }

    public void LoadResource()
    {
        mUIFacade.GetSprite(filePath + "AllClear");
        mUIFacade.GetSprite(filePath + "Carrot_1");
        mUIFacade.GetSprite(filePath + "Carrot_2");
        mUIFacade.GetSprite(filePath + "Carrot_3");
        for(int i = 1; i < 4; i++)
        {
            string spriteLoadPath = filePath + i.ToString()+"/";
            mUIFacade.GetSprite(spriteLoadPath + "BG_Left");
            mUIFacade.GetSprite(spriteLoadPath + "BG_Right");
            for(int j = 1; j < 6; j++)
            {
                mUIFacade.GetSprite(spriteLoadPath + "Level_" + j.ToString());
            }
        }
        
        for(int i = 1; i < 13; i++)
        {
            mUIFacade.GetSprite(filePath + "Tower/Tower_" + i.ToString());
        }
    }

    //���µ�ͼui
    public void UpdateMapUI(string spriteLoadPath)
    {
        
        img_BGLeft.sprite = mUIFacade.GetSprite(spriteLoadPath + "BG_Left");
        img_BGRight.sprite = mUIFacade.GetSprite(spriteLoadPath + "BG_Right");
        
        for(int i = 1; i <=5 ; i++)
        {
            
            GameObject currentUI = CreateUIAndSetPosition("Img_Level", levelContentTrans);
            levelContentImageGos.Add(currentUI);
            UpdateLevelPage(currentUI, playerManager.unLpckdeNormalModelLevel[(currentBigLevelID-1)*5+i-1]);
        }
    }
    public void ClearLevelList()
    {
        foreach (var item in levelContentImageGos)
        {
            mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Level", item);
        }
        levelContentImageGos.Clear();
    }
    public void ClearTowerList()
    {
        Transform[] children =emp_TowerTrans.GetComponentsInChildren<Transform>();

        // ������ 1 ��ʼ����������������
        for (int i = 1; i < children.Length; i++)
        {
            // ����������
            Destroy(children[i].gameObject);
        }
        towerContentImageGos.Clear();
    }
    //���¾�̬ui
    public void UpdateStaticUI(int currentLevel)
    {
        currentLevelID = currentLevel;
        if (towerContentImageGos.Count > 0) ClearTowerList();

        Stage stage = playerManager.unLpckdeNormalModelLevel[(currentBigLevelID-1)*5+currentLevel-1];
        tex_TotalWaves.text = stage.mTotalRound.ToString();

        for(int i = 0; i < stage.mTowerIDListLenght; i++)
        {
            GameObject towerObjeck = new GameObject();
            Sprite towerSprite = mUIFacade.GetSprite(filePath + "Tower/Tower_" + stage.mTowerIDList[i].ToString());
            towerObjeck.AddComponent<Image>().sprite = towerSprite;
            towerObjeck.transform.SetParent(emp_TowerTrans);
            towerContentImageGos.Add(towerObjeck);
        }

        if (stage.munLocked)
        {
            img_LockBtnGo.SetActive(false);
        }
        else
        {
            img_LockBtnGo.SetActive(true);
        }
    }
    //����ĳ��С�ؿ������
    public void UpdateLevelPage(GameObject map,Stage stage)
    {

        if (!stage.munLocked)//δ����
        {
            map.transform.Find("Img_AllClear").gameObject.SetActive(false);
            map.transform.Find("Img_Carrot").gameObject.SetActive(false);
            if (stage.mIsRewardLevel)//�ǽ�����
            {
                map.transform.Find("Img_Lock").gameObject.SetActive(false);
                map.transform.Find("Img_BG").gameObject.SetActive(true);
            }
            else//���ǽ�����
            {
                map.transform.Find("Img_Lock").gameObject.SetActive(true);
                map.transform.Find("Img_BG").gameObject.SetActive(false);
            }
        }
        else//����
        {
            map.transform.Find("Img_Lock").gameObject.SetActive(false);
            map.transform.Find("Img_BG").gameObject.SetActive(false);

            //����ͨ��ͼƬ
            if (stage.mCarrotState != 0)//�п����ǳ�ʼ�ؿ��������˵���û��ͨ��
            {
                Sprite carrotSprite = mUIFacade.GetSprite(filePath + "Carrot_" + stage.mCarrotState.ToString());
                map.transform.Find("Img_Carrot").GetComponent<Image>().sprite = carrotSprite;
                map.transform.Find("Img_Carrot").gameObject.SetActive(true);
            }
            else
            {
                map.transform.Find("Img_Carrot").gameObject.SetActive(false);
            }
            if (stage.mAllClear)//ȫ�����
            {
                map.transform.Find("Img_AllClear").gameObject.SetActive(true);
            }
            else//δȫ�����
            {
                map.transform.Find("Img_AllClear").gameObject.SetActive(false);
            }
        }

        map.GetComponent<Image>().sprite = mUIFacade.GetSprite(filePath + currentBigLevelID.ToString() + "/Level_" + stage.mLevelID.ToString());
    }
    public void ToThisPanel(int currentBigLevel)
    {
         currentBigLevelID = currentBigLevel;
         currentLevelID = 1;
         UpdateMapUI(filePath + currentBigLevel.ToString()+"/");
         UpdateStaticUI(currentLevelID);
         gameObject.SetActive(true);
    }

    public override void InitPanel()
    {
        slideScrollView.Init();
        gameObject.SetActive(false);
    }

    public override void ExitPanel()
    {
        InitPanel();
        ClearLevelList();
    }

    public GameObject CreateUIAndSetPosition(string name,Transform Parent)
    {
        GameObject UIgameObject = mUIFacade.CreateUIAndSetUIPosition(name);
        UIgameObject.transform.SetParent(Parent);
        return UIgameObject;
    }
    public void ToGameScene()
    {
        mUIFacade.PlayButton();
        ClearLevelList();
        ClearTowerList();
        currentLevelID = slideScrollView.currentNum;
        GameManager.Instance.currentStage = playerManager.unLpckdeNormalModelLevel[(currentBigLevelID-1)*5+currentLevelID-1];
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(GameManager.Instance.sceneStateManager.nomalModelSenceState);
    }
}


