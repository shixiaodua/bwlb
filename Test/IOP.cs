using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        qi w = new duqi();
        w.hp=1;
        Debug.Log(w.hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public interface Iduqi
    {
        void Duqi1();
        void Duqi2();
        void Duqi3();
    }

    public class qi : Iduqi
    {
        public int hp;
        virtual public void Duqi1()
        {
            Debug.Log("111");
        }

        public void Duqi2()
        {
            Debug.Log("222");
        }

        public void Duqi3()
        {
            Debug.Log("333");
        }
    }
    public class duqi : qi
    {
        override public void Duqi1()
        {
            //base.Duqi1();
            Debug.Log("1");
        }

        public void Duqi2()
        {
            Debug.Log("2");
        }

        public void Duqi3()
        {
            Debug.Log("3");
        }
    }

    public class du : Iduqi
    {
        public void Duqi1()
        {
            Debug.Log("11");
        }

        public void Duqi2()
        {
            Debug.Log("22");
        }

        public void Duqi3()
        {
            Debug.Log("33");
        }
    }
}
