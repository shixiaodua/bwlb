using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NormalModelPanel : BasePanel
{   //引用
    private GameObject topPageGo;
    private GameObject gameOverPageGo;
    private GameObject gameWinGo;
    private GameObject menuPageGo;
    private GameObject img_FinalWave;
    private GameObject img_StartGo;
    private GameObject prizePageGo;


    public TopPage topPage;
    public GameController gameController;

    //属性
    public int totalRound;
    private bool lastPause;//点击菜单之前的暂停情况
#if Game
    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(1);//渲染层级
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
    private void PlayAudio()//播放音乐
    {
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/NormalMordel/CountDown"));
    }
    private void StartGame()
    {
        gameController.StartGame();

        CancelInvoke();//取消该MonoBehaviour全部的invoke方法
        //播放开始音效
        gameController.PlayEffectMusic("AudioClips/NormalMordel/GO");
        Invoke("StartPageFalse", 0.5f);
    }
    private void StartPageFalse()
    {
        img_StartGo.SetActive(false);
    }
    //与面板处理有关的方法
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
    //显示奖励
    public void ShowPrizePage()
    {
        lastPause = gameController.isPause;
        prizePageGo.SetActive(true);
        gameController.isPause = true;
        //更新存档
        GameManager.Instance.playerManager.SaveData();
    }
    //关闭奖励
    public void ClosePrizePage()
    {
        prizePageGo.SetActive(false);
        gameController.isPause = lastPause;
    }
    //显示菜单
    public void ShowMenuPage()
    {
        lastPause = gameController.isPause;
        menuPageGo.SetActive(true);
        gameController.isPause = true;
    }
    //关闭菜单
    public void CloseMenuPage()
    {
        menuPageGo.SetActive(false);
        gameController.isPause = lastPause;
    }
    //游戏胜利
    public void ShowWinPage()
    {
        UpdeatPlayerManagerData();
        Stage currentStage = gameController.currentStage;
        int nextStageNum = (currentStage.mBigLevelID - 1) * 5 + currentStage.mLevelID;
        
        //道具徽章更新
        if (gameController.IfAllClear())
        {
            currentStage.mAllClear = true;
        }
        //萝卜徽章更新
        int state = gameController.GetCarrotState();
        if(currentStage.mCarrotState>state||currentStage.mCarrotState==0)
        currentStage.mCarrotState = state;
        //解锁下一个关卡
        //不是最后一关才能解锁下一关
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
       
        //显示游戏胜利页面
        gameWinGo.SetActive(true);
        gameController.gameOver = false;
        //更新存档
        GameManager.Instance.playerManager.SaveData();
    }
    //游戏失败
    public void ShowGameOverPage()
    {
        UpdeatPlayerManagerData();
        gameOverPageGo.SetActive(true);
        gameController.gameOver = false;
        //更新存档
        GameManager.Instance.playerManager.SaveData();
    }
    //显示最后一波怪物
    public void ShowFinalWaveUI()
    {
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI", 1);
    }
    //更新回合的显示文本
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
    //关闭最后一波怪物显示
    public void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
    }

    //更新基础数据
    private void UpdeatPlayerManagerData()
    {
        GameManager.Instance.playerManager.coin += gameController.coin;
        GameManager.Instance.playerManager.killMonsterNum += gameController.killMonsterTotalNum;
        GameManager.Instance.playerManager.clearItemNum += gameController.clearItemNum;

    }
    //再来一把
    public void RePlay()
    {
        UpdeatPlayerManagerData();
        Invoke("ResetUI", 2);//切换场景显示遮罩时调用
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.nomalModelSenceState);
    }
    
    //重置ui显示
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
