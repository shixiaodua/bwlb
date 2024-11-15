using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MainPanel : BasePanel
{
    private Animator carrotAnimator;
    private Transform monsterTrans;
   // private Transform cloudTrans;
    public Tween[] mainPanelTween=new Tween[2];//0���ƣ�1����
    public Tween exitTween;
    protected override void Awake()
    {
        base.Awake();
        //carrotAnimator = GameObject.Find("Emp_Carrot").GetComponent<Animator>();
        monsterTrans = GameObject.Find("Img_Monster").transform;
        //cloudTrans = GameObject.Find("Img_Cloud").transform;
        
        mainPanelTween[0] = gameObject.transform.DOLocalMoveX(-960, 0.5f);//����һ������
        mainPanelTween[0].SetAutoKill(false);//ʹ���������������
        mainPanelTween[0].Pause();//��ͣ��������
        mainPanelTween[1] = transform.DOLocalMoveX(960, 0.5f);//����һ������
        mainPanelTween[1].SetAutoKill(false);//ʹ���������������
        mainPanelTween[1].Pause();//��ͣ��������
        PlayUITween();
    }
    public override void InitPanel()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.SetSiblingIndex(8);
    }
    //���Ŷ���
    private void PlayUITween()
    {
        monsterTrans.DOLocalMoveY(130, 1f).SetLoops(-1,LoopType.Yoyo);
        //cloudTrans.DOLocalMoveX(650, 8f).SetLoops(-1, LoopType.Restart);
    }
    //�������
    public void MoveToRight()
    {
        exitTween = mainPanelTween[1];
        ExitPanel();
        mUIFacade.currentSencePanelDist[StringManager.SetPanel].EnterPanel();
    }
    //�������
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
        //carrotAnimator.Play("CarrotGrowAnimation");//��ͷ�����ܲ�����
        if (exitTween != null)
        {
            exitTween.PlayBackwards();//��������
        }
        else
        {
            Debug.Log("��main���û�н��й����һ���");
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
