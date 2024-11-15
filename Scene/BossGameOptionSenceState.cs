using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameOptionSenceState : BaseSenceState
{
    public override void EnterSence()
    {
        mUIFacade.AddUIPanelToDist(StringManager.GameLoadPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameBossOptionPanel);
        mUIFacade.AddUIPanelToDist(StringManager.HelpPanel);
        base.EnterSence();
    }
}
