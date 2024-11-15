using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnimatorControllerFactory : IBaseRescrousFactory<RuntimeAnimatorController>
{
    protected Dictionary<string, RuntimeAnimatorController> factoryDist = new Dictionary<string, RuntimeAnimatorController>();

    protected string loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        loadPath = "Animator/";
    }
    public RuntimeAnimatorController GetSingleResources(string resourcesPath)
    {
        RuntimeAnimatorController itemGo;
        if (factoryDist.ContainsKey(resourcesPath))
        {
            itemGo = factoryDist[resourcesPath];
        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(resourcesPath);
            factoryDist.Add(resourcesPath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log("无法获取动画控制器资源，这是资源路径" + resourcesPath);
        }
        return itemGo;
    }

}



