using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIFacade
{
    //������
    private GameManager mGameManager;
    private AudioSourceManager mAudioSourceManager;
    private UIManager mUIManager;
    public PlayerManager mPlayerManager;
    public SceneStateManager mSceneStateManager;
    //UIPanel�ű�
    public Dictionary<string, IBasePanel> currentSencePanelDist = new Dictionary<string, IBasePanel>();

    private GameObject mask;//����
    private Image maskImage;//����ͼƬ
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
        //��ʼ������
        InitMask();
    }

    public void InitMask()//��ʼ������
    {
        mask = CreateUIAndSetUIPosition("Img_Mask");
        maskImage = mask.GetComponent<Image>();
        
    }

    //��ʼ����ǰ�������ű����ֵ�,�����������ú�
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
                Debug.Log(item.Value.name + "���û�й��ؽű��򲻴��ڵ�ǰ���");
            }
            item.Value.GetComponent<IBasePanel>().InitPanel();//ÿ�ν��볡����������е������г�ʼ��
            currentSencePanelDist.Add(item.Key, item.Value.GetComponent<IBasePanel>());
        }
    }

    //������ת
    public void ChangeSceneState(IBaseSenceState senceState)
    {
        lastSceneState = currentSceneState;
        currentSceneState = senceState;
        ShowMask();
        
    }

    //�������ֶ���
    private void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 1), 2f);
        t.OnComplete(ExitSceneComplete);
    }

    //�뿪��ǰ����,������һ������
    private void ExitSceneComplete()
    {
        
        lastSceneState.ExitSence();
        currentSceneState.EnterSence();
        HideMask();
    }

    //��������
    private void HideMask()
    {
        mask.transform.SetSiblingIndex(10);
        DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 2f);
        //mask.transform.SetSiblingIndex(0);
    }
    //��յ�ǰ�������ű����ֵ䣬�Լ���յ�ǰ���������ֵ䣬������������
    public void ClearDist()
    {
        currentSencePanelDist.Clear();
        mUIManager.ClearDist();
    }
    //�����嵽UIManager�еĵ�ǰ��������ֵ䣬��ȡ������Ψһ����
    public void AddUIPanelToDist(string name)
    {
   
        mUIManager.currentSencePanelDist.Add(name,GetGameObject(FactoryType.UIPanelFactory,name));
    }

    //ʵ����UI�����ú�״̬
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
    //���ֿ���
    //��������
    public void CloseOrOpenBGMusic()
    {
        mAudioSourceManager.CloseOrOpenBGMusic();
    }
    //��Ϸ����
    public void CloseOrOpenEffectMusic()
    {
        mAudioSourceManager.CloseOrOpenEffectMusic();
    }
    //��ť��Ч����
    public void PlayButton()
    {
        mAudioSourceManager.PlayButton();
    }
    //������Ч����
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
    public void PlayEffectMusic(AudioClip audioClip)//���Ÿ���������һ�Σ��������������Ч
    {
        mAudioSourceManager.PlayEffectMusic(audioClip);
    }
}
