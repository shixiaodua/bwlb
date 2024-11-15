using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFactory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Factory8 gc=new Factory8();
        gc.sc().Ff();
        gc.sccd().Ff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public  class Iphone
{
   public virtual void Ff()
    {

    }
}

public class Iphone8:Iphone
{
    public override void Ff()
    {
        Debug.Log("我是Iphone8");
    }
}

public class Iphone9 : Iphone
{
    public override void Ff()
    {
        Debug.Log("我是Iphone9");
    }
}


public class Iphonecd
{
    public virtual void Ff()
    {

    }
}

public class Iphone8cd : Iphonecd
{
    public override void Ff()
    {
        Debug.Log("我是Iphone8cd");
    }
}

public class Iphone9cd : Iphonecd
{
    public override void Ff()
    {
        Debug.Log("我是Iphone9cd");
    }
}

public interface Factory
{
    Iphone sc();
    Iphonecd sccd();
}

public class Factory8 : Factory
{
    public Iphone sc()
    {
        return new Iphone8();
    }

    public Iphonecd sccd()
    {
        return new Iphone8cd();
    }
}

public class Factory9 : Factory
{
    public Iphone sc()
    {
        return new Iphone9();
    }

    public Iphonecd sccd()
    {
        return new Iphone9cd();
    }
}
