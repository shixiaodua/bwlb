using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
#if Game
    private void OnMouseDown()
    {
        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
        GameController.Instance.ShowPrizePage();
    }
#endif
}
