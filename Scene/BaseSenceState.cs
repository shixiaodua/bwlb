using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSenceState : IBaseSenceState
{
    protected UIFacade mUIFacade;

    public BaseSenceState()
    {
        //Debug.Log(GameManager.Instance.uiManager);
        mUIFacade = GameManager.Instance.uiManager.mUIFacade;
    }
    public virtual void EnterSence()
    {
        mUIFacade.InitDist();
    }

    public virtual void ExitSence()
    {
        mUIFacade.ClearDist();//����廹������ز���������ű��ֵ�������ֵ�
    }

    
}
