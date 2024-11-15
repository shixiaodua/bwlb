using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameNormalBigLevelPanel : BasePanel
{
    public Transform bigLevelContentTrans;
    public int bigLevelPageNum;
    private SlideScrollView sildeScrollView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;

    protected override void Awake()
    {
        base.Awake();
        playerManager = mUIFacade.mPlayerManager;
        bigLevelPageNum = playerManager.adventrueModelNum;
        bigLevelPage = new Transform[playerManager.adventrueModelNum];
        sildeScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        for(int i = 0; i < bigLevelPageNum; i++)//获取到所以大关卡按钮trans
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            int j = i;
            bigLevelPage[i].GetComponent<Button>().onClick.AddListener(()=>Onbuttn(j+1));
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < bigLevelPageNum; i++)
        {
            ShowBigLevelState(playerManager.unLpckdeNormalModelBigLevelList[i], playerManager.unLpckdeNormalModelLevelNum[i], playerManager.BigTotalLeveNum[i], bigLevelPage[i], i);
        }
    }

    public void ShowBigLevelState(bool unLocked,int unLockLevelNum,int totalNum,Transform theBigLevelButtonTrans,int bigLevelID)
    {
        if (unLocked)//解锁
        {
            theBigLevelButtonTrans.GetComponent<Button>().interactable = true;
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page/Tex_Page").GetComponent<Text>().text = unLockLevelNum.ToString() + "/" + totalNum.ToString();
        }
        else
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }

    public void Onbuttn(int bigLevelID)
    {
        mUIFacade.PlayButton();
                ExitPanel();
                GameNormalLevelPanel gameNormalLevelPanel = mUIFacade.currentSencePanelDist[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                gameNormalLevelPanel.ToThisPanel(bigLevelID);
                GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentSencePanelDist[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                gameNormalOptionPanel.isInBigLevelPanel = false;
    }
    public override void InitPanel()
    {
        sildeScrollView.Init();
    }

    public override void EnterPanel()
    {
        gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        gameObject.SetActive(false);
    }
    public void ToNextPage()
    {
        mUIFacade.PlayButton();
        sildeScrollView.ToNextPage();
    }

    public void ToLastPage()
    {
        mUIFacade.PlayButton();
        sildeScrollView.ToLastPage();
    }

}
