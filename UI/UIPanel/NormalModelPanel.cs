using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NormalModelPanel : BasePanel
{   //����
    private GameObject topPageGo;
    private GameObject gameOverPageGo;
    private GameObject gameWinGo;
    private GameObject menuPageGo;
    private GameObject img_FinalWave;
    private GameObject img_StartGo;
    private GameObject prizePageGo;


    public TopPage topPage;
    public GameController gameController;

    //����
    public int totalRound;
    private bool lastPause;//����˵�֮ǰ����ͣ���
#if Game
    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(1);//��Ⱦ�㼶
        topPageGo = transform.Find("Img_TopPage").gameObject;
        gameOverPageGo= transform.Find("GameOverPage").gameObject;
        gameWinGo= transform.Find("GameWinPage").gameObject;
        menuPageGo= transform.Find("MenuPage").gameObject;
        img_FinalWave= transform.Find("Img_FinalWave").gameObject;
        img_StartGo= transform.Find("StartUI").gameObject;
        prizePageGo= transform.Find("PrizePage").gameObject;
        topPage = topPageGo.GetComponent<TopPage>();
    }
    private void OnEnable()
    {
        img_StartGo.SetActive(true);
        InvokeRepeating("PlayAudio", 0, 1);
        Invoke("StartGame", 3);
    }
    private void PlayAudio()//��������
    {
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/NormalMordel/CountDown"));
    }
    private void StartGame()
    {
        gameController.StartGame();

        CancelInvoke();//ȡ����MonoBehaviourȫ����invoke����
        //���ſ�ʼ��Ч
        gameController.PlayEffectMusic("AudioClips/NormalMordel/GO");
        Invoke("StartPageFalse", 0.5f);
    }
    private void StartPageFalse()
    {
        img_StartGo.SetActive(false);
    }
    //����崦���йصķ���
    public override void EnterPanel()
    {
        base.EnterPanel();
        gameController = GameController.Instance;
        totalRound = gameController.currentStage.mTotalRound;
        topPageGo.SetActive(true);
    }
    public override void UpdatePanel()
    {
        base.UpdatePanel();
        topPage.gameObject.SetActive(false);
    }
    //��ʾ����
    public void ShowPrizePage()
    {
        lastPause = gameController.isPause;
        prizePageGo.SetActive(true);
        gameController.isPause = true;
        //���´浵
        GameManager.Instance.playerManager.SaveData();
    }
    //�رս���
    public void ClosePrizePage()
    {
        prizePageGo.SetActive(false);
        gameController.isPause = lastPause;
    }
    //��ʾ�˵�
    public void ShowMenuPage()
    {
        lastPause = gameController.isPause;
        menuPageGo.SetActive(true);
        gameController.isPause = true;
    }
    //�رղ˵�
    public void CloseMenuPage()
    {
        menuPageGo.SetActive(false);
        gameController.isPause = lastPause;
    }
    //��Ϸʤ��
    public void ShowWinPage()
    {
        UpdeatPlayerManagerData();
        Stage currentStage = gameController.currentStage;
        int nextStageNum = (currentStage.mBigLevelID - 1) * 5 + currentStage.mLevelID;
        
        //���߻��¸���
        if (gameController.IfAllClear())
        {
            currentStage.mAllClear = true;
        }
        //�ܲ����¸���
        int state = gameController.GetCarrotState();
        if(currentStage.mCarrotState>state||currentStage.mCarrotState==0)
        currentStage.mCarrotState = state;
        //������һ���ؿ�
        //�������һ�ز��ܽ�����һ��
        if (nextStageNum!=15)
        {
            Stage nextStage = GameManager.Instance.playerManager.unLpckdeNormalModelLevel[nextStageNum];
            
            if (currentStage.mLevelID != 5&& nextStage.munLocked ==false )
            {
                GameManager.Instance.playerManager.unLpckdeNormalModelLevelNum[currentStage.mBigLevelID - 1]++;
            }
            nextStage.munLocked = true;
            if(currentStage.mLevelID==5)
            GameManager.Instance.playerManager.unLpckdeNormalModelBigLevelList[currentStage.mBigLevelID] = true;
        }
       
        //��ʾ��Ϸʤ��ҳ��
        gameWinGo.SetActive(true);
        gameController.gameOver = false;
        //���´浵
        GameManager.Instance.playerManager.SaveData();
    }
    //��Ϸʧ��
    public void ShowGameOverPage()
    {
        UpdeatPlayerManagerData();
        gameOverPageGo.SetActive(true);
        gameController.gameOver = false;
        //���´浵
        GameManager.Instance.playerManager.SaveData();
    }
    //��ʾ���һ������
    public void ShowFinalWaveUI()
    {
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI", 1);
    }
    //���»غϵ���ʾ�ı�
    public void UpdateRoundText(Text text)
    {
        int roundNum = gameController.level.currentRound;
        if (roundNum >= 10)
        {
            text.text = (roundNum / 10).ToString() + "  " + (roundNum % 10).ToString();
        }
        else
        {
            text.text = "0  " + roundNum.ToString();
        }
    }
    //�ر����һ��������ʾ
    public void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
    }

    //���»�������
    private void UpdeatPlayerManagerData()
    {
        GameManager.Instance.playerManager.coin += gameController.coin;
        GameManager.Instance.playerManager.killMonsterNum += gameController.killMonsterTotalNum;
        GameManager.Instance.playerManager.clearItemNum += gameController.clearItemNum;

    }
    //����һ��
    public void RePlay()
    {
        UpdeatPlayerManagerData();
        Invoke("ResetUI", 2);//�л�������ʾ����ʱ����
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.nomalModelSenceState);
    }
    
    //����ui��ʾ
    public void ResetUI()
    {
        gameOverPageGo.SetActive(false);
        gameWinGo.SetActive(false);
        menuPageGo.SetActive(false);
        img_FinalWave.SetActive(false);
        topPageGo.SetActive(false);
    }
    public void ChooseOtherLevel()
    {
        UpdeatPlayerManagerData();
        Invoke("ToOtherScene", 2);
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.normalGameOptionSenceState);
    }
    public void ToOtherScene()
    {
        gameController.gameOver = false;
        ResetUI();
        SceneManager.LoadScene(2);
    }
#endif
}
