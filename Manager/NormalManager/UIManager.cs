using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//负责管理UI的管理者
public class UIManager 
{
    public UIFacade mUIFacade;
    public GameManager mGameManager;
    public Dictionary<string, GameObject> currentSencePanelDist;//当前场景面板字典

    public UIManager()
    {
        mUIFacade = new UIFacade(this);
        
        //Debug.Log(mUIFacade.currentSceneState);
        mGameManager = GameManager.Instance;
        currentSencePanelDist = new Dictionary<string, GameObject>();
    }

    public void ClearDist()//清空字典，并将物体放入对象池
    {
        foreach(var item in currentSencePanelDist)
        {
            mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, item.Key, item.Value);
        }
        currentSencePanelDist.Clear();
    }
}
