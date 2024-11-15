using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float animationTime;
    public string resourcePath;
#if Game
    private void OnEnable()
    {
        Invoke("DestroyEffect", animationTime);//每次拿出来播放完特效就放回对象池
    }

    private void DestroyEffect()//放回对象池
    {
        GameController.Instance.PushGameObjectToFactory(resourcePath, gameObject);
    }
#endif
}
