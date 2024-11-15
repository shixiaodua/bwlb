using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartLoadSenceState : BaseSenceState
{
    public StartLoadSenceState()
    {

    }
    public override void EnterSence()//��δ�����ű���δ���ó�ʼλ��
    {
        //SceneManager.LoadScene(1);//��ת�����ķ���
        mUIFacade.AddUIPanelToDist(StringManager.StartLoadPanel);
        base.EnterSence();
    }
    public override void ExitSence()
    {
        base.ExitSence();
    }
}
