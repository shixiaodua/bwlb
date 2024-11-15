using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    private NormalModelPanel normalModelPanel;
    private GameController gameController;
#if Game
    private void Awake()
    {
        gameController = GameController.Instance;
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }
    public void GoOn()
    {
        normalModelPanel.CloseMenuPage();
    }
    public void Replay()
    {
        normalModelPanel.RePlay();
    }
    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
#endif
}
