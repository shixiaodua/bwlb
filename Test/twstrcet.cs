using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class twstrcet : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)//ֻ�ܼ��UI
    {
        
        Debug.Log("������Ҽ���" + eventData.pointerId);
    }

    private void OnMouseDown()//����������İ���
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("����");
        }
        else
            if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("����·��");
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
