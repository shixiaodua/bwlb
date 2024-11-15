using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class twstrcet : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)//只能监测UI
    {
        
        Debug.Log("鼠标左右键：" + eventData.pointerId);
    }

    private void OnMouseDown()//监测鼠标左键的按下
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("建塔");
        }
        else
            if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("怪物路点");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
       // Debug.Log(Singleton.Instance.w);
    }

   
}
