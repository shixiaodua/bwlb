using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class SetPanel: BasePanel
{
    private GameObject optionPageGo;
    private GameObject statisticsPageGo;
    private GameObject producerPageGo;
    private GameObject panel_ResetGo;
    private Tween setPanelTween;
    private bool playBGMusic;
    private bool playEffectMusic;
    public Sprite[] btnSprites;//0.��Ч����1.��Ч�أ�2.������Ч����3.������Ч��
    private Image Img_Btn_EffectAudio;
    private Image Img_Btn_BGAudio;
    public Text[] statisticesTexts;

    protected override void Awake()
    {
        //Debug.Log(gameObject.name);���ڹ��ؽű���canvas���±���
        base.Awake();
        optionPageGo = transform.Find("OptionPage").gameObject;
        statisticsPageGo = transform.Find("StatisticsPage").gameObject;
        producerPageGo = transform.Find("ProducerPage").gameObject;
        panel_ResetGo = transform.Find("Panel_Reset").gameObject;
        setPanelTween = transform.DOLocalMoveX(0, 0.5f);
        setPanelTween.SetAutoKill(false);
        setPanelTween.Pause();

        playBGMusic = true;
        playEffectMusic = true;
        Img_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Image>();
        Img_Btn_EffectAudio = optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Image>();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-960, 0, 0);
        transform.SetSiblingIndex(2);
    }

    //��ʾҳ��
    public void ShowOptionPage()
    {
        mUIFacade.PlayButton();
        optionPageGo.SetActive(true);
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(false);
    }
    public void ShowStatisticsPage()
    {
        mUIFacade.PlayButton();
        ShowStatistics();//����Ϊʵʱ������
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(true);
        producerPageGo.SetActive(false);
    }

    public void ShowProducerPage()
    {
        mUIFacade.PlayButton();
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(true);
    }

    //�����˳����
    public override void EnterPanel()
    {
        ShowOptionPage();
        setPanelTween.PlayForward();
    }
    public override void ExitPanel()
    {
        mUIFacade.PlayButton();
        setPanelTween.PlayBackwards();
        mUIFacade.currentSencePanelDist[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }

    //�������ְ�ť�¼�
    public void CloseOrOpenBGMusic()
    {
        mUIFacade.PlayButton();
        playBGMusic = !playBGMusic;
        mUIFacade.CloseOrOpenBGMusic();
        if (playBGMusic == false)
        {
            Img_Btn_BGAudio.sprite = btnSprites[1];
        }
        else
        {
            Img_Btn_BGAudio.sprite = btnSprites[0];
        }
    }
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
        mUIFacade.CloseOrOpenEffectMusic();
        if (playEffectMusic == false)
        {
            Img_Btn_EffectAudio.sprite = btnSprites[3];
        }
        else
        {
            Img_Btn_EffectAudio.sprite = btnSprites[2];
        }
        mUIFacade.PlayButton();
    }

    //��ʾ����
    public void ShowStatistics()
    {
        PlayerManager playerManager = mUIFacade.mPlayerManager;
        statisticesTexts[0].text = playerManager.adventrueModelNum.ToString();
        statisticesTexts[1].text = playerManager.burriedLevelNum.ToString();
        statisticesTexts[2].text = playerManager.bossModelNum.ToString();
        statisticesTexts[3].text = playerManager.coin.ToString();
        statisticesTexts[4].text = playerManager.killMonsterNum.ToString();
        statisticesTexts[5].text = playerManager.killBossNum.ToString();
        statisticesTexts[6].text = playerManager.clearItemNum.ToString();
    }

    //������Ϸ
    public void ResetGame()
    {
        mUIFacade.PlayButton();
        ShowResetPanel();
    }
    public void CancelReset()
    {
        CloseResetPanel();
    }

    public void ShowResetPanel()
    {
        panel_ResetGo.SetActive(true);
    }

    public void Right()
    {
        GameManager.Instance.isInitPlayerManager = true;
        GameManager.Instance.playerManager.ReadData();
        GameManager.Instance.playerManager.SaveData();
        GameManager.Instance.isInitPlayerManager = false;
        CloseResetPanel();
    }
    public void CloseResetPanel()
    {
        mUIFacade.PlayButton();
        panel_ResetGo.SetActive(false);
    }
}
