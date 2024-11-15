using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBossOptionPanel : BasePanel
{
    public void HelpPage()
    {
        mUIFacade.currentSencePanelDist[StringManager.HelpPanel].EnterPanel();
    }
    public void ReturnMain()
    {
        mUIFacade.PlayButton();
        mUIFacade.currentSencePanelDist[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.mainSenceState);
    }
    public void ToGameScene()
    {
        mUIFacade.PlayButton();
        //具体的方法
    }
}
