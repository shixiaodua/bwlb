using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����������Դ�����ӿڣ���Դ���Ϳ��ܲ�ͬ���ʹ�÷���
public interface IBaseRescrousFactory<T>
{
    T GetSingleResources(string resourcesPath);

}
