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
public class Context//״̬��
{
    private IState mState;//��ǰ״̬

    public void SetState(IState state)//����״̬
    {
        mState = state;
    }

    public void Handle()//���õ�ǰ״̬�ķ���
    {
        mState.Handle();
    }
}

public class Eat : IState
{
    
    public void Handle()
    {
        Debug.Log("�����ڳԷ�");
    }
}

public class Work : IState
{
    
    public void Handle()
    {
        Debug.Log("�����ڹ���");
    }
}

public class Sleep : IState
{
   
    public void Handle()
    {
        Debug.Log("������˯��");
    }
}