using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//其他种类资源工厂接口，资源类型可能不同因此使用范形
public interface IBaseRescrousFactory<T>
{
    T GetSingleResources(string resourcesPath);

}
