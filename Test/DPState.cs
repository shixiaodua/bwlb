using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPState : MonoBehaviour
{
    private void Start()
    {
        Context context = new Context();
        context.SetState(new Eat());
        context.Handle();
        context.SetState(new Sleep());
        context.Handle();
    }

}
public interface IState
{
    void Handle();
}
public class Context//状态机
{
    private IState mState;//当前状态

    public void SetState(IState state)//设置状态
    {
        mState = state;
    }

    public void Handle()//调用当前状态的方法
    {
        mState.Handle();
    }
}

public class Eat : IState
{
    
    public void Handle()
    {
        Debug.Log("我正在吃饭");
    }
}

public class Work : IState
{
    
    public void Handle()
    {
        Debug.Log("我正在工作");
    }
}

public class Sleep : IState
{
   
    public void Handle()
    {
        Debug.Log("我正在睡觉");
    }
}