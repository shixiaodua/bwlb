using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NormalGameOptionSenceState : BaseSenceState
{
    public override void EnterSence()
    {
        SceneManager.LoadScene(2);//跳转场景的方法
        mUIFacade.AddUIPanelToDist(StringManager.GameNormalOptionPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameLoadPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameNormalBigLevelPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameNormalLevelPanel);
        mUIFacade.AddUIPanelToDist(StringManager.HelpPanel);
        base.EnterSence();
        GameManager.Instance.uiManager.mUIFacade.PlayBGMusic();
    }
    public override void ExitSence()
    {
        base.ExitSence();
    }
}
