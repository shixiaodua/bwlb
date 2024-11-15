using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TopPage : MonoBehaviour
{
    //����
    private Text tex_Coin;
    private Text tex_RoundNum;
    private Text tex_TotalRoundNum;
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;
    private GameObject emp_PlayingTextGo;
    private GameObject emp_PauseGo; 
    private NormalModelPanel normalModelPanel;
    private GameController gameController;
    //��Դ
    private Sprite[] GameSpeedSprites;
    private Sprite[] PauseSprites;
    //����ֵ
    private bool isNormalSpeed;
    private bool isPause;
#if Game

    private void Awake()
    {  
        gameController = GameController.Instance;
        tex_Coin = transform.Find("Tex_Coin").GetComponent<Text>();
        tex_RoundNum = transform.Find("Emp_PlayingText/Tex_RoundText").GetComponent<Text>();
        tex_TotalRoundNum = transform.Find("Emp_PlayingText/Tex_TotalCount").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause = transform.Find("Btn_Pause").GetComponent<Image>();
        emp_PauseGo = transform.Find("Emp_Pause").gameObject;
        emp_PlayingTextGo = transform.Find("Emp_PlayingText").gameObject;

        //��ȡ���������ϵĽű�
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        GameSpeedSprites = new Sprite[2];
        GameSpeedSprites[0] = gameController.GetSprite("Pictures/NormalMordel/touming-hd.pvr_7");
        GameSpeedSprites[1] = gameController.GetSprite("Pictures/NormalMordel/touming-hd.pvr_9");
        PauseSprites = new Sprite[2];
        PauseSprites[0] = gameController.GetSprite("Pictures/NormalMordel/touming-hd.pvr_11");
        PauseSprites[1] = gameController.GetSprite("Pictures/NormalMordel/touming-hd.pvr_12");
    }
    private void OnEnable()
    {
        gameController = GameController.Instance;
        isNormalSpeed = true;
        isPause = false;
        //����ui��ʾ
        tex_Coin.text = gameController.coin.ToString();
        tex_TotalRoundNum.text= gameController.currentStage.mTotalRound.ToString();
        tex_RoundNum.text = "0  1";
        img_Btn_GameSpeed.sprite = GameSpeedSprites[0];
        img_Btn_Pause.sprite = PauseSprites[1];
        emp_PlayingTextGo.SetActive(true);
        emp_PauseGo.SetActive(false);
    }
    public void UpdateCoinText()
    {
        tex_Coin.text = gameController.coin.ToString();
    }
    public void UpdateRoundNum()
    {
        normalModelPanel.UpdateRoundText(tex_RoundNum);
    }
    //�ı���Ϸ�ٶ�
    public void ChangeSpeed()
    {
        isNormalSpeed = !isNormalSpeed;
        if (isNormalSpeed)
        {
            gameController.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = GameSpeedSprites[0];
        }
        else
        {
            gameController.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = GameSpeedSprites[1];
        }
    }
    //��Ϸ��ͣ״̬�л�
    public void ChangePause()
    {
        isPause = !isPause;
        gameController.isPause = isPause;
        if (isPause)
        {
            img_Btn_Pause.sprite = PauseSprites[0];
            emp_PauseGo.SetActive(true);
            emp_PlayingTextGo.SetActive(false);
        }
        else
        {
            img_Btn_Pause.sprite = PauseSprites[1];
            emp_PauseGo.SetActive(false);
            emp_PlayingTextGo.SetActive(true);
        }
    }
    public void ShowMenu()
    {
        normalModelPanel.ShowMenuPage();
    }
#endif
}
