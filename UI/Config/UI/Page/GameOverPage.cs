using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverPage : MonoBehaviour
{
    private Text tex_RoundNum;
    private Text tex_TotalRound;
    private Text tex_CurrentLevel;
    private NormalModelPanel normalModelPanel;
#if Game
    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
        tex_RoundNum = transform.Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalRound = transform.Find("Tex_TotalCount").GetComponent<Text>();

    }
    private void OnEnable()
    {
        tex_TotalRound.text = normalModelPanel.totalRound.ToString();
        tex_CurrentLevel.text = (GameManager.Instance.currentStage.mLevelID+(GameManager.Instance.currentStage.mBigLevelID-1)*5).ToString();
        normalModelPanel.UpdateRoundText(tex_RoundNum);
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
