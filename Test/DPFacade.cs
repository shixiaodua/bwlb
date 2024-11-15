using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFacade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        校长 xz = new 校长();
        xz.zj();
    }

    public class 校长
    {
        老师 ls = new 老师();
       public void zj()
        {
            ls.分派任务();
        }
    }

    public class 老师
    {
        班长 bz = new 班长();
        学委 xw = new 学委();
        public void 分派任务()
        {
            Debug.Log("pp");
            bz.总结();
            xw.总结();
        }
    }

    public class 班长
    {
        public void 总结()
        {
            Debug.Log("班长的总结");
        }
    }

    public class 学委
    {
        public void 总结()
        {
            Debug.Log("学委的总结");
        }
    }
}
