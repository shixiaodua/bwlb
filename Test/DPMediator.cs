using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPMediator : MonoBehaviour
{
    public zjz zz;
    // Start is called before the first frame update
    void Start()
    {
        zz = new zjz();
        yh w1 = new yh("w1",zz);
        yh w2 = new yh("w2", zz);
        yh w3 = new yh("w3", zz);

        w1.xx("1",w2);
        w2.xx("2",w3);
        w3.xx("3",w1);
    }

   
    

    
}

public class zjz
    {
       public void xx(string name,string s,yh ww)
        {
            Debug.Log(name + ":" + s+":"+ww.name);
        }
    }
public class yh
    {
        public string name;

    private zjz zz;
    public yh(string name,zjz zz)
    {
        this.name = name;
        this.zz = zz;
    }
        public void xx(string s,yh ww)
        {
        zz.xx(name,s,ww);
        }
    }