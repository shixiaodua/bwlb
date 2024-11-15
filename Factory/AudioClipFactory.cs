using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipFactory : IBaseRescrousFactory<AudioClip>
{
    protected Dictionary<string, AudioClip> factoryDist = new Dictionary<string, AudioClip>();

    protected string loadPath;

    public AudioClipFactory()
    {
        loadPath = "AudioClips/";
    }
    public AudioClip GetSingleResources(string resourcesPath)
    {
        AudioClip itemGo;
        if (factoryDist.ContainsKey(resourcesPath))
        {
            itemGo = factoryDist[resourcesPath];
        }
        else
        {
            itemGo = Resources.Load<AudioClip>(resourcesPath);
            factoryDist.Add(resourcesPath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log("无法获取音频资源，这是资源路径" + resourcesPath);
        }
        return itemGo;
    }

   
}
