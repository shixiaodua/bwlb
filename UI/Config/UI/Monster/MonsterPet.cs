using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPet : MonoBehaviour
{
    public GameObject egg;
    public GameObject baby;
    public GameObject normal;
    public GameObject leftTips;
    public GameObject rightTips;

    public MonsterPetDate mdata;
    private MonsterNestPanel monsterPanle;
    private PlayerManager player;
    private int index;//��ǰ�����б��еĵڼ�������
    //��ʼ��
    public void Init(MonsterNestPanel monsterPanle,int monsterListIndex)
    {
        index=monsterListIndex;
        this.monsterPanle = monsterPanle;
        player= GameManager.Instance.uiManager.mUIFacade.mPlayerManager;
        //����
        UpdateLevel();
    }
    //�������
    public void ClickHatch(){
        //�жϹ������Ƿ��㹻
        if(player.nest==0){
            ShowTips("����Ҫһ������...");
            return ;
        }
        ShowTips("�ҳ�������");
        player.nest--;
        UpLevel();
        mdata.remainMilk=MonsterConfig.mconfig.Milk[mdata.monsterLevel-1];
        mdata.remainCookies=MonsterConfig.mconfig.Cookie[mdata.monsterLevel-1];
        UpdatePlayerData();
    }

    //������ɰ�ť���÷���
    public void ClickCookieBtn()
    {
        Debug.Log("�������");
        //�ж��Ƿ��Ѿ�����
        if(mdata.monsterLevel==2){
            ShowTips("���Ѿ�������");
            return ;
        }
        //�ж��Ƿ��Ѿ��Ա�
        if(mdata.remainCookies==0){
            ShowTips("�ҳԱ����������е�ڿ�>_<");
            return;
        }
        //�ж��Ƿ�û��ţ��
        if(player.cookies==0){
            ShowTips("���Ͱ���...");
        }
        //�ж�ʣ������Ƿ��㹻
        if (player.cookies >= MonsterConfig.addCookie)
        {
            //�ж��Ƿ�Ա�
            if (MonsterConfig.addCookie >= mdata.remainCookies)
            {
                player.cookies -= mdata.remainCookies;
                mdata.remainCookies = 0;
                ShowTips("�����Ա���");
            }
            else
            {
                player.cookies -= MonsterConfig.addCookie;
                mdata.remainCookies -= MonsterConfig.addCookie;
                ShowTips("�ɷ��ɷ��ɷ���");
            }
        }
        else
        {
            //�ж��Ƿ�Ա�
            if (player.cookies >= mdata.remainCookies)
            {
                player.cookies -= mdata.remainCookies;
                mdata.remainCookies = 0;
                ShowTips("�����Ա���");
            }
            else
            {
                player.cookies =0;
                mdata.remainCookies -= player.cookies;
                ShowTips("�ɷ��ɷ��ɷ���");
            }
        }
        IsUpLevel();
        UpdatePlayerData();
        monsterPanle.UpdatePanleData();
    }
    //���ţ�̰�ť���÷���
    public void ClickMilkBtn()
    {  
        Debug.Log("���ţ��");
        //�ж��Ƿ��Ѿ�����
        if(mdata.monsterLevel==2){
            ShowTips("���Ѿ�������");
            return ;
        }
        //�ж��Ƿ��Ѿ�����
        if(mdata.remainMilk==0){
            ShowTips("�Һȱ��������Ը��ҳԱ�����QAQ");
            return;
        }
        //�ж��Ƿ�û��ţ��
        if(player.milk==0){
            ShowTips("���Ͱ���...");
        }
        //�ж�ʣ���Ƿ��㹻
        if (player.milk >= MonsterConfig.addMilk)
        {
            //�ж��Ƿ�Ա�
            if (MonsterConfig.addMilk >= mdata.remainMilk)
            {
                player.milk -= mdata.remainMilk;
                mdata.remainMilk = 0;
                ShowTips("ţ��������...");
            }
            else
            {
                player.milk -= MonsterConfig.addMilk;
                mdata.remainMilk -= MonsterConfig.addMilk;
                ShowTips("�úȣ��Ұ���ţ��^_^");
            }
        }
        else
        {
            //�ж��Ƿ�Ա�
            if (player.milk >= mdata.remainMilk)
            {
                player.milk -= mdata.remainMilk;
                mdata.remainMilk = 0;
                ShowTips("ţ��������...");
            }
            else
            {
                player.milk = 0;
                mdata.remainMilk -= player.milk;
                ShowTips("�úȣ��Ұ���ţ��^_^");
            }
        }
        IsUpLevel();
        UpdatePlayerData();
        monsterPanle.UpdatePanleData();
    }
    //�ж��ܷ�����
    void IsUpLevel()
    {
        Debug.Log("�ж��Ƿ�����");
        if (mdata.remainCookies == 0 && mdata.remainMilk == 0&&mdata.monsterLevel<2)
        {
            UpLevel();
        }
    }
    //����
    void UpLevel()
    {
        Debug.Log("����");
        mdata.monsterLevel++;
        mdata.remainMilk=MonsterConfig.mconfig.Milk[mdata.monsterLevel-1];
        mdata.remainCookies=MonsterConfig.mconfig.Cookie[mdata.monsterLevel-1];
        UpdateLevel();
    }
    //���ò�ͬ�ȼ���Ӧ����ʾ
    void UpdateLevel()
    {
        Debug.Log(mdata);
        if (mdata.monsterLevel == 0)
        {
            Debug.Log(monsterPanle.monsterSprite[1][mdata.monsterID]);
            egg.SetActive(true);
            baby.SetActive(false);
            normal.SetActive(false);
            transform.Find("Emp_Egg").Find("Img_Egg").GetComponent<Image>().sprite = monsterPanle.monsterSprite[0][mdata.monsterID];
        }
        if (mdata.monsterLevel == 1)
        {
            Debug.Log(monsterPanle.monsterSprite[1][mdata.monsterID]);
            egg.SetActive(false);
            baby.SetActive(true);
            normal.SetActive(false);
            transform.Find("Emp_Baby").transform.Find("Img_Baby").GetComponent<Image>().sprite = monsterPanle.monsterSprite[1][mdata.monsterID];
        }
        if (mdata.monsterLevel == 2)
        {
            egg.SetActive(false);
            baby.SetActive(false);
            normal.SetActive(true);
            transform.Find("Emp_Normal").transform.Find("Img_Normal").GetComponent<Image>().sprite = monsterPanle.monsterSprite[2][mdata.monsterID];
        }
     }
    //����������������Ϣ
    public void ClickMonster()
    {
        Debug.Log("���monster");
        if(mdata.monsterLevel==0){
            ShowTips("ʲôʱ�������������");
        }
        else{
          ShowTips();
        }
        //��ʾ���찴ť
        transform.Find("AIBtn").gameObject.SetActive(true);
    }
    //������찴ť
    public void ClickAIBtn(){
        //���������,�˳���ǰ���
        monsterPanle.EnterAIPanel(mdata.monsterLevel,mdata.monsterID);
    }
    void ShowTips(string str=null){
        //��ȡ����ǰ����ʾ״̬
        bool left=leftTips.activeSelf;
        bool right=rightTips.activeSelf;
        if(left||right)return;
        Debug.Log(left);
        Debug.Log(right);
        if(str==null){
            str = MonsterConfig.tipsString[Random.Range(0,MonsterConfig.tipsString.Length)];
        }
        if(Random.Range(0,2)==0){
            leftTips.SetActive(true);
            leftTips.transform.Find("Text (Legacy)").GetComponent<Text>().text=str;
        }
        else{
            rightTips.SetActive(true);
            rightTips.transform.Find("Text (Legacy)").GetComponent<Text>().text=str;
        }
        Invoke("HideTips",2);
    }
    void HideTips(){
        leftTips.SetActive(false);
        rightTips.SetActive(false);
    }
    void UpdatePlayerData()
    {
        player.monsterPetDatesList[index] = mdata;
    }
}

public struct MonsterPetDate
{
    public int monsterLevel;//����ȼ�0--����1--baby��2--����
    public int remainCookies;
    public int remainMilk;
    public int monsterID;
}
