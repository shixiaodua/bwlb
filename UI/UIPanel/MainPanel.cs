using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MainPanel : BasePanel
{
    private Animator carrotAnimator;
    private Transform monsterTrans;
   // private Transform cloudTrans;
    public Tween[] mainPanelTween=new Tween[2];//0左移，1右移
    public Tween exitTween;
    protected override void Awake()
    {
        base.Awake();
        //carrotAnimator = GameObject.Find("Emp_Carrot").GetComponent<Animator>();
        monsterTrans = GameObject.Find("Img_Monster").transform;
        //cloudTrans = GameObject.Find("Img_Cloud").transform;
        
        mainPanelTween[0] = gameObject.transform.DOLocalMoveX(-960, 0.5f);//创建一个动画
        mainPanelTween[0].SetAutoKill(false);//使动画播放完后不销毁
        mainPanelTween[0].Pause();//暂停动画播放
        mainPanelTween[1] = transform.DOLocalMoveX(960, 0.5f);//创建一个动画
        mainPanelTween[1].SetAutoKill(false);//使动画播放完后不销毁
        mainPanelTween[1].Pause();//暂停动画播放
        PlayUITween();
    }
    public override void InitPanel()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.SetSiblingIndex(8);
    }
    //播放动画
    private void PlayUITween()
    {
        monsterTrans.DOLocalMoveY(130, 1f).SetLoops(-1,LoopType.Yoyo);
        //cloudTrans.DOLocalMoveX(650, 8f).SetLoops(-1, LoopType.Restart);
    }
    //面板右移
    public void MoveToRight()
    {
        exitTween = mainPanelTween[1];
        ExitPanel();
        mUIFacade.currentSencePanelDist[StringManager.SetPanel].EnterPanel();
    }
    //面板左移
    public void MoveToLeft()
    {
        exitTween = mainPanelTween[0];
        ExitPanel();
        mUIFacade.currentSencePanelDist[StringManager.HelpPanel].EnterPanel();
    }
    public void ToNormalGameOptionScene()
    {
        mUIFacade.PlayButton();
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.normalGameOptionSenceState);
    }
    public void ToBossModelScene()
    {
        mUIFacade.PlayButton();
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.bossGameOptionSenceState);
    }

    public void ToMonsterNestScene()
    {
        mUIFacade.PlayButton();
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.monsterNestSenceState);
    }

    public override void EnterPanel()
    {
        //carrotAnimator.Play("CarrotGrowAnimation");//从头播放萝卜生长
        if (exitTween != null)
        {
            exitTween.PlayBackwards();//倒播动画
        }
        else
        {
            Debug.Log("在main面板没有进行过左右滑动");
        }
        //cloudTrans.gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        exitTween.PlayForward();
        //cloudTrans.gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        mUIFacade.PlayButton();
        Application.Quit();
    }
}
