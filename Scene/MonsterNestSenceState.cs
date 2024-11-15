using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MonsterNestSenceState : BaseSenceState
{
    public override void EnterSence()
    {
        SceneManager.LoadScene(6);
        mUIFacade.AddUIPanelToDist(StringManager.MonsterNestPanel);
        mUIFacade.AddUIPanelToDist(StringManager.GameLoadPanel);
        mUIFacade.AddUIPanelToDist(StringManager.UIPanle);
        base.EnterSence();
    }
    public override void ExitSence()
    {
        base.ExitSence();
    }
}
