using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalOptionPanel : BasePanel
{
    public bool isInBigLevelPanel;
    protected override void Awake()
    {
        base.Awake();
        isInBigLevelPanel = true;
    }

    public void ReturnToLastPanel()
    {
        mUIFacade.PlayButton();
        if (isInBigLevelPanel)
        {
            mUIFacade.ChangeSceneState(mUIFacade.mSceneStateManager.mainSenceState);
        }
        else
        {
            mUIFacade.currentSencePanelDist[StringManager.GameNormalLevelPanel].ExitPanel();
            mUIFacade.currentSencePanelDist[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        isInBigLevelPanel = true;
    }

    public override void EnterPanel()
    {
        
    }

    public override void ExitPanel()
    {
        
    }

    public void ToHelpPanel()
    {
        mUIFacade.currentSencePanelDist[StringManager.HelpPanel].EnterPanel();
    }
}
