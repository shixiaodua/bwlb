using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainSenceState : BaseSenceState
{
    public override void EnterSence()
    {
        SceneManager.LoadScene(1);//��ת�����ķ���
        mUIFacade.AddUIPanelToDist(StringManager.MainPanel);
        mUIFacade.AddUIPanelToDist(StringManager.SetPanel);
        mUIFacade.AddUIPanelToDist(StringManager.HelpPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameLoadPanel);
        base.EnterSence();

        GameManager.Instance.uiManager.mUIFacade.PlayBGMusic();
    }
    public override void ExitSence()
    {
        base.ExitSence();
    }
}
