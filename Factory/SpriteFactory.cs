using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : IBaseRescrousFactory<Sprite>
{
    protected Dictionary<string, Sprite> factoryDist = new Dictionary<string, Sprite>();

    protected string loadPath;

    public SpriteFactory()
    {
        loadPath = "Pictrues/";
    }
    public Sprite GetSingleResources(string resourcesPath)
    {
        Sprite itemGo;
        if (factoryDist.ContainsKey(resourcesPath))
        {
            itemGo = factoryDist[resourcesPath];
        }
        else
        {
            itemGo = Resources.Load<Sprite>(resourcesPath);
            factoryDist.Add(resourcesPath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log("�޷���ȡͼƬ��Դ��������Դ·��" + resourcesPath);
        }
        return itemGo;
    }

}
