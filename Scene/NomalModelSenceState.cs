using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NomalModelSenceState : BaseSenceState
{
    public override void EnterSence()
    {
        SceneManager.LoadScene(3);//��ת�����ķ���
        mUIFacade.AddUIPanelToDist(StringManager.GameLoadPanel);
        mUIFacade.AddUIPanelToDist(StringManager.NormalModelPanel);
        base.EnterSence();
        GameManager.Instance.audioSourceManager.PlayBGMusic(null);
    }
    public override void ExitSence()
    {
        base.ExitSence();
    }
}
