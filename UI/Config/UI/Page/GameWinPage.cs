using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameWinPage : MonoBehaviour
{
    private Text tex_RoundNum;
    private Text tex_TotalRound;
    private Text tex_CurrentLevel;
    private Image img_Carrot;
    private Sprite[] carrotSprites;
    private NormalModelPanel normalModelPanel;
    private GameController gameController;
#if Game
    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
        tex_RoundNum = transform.Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalRound = transform.Find("Tex_TotalCount").GetComponent<Text>();
        img_Carrot = transform.Find("Image").GetComponent<Image>();

        carrotSprites = new Sprite[3];
        for(int i = 1; i <= 3; i++)
        {
            carrotSprites[i - 1] = GameManager.Instance.GetSprite("Pictures/GameOption/Normal/Level/Carrot_" + i.ToString());
        }

    }
    private void OnEnable()
    {
        gameController = GameController.Instance;
        tex_TotalRound.text = normalModelPanel.totalRound.ToString();
        tex_CurrentLevel.text = (GameManager.Instance.currentStage.mLevelID + (GameManager.Instance.currentStage.mBigLevelID - 1) * 5).ToString();
        normalModelPanel.UpdateRoundText(tex_RoundNum);
        img_Carrot.sprite = carrotSprites[gameController.GetCarrotState() - 1];
    }
    public void RePlay()
    {
        normalModelPanel.RePlay();
    }
    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
#endif
}
