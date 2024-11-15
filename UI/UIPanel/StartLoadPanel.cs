using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        Invoke("LoadNextScene", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadNextScene()
    {
        mUIFacade.ChangeSceneState(new MainSenceState());
    }
}
