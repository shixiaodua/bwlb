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
        Invoke("DestroyEffect", animationTime);//ÿ���ó�����������Ч�ͷŻض����
    }

    private void DestroyEffect()//�Żض����
    {
        GameController.Instance.PushGameObjectToFactory(resourcePath, gameObject);
    }
#endif
}
