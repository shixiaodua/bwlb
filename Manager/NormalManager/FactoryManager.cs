using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//工厂管理，负责管理各种类型的工厂以及对象池
public class FactoryManager 
{
    public Dictionary<FactoryType, IBaseFactory> factoryDist = new Dictionary<FactoryType, IBaseFactory>();
    public AudioClipFactory audioClipFactory;
    public SpriteFactory spriteFactory;
    public RuntimeAnimatorControllerFactory runtimeAnimatorControllerFactory;

    public FactoryManager()
    {
        audioClipFactory = new AudioClipFactory();
        spriteFactory = new SpriteFactory();
        runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();

        factoryDist.Add(FactoryType.GameFactory, new GameFactory());
        factoryDist.Add(FactoryType.UIFactory, new UIFactory());
        factoryDist.Add(FactoryType.UIPanelFactory, new UIPanelFactory());
    }
}
