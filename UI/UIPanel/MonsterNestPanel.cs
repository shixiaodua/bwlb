using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;
public class MonsterNestPanel : BasePanel
{
    private GameObject monsterShop;//�����̵����
    private PlayerManager playerManager;
    private GameObject empMG;
    public List<GameObject> cwList;
    public GameObject monsterPre;
    public Sprite[][] monsterSprite;
    protected override void Awake()
    {
        Debug.Log("����������");
        base.Awake();
        //��ȡmonsterͼƬ
        monsterSprite = new Sprite[3][];
        for(int i = 0; i < 3; i++)
        {
            monsterSprite[i] = new Sprite[3];
        }
        for (int i = 1; i <= 3; i++) {
            monsterSprite[0][i-1] = GameManager.Instance.GetSprite("Pictures/MonsterNest/Monster/Egg/" + i.ToString());
            monsterSprite[1][i-1]= GameManager.Instance.GetSprite("Pictures/MonsterNest/Monster/Baby/" + i.ToString());
            monsterSprite[2][i-1] = GameManager.Instance.GetSprite("Pictures/MonsterNest/Monster/Normal/" + i.ToString());
        }
    }
    public override void EnterPanel(){
        transform.gameObject.SetActive(true);
    }
    public override void InitPanel()
    {
        base.InitPanel();
         //��ȡ����ɫ��Ϣ
        playerManager = mUIFacade.mPlayerManager;
        Debug.Log(playerManager.monsterPetDatesList.Count);
        //������Ӧ�����ĳ���
        cwList = new List<GameObject>();
        empMG = transform.Find("Emp_MonsterGroup").gameObject;
        monsterPre = Resources.Load<GameObject>("Prefabs/UI/Emp_Monsters");
        for (int i = 0; i < playerManager.monsterPetDatesList.Count; i++)
        {
            GameObject kk = Instantiate(monsterPre);
            //���ø���
            Transform tra=this.transform.Find("Emp_MonsterGroup/Viewport/Content");
            kk.transform.SetParent(tra);
            Debug.Log(tra);
            tra.GetComponent<RectTransform>().offsetMax=new Vector2 ((tra.childCount-1)*580,0);
            kk.GetComponent<MonsterPet>().mdata = playerManager.monsterPetDatesList[i];
            Debug.Log("����id��"+playerManager.monsterPetDatesList[i].monsterID);
            kk.GetComponent<MonsterPet>().Init(this,i);
            cwList.Add(kk);
            //��ʼ��
        }
        UpdatePanleData();
    }
    public void UpdatePanleData(){
        //���ݸ���
        transform.Find("Img_TopPage/Txt_Milk").GetComponent<Text>().text=playerManager.milk.ToString();
        transform.Find("Img_TopPage/Txt_Cookie").GetComponent<Text>().text=playerManager.cookies.ToString();
        transform.Find("Img_TopPage/Txt_Nest").GetComponent<Text>().text=playerManager.nest.ToString();
    }
    public void ReturnMain()
    {
        mUIFacade.PlayButton();
        Debug.Log(playerManager.monsterPetDatesList[0].monsterID);
        playerManager.SaveData();
        Transform k=this.transform.Find("Emp_MonsterGroup/Viewport/Content");
        for(int i=0;i<k.childCount;i++){
            Destroy(k.GetChild(i).gameObject);
        }
        Transform tra=this.transform.Find("Emp_MonsterGroup/Viewport/Content");
        tra.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,0);
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(GameManager.Instance.sceneStateManager.mainSenceState);
    }
    public void MonsterShop()
    {
        mUIFacade.PlayButton();
    }
    public void EnterAIPanel(int level,int id){
        ExitPanel();
        UIPanel uipanel=mUIFacade.currentSencePanelDist[StringManager.UIPanle] as UIPanel;
        uipanel.ToTishPanel(level,id);
    }
    public override void ExitPanel(){
        transform.gameObject.SetActive(false);
    }
}
