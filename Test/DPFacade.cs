using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFacade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        У�� xz = new У��();
        xz.zj();
    }

    public class У��
    {
        ��ʦ ls = new ��ʦ();
       public void zj()
        {
            ls.��������();
        }
    }

    public class ��ʦ
    {
        �೤ bz = new �೤();
        ѧί xw = new ѧί();
        public void ��������()
        {
            Debug.Log("pp");
            bz.�ܽ�();
            xw.�ܽ�();
        }
    }

    public class �೤
    {
        public void �ܽ�()
        {
            Debug.Log("�೤���ܽ�");
        }
    }

    public class ѧί
    {
        public void �ܽ�()
        {
            Debug.Log("ѧί���ܽ�");
        }
    }
}
