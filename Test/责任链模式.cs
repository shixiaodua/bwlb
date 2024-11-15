using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 责任链模式 : MonoBehaviour
{
    Luoli w = new Luoli();
    private void Start()
    {
        w.ff(类型.小姐姐);
    }
}

public enum 类型{
    萝莉,
    小姐姐,
    御姐,
}

//责任链基类
public abstract class BaseHadler
{
    protected BaseHadler nextHadler;
    public abstract void ff(类型 lx);

}

public class Luoli : BaseHadler
{
    public Luoli()
    {
        nextHadler = new XiaoJieJie();
    }
    public override void ff(类型 lx)
    {
        if (lx == 类型.萝莉)
        {
            Debug.Log("萝莉");
        }
        else
        {
            nextHadler.ff(lx);
        }
    }
}

public class XiaoJieJie : BaseHadler
{
    public XiaoJieJie()
    {
        nextHadler = new YuJie();
    }
    public override void ff(类型 lx)
    {
        if (lx == 类型.小姐姐)
        {
            Debug.Log("你好呀，小姐姐很漂亮呢");
        }
        else
        {
            nextHadler.ff(lx);
        }
    }
}

public class YuJie : BaseHadler
{
    public YuJie()
    {
        nextHadler = null;
    }
    public override void ff(类型 lx)
    {
        if (lx == 类型.御姐)
        {
            Debug.Log("你好呀，御姐很漂亮呢");
        }
        else
        {
            Debug.Log("无法交流");
        }
    }
}