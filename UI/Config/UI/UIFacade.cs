using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIFacade
{
    //管理者
    private GameManager mGameManager;
    private AudioSourceManager mAudioSourceManager;
    private UIManager mUIManager;
    public PlayerManager mPlayerManager;
    public SceneStateManager mSceneStateManager;
    //UIPanel脚本
    public Dictionary<string, IBasePanel> currentSencePanelDist = new Dictionary<string, IBasePanel>();

    private GameObject mask;//遮罩
    private Image maskImage;//遮罩图片
    public Transform canvasTransform;//canvas
    public IBaseSenceState lastSceneState;
    public IBaseSenceState currentSceneState;
    public UIFacade(UIManager uiManager)
    {
        mUIManager = uiManager;
        mGameManager = GameManager.Instance;
        mPlayerManager = GameManager.Instance.playerManager;
        mAudioSourceManager = GameManager.Instance.audioSourceManager;
        //mSceneStateManager = GameManager.Instance.sceneStateManager;
        canvasTransform = GameObject.Find("Canvas").transform;
        //初始化遮罩
        InitMask();
    }

    public void InitMask()//初始化遮罩
    {
        mask = CreateUIAndSetUIPosition("Img_Mask");
        maskImage = mask.GetComponent<Image>();
        
    }

    //初始化当前场景面板脚本的字典,将面板参数设置好
    public void InitDist()
    {
        foreach (var item in mUIManager.currentSencePanelDist)
        {
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            if (item.Value.GetComponent<IBasePanel>() == null)
            {
                //Debug.Log(item.Value);
                Debug.Log(item.Value.name + "面板没有挂载脚本或不存在当前面板");
            }
            item.Value.GetComponent<IBasePanel>().InitPanel();//每次进入场景都会对所有的面板进行初始化
            currentSencePanelDist.Add(item.Key, item.Value.GetComponent<IBasePanel>());
        }
    }

    //场景跳转
    public void ChangeSceneState(IBaseSenceState senceState)
    {
        lastSceneState = currentSceneState;
        currentSceneState = senceState;
        ShowMask();
        
    }

    //播放遮罩动画
    private void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 1), 2f);
        t.OnComplete(ExitSceneComplete);
    }

    //离开当前场景,进入下一个场景
    private void ExitSceneComplete()
    {
        
        lastSceneState.ExitSence();
        currentSceneState.EnterSence();
        HideMask();
    }

    //隐藏遮罩
    private void HideMask()
    {
        mask.transform.SetSiblingIndex(10);
        DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 2f);
        //mask.transform.SetSiblingIndex(0);
    }
    //清空当前场景面板脚本的字典，以及清空当前场景面板的字典，将面板放入对象池
    public void ClearDist()
    {
        currentSencePanelDist.Clear();
        mUIManager.ClearDist();
    }
    //添加面板到UIManager中的当前场景面板字典，获取到的是唯一副本
    public void AddUIPanelToDist(string name)
    {
   
        mUIManager.currentSencePanelDist.Add(name,GetGameObject(FactoryType.UIPanelFactory,name));
    }

    //实例化UI并设置好状态
    public GameObject CreateUIAndSetUIPosition(string name)
    {
        GameObject itemGo=GetGameObject(FactoryType.UIFactory,name);
        itemGo.transform.SetParent(canvasTransform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }

    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }

    public void PushGameObjectToFactory(FactoryType factoryType, string name, GameObject gameObject)
    {
         mGameManager.PushGameObjectToFactory(factoryType, name, gameObject);
    }

    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }

    public GameObject GetGameObject(FactoryType factoryType, string name)
    {
        return mGameManager.GetGameObject(factoryType, name);
    }
    //音乐控制
    //背景音乐
    public void CloseOrOpenBGMusic()
    {
        mAudioSourceManager.CloseOrOpenBGMusic();
    }
    //游戏音乐
    public void CloseOrOpenEffectMusic()
    {
        mAudioSourceManager.CloseOrOpenEffectMusic();
    }
    //按钮音效播放
    public void PlayButton()
    {
        mAudioSourceManager.PlayButton();
    }
    //翻书音效播放
    public void PlayPagingAudioClip()
    {
        mAudioSourceManager.PlayPagingAudioClip();
    }
    public void PlayBGMusic()
    {
        mAudioSourceManager.PlayBGMusic(GameManager.Instance.GetAudioClip("AudioClips/Main/BGMusic"));
    }
    public void CloseBGMusic()
    {
        mAudioSourceManager.CloseBGMusic();
    }
    public void PlayEffectMusic(AudioClip audioClip)//播放给定的音乐一次，例如怪物死亡音效
    {
        mAudioSourceManager.PlayEffectMusic(audioClip);
    }
}
