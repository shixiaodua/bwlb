using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������UI�Ĺ�����
public class UIManager 
{
    public UIFacade mUIFacade;
    public GameManager mGameManager;
    public Dictionary<string, GameObject> currentSencePanelDist;//��ǰ��������ֵ�

    public UIManager()
    {
        mUIFacade = new UIFacade(this);
        
        //Debug.Log(mUIFacade.currentSceneState);
        mGameManager = GameManager.Instance;
        currentSencePanelDist = new Dictionary<string, GameObject>();
    }

    public void ClearDist()//����ֵ䣬���������������
    {
        foreach(var item in currentSencePanelDist)
        {
            mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, item.Key, item.Value);
        }
        currentSencePanelDist.Clear();
    }
}
