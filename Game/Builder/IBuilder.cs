using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
#if Game
    //获取到游戏物体身上的脚本
    T GetProductClass(GameObject gameObject);

    //通过工厂获取到游戏物体
    GameObject GetProduct();

    //获取信息
    void GetData(T ProductClass);

    //获取其他资源信息
    void GetOtherResource(T ProductClassGo);
#endif
}
