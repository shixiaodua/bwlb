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
    private int index;//当前怪物列表中的第几个怪物
    //初始化
    public void Init(MonsterNestPanel monsterPanle,int monsterListIndex)
    {
        index=monsterListIndex;
        this.monsterPanle = monsterPanle;
        player= GameManager.Instance.uiManager.mUIFacade.mPlayerManager;
        //更新
        UpdateLevel();
    }
    //点击孵化
    public void ClickHatch(){
        //判断怪物窝是否足够
        if(player.nest==0){
            ShowTips("我需要一个窝窝...");
            return ;
        }
        ShowTips("我出来啦！");
        player.nest--;
        UpLevel();
        mdata.remainMilk=MonsterConfig.mconfig.Milk[mdata.monsterLevel-1];
        mdata.remainCookies=MonsterConfig.mconfig.Cookie[mdata.monsterLevel-1];
        UpdatePlayerData();
    }

    //点击饼干按钮调用方法
    public void ClickCookieBtn()
    {
        Debug.Log("点击饼干");
        //判断是否已经长大
        if(mdata.monsterLevel==2){
            ShowTips("我已经长大啦");
            return ;
        }
        //判断是否已经吃饱
        if(mdata.remainCookies==0){
            ShowTips("我吃饱啦，现在有点口渴>_<");
            return;
        }
        //判断是否没有牛奶
        if(player.cookies==0){
            ShowTips("阿巴阿巴...");
        }
        //判断剩余饼干是否足够
        if (player.cookies >= MonsterConfig.addCookie)
        {
            //判断是否吃饱
            if (MonsterConfig.addCookie >= mdata.remainCookies)
            {
                player.cookies -= mdata.remainCookies;
                mdata.remainCookies = 0;
                ShowTips("呼，吃饱了");
            }
            else
            {
                player.cookies -= MonsterConfig.addCookie;
                mdata.remainCookies -= MonsterConfig.addCookie;
                ShowTips("干饭干饭干饭！");
            }
        }
        else
        {
            //判断是否吃饱
            if (player.cookies >= mdata.remainCookies)
            {
                player.cookies -= mdata.remainCookies;
                mdata.remainCookies = 0;
                ShowTips("呼，吃饱了");
            }
            else
            {
                player.cookies =0;
                mdata.remainCookies -= player.cookies;
                ShowTips("干饭干饭干饭！");
            }
        }
        IsUpLevel();
        UpdatePlayerData();
        monsterPanle.UpdatePanleData();
    }
    //点击牛奶按钮调用方法
    public void ClickMilkBtn()
    {  
        Debug.Log("点击牛奶");
        //判断是否已经长大
        if(mdata.monsterLevel==2){
            ShowTips("我已经长大啦");
            return ;
        }
        //判断是否已经喝满
        if(mdata.remainMilk==0){
            ShowTips("我喝饱啦，可以给我吃饼干吗QAQ");
            return;
        }
        //判断是否没有牛奶
        if(player.milk==0){
            ShowTips("阿巴阿巴...");
        }
        //判断剩余是否足够
        if (player.milk >= MonsterConfig.addMilk)
        {
            //判断是否吃饱
            if (MonsterConfig.addMilk >= mdata.remainMilk)
            {
                player.milk -= mdata.remainMilk;
                mdata.remainMilk = 0;
                ShowTips("牛奶满足了...");
            }
            else
            {
                player.milk -= MonsterConfig.addMilk;
                mdata.remainMilk -= MonsterConfig.addMilk;
                ShowTips("好喝，我爱喝牛奶^_^");
            }
        }
        else
        {
            //判断是否吃饱
            if (player.milk >= mdata.remainMilk)
            {
                player.milk -= mdata.remainMilk;
                mdata.remainMilk = 0;
                ShowTips("牛奶满足了...");
            }
            else
            {
                player.milk = 0;
                mdata.remainMilk -= player.milk;
                ShowTips("好喝，我爱喝牛奶^_^");
            }
        }
        IsUpLevel();
        UpdatePlayerData();
        monsterPanle.UpdatePanleData();
    }
    //判断能否升级
    void IsUpLevel()
    {
        Debug.Log("判断是否升级");
        if (mdata.remainCookies == 0 && mdata.remainMilk == 0&&mdata.monsterLevel<2)
        {
            UpLevel();
        }
    }
    //升级
    void UpLevel()
    {
        Debug.Log("升级");
        mdata.monsterLevel++;
        mdata.remainMilk=MonsterConfig.mconfig.Milk[mdata.monsterLevel-1];
        mdata.remainCookies=MonsterConfig.mconfig.Cookie[mdata.monsterLevel-1];
        UpdateLevel();
    }
    //设置不同等级对应的显示
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
    //点击怪物随机怪物消息
    public void ClickMonster()
    {
        Debug.Log("点击monster");
        if(mdata.monsterLevel==0){
            ShowTips("什么时候才能数星星呢");
        }
        else{
          ShowTips();
        }
        //显示聊天按钮
        transform.Find("AIBtn").gameObject.SetActive(true);
    }
    //点击聊天按钮
    public void ClickAIBtn(){
        //打开聊天面板,退出当前面板
        monsterPanle.EnterAIPanel(mdata.monsterLevel,mdata.monsterID);
    }
    void ShowTips(string str=null){
        //获取到当前的显示状态
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
    public int monsterLevel;//怪物等级0--蛋，1--baby，2--成年
    public int remainCookies;
    public int remainMilk;
    public int monsterID;
}
