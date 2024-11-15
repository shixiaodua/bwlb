using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HelpPanel : BasePanel
{
    private GameObject helpPage;
    private GameObject monsterPage;
    private GameObject towerPage;
    private SlideScrollView slideScrollView;
    private SlideCanCoverScrollView slideCanCoverScrollView;
    private Tween helpPanelTween;
    protected override void Awake()
    {
        base.Awake();
        helpPage = transform.Find("HelpPage").gameObject;
        monsterPage = transform.Find("MonsterPage").gameObject;
        towerPage = transform.Find("TowerPage").gameObject;
        slideScrollView = transform.Find("HelpPage/Scroll View").GetComponent<SlideScrollView>();
        slideCanCoverScrollView = transform.Find("TowerPage/Scroll View").GetComponent<SlideCanCoverScrollView>();
        
        helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
    }
    public override void InitPanel()
    {
        slideScrollView.Init();
        slideCanCoverScrollView.Init();
        transform.localPosition = new Vector3(960, 0, 0);
        transform.SetSiblingIndex(5);
    }

    //显示页面
    public void ShowHelpPage()
    {
        mUIFacade.PlayButton();
        helpPage.SetActive(true);
        monsterPage.SetActive(false);
        towerPage.SetActive(false);
    }
    public void ShowMonsterPage()
    {
        mUIFacade.PlayButton();
        helpPage.SetActive(false);
        monsterPage.SetActive(true);
        towerPage.SetActive(false);
    }
    public void ShowTowerPage()
    {
        mUIFacade.PlayButton();
        helpPage.SetActive(false);
        monsterPage.SetActive(false);
        towerPage.SetActive(true);
    }

    public override void EnterPanel()
    {
       
        ShowHelpPage();
        helpPanelTween.PlayForward();
    }
    public override void ExitPanel()
    {
        mUIFacade.PlayButton();
        helpPanelTween.PlayBackwards();
        //判断当前在哪个场景
        if (mUIFacade.currentSencePanelDist.ContainsKey(StringManager.MainPanel)) 
        {
           mUIFacade.currentSencePanelDist[StringManager.MainPanel].EnterPanel();
        }
        else
        {
            
        }
       
        InitPanel();
    }
}
