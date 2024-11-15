using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
#if Game
    //��ȡ����Ϸ�������ϵĽű�
    T GetProductClass(GameObject gameObject);

    //ͨ��������ȡ����Ϸ����
    GameObject GetProduct();

    //��ȡ��Ϣ
    void GetData(T ProductClass);

    //��ȡ������Դ��Ϣ
    void GetOtherResource(T ProductClassGo);
#endif
}
