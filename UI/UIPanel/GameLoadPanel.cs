using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadPanel : BasePanel
{

    protected override void Awake()
    {
        base.Awake();
    }
    public override void InitPanel()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
        transform.SetSiblingIndex(8);
    }

    public override void EnterPanel()
    {
        gameObject.SetActive(true);
    }
    
}
