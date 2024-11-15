using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory
{
    //当前拥有的gameobject类型的资源（ui，uipanel，game），唯一的副本
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();

    //对象池字典，某种物体对应哪个对象池
    protected Dictionary<string, Stack<GameObject>> objectPoolDist = new Dictionary<string, Stack<GameObject>>();

    protected string loadPath;//包含当前所有资源文件夹的路径

    public BaseFactory()//构造函数
    {
        loadPath = "Prefabs/";
    }
    //如果一个物体要存入对象池，那么它一定是先从对象池中拿出来
    public GameObject GetItem(string itemName)//获取物体
    {
        GameObject item = null;//初始化物体为空

        if (objectPoolDist.ContainsKey(itemName))//如果物体
        {
            if (objectPoolDist[itemName].Count > 0)
                item = objectPoolDist[itemName].Pop();
            else
            {
                item = GameManager.Instance.CreateItem(factoryDict[itemName]);//获取到脚本对应的实例，调用实例对象的创建物体方法
            }
            
        }
        else
        {
            objectPoolDist.Add(itemName, new Stack<GameObject>());//创建对象池
            GameObject go = GetResources(itemName);//获取资源
            item = GameManager.Instance.CreateItem(go);//获取到脚本对应的实例，调用实例对象的创建物体方法
        }
        if (item == null)
        {
            Debug.Log("无法从工厂中获取到" + itemName + "物体");
        }
        item.SetActive(true);//拿出来先置位显示
        return item;
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objectPoolDist.ContainsKey(itemName))
        {
            objectPoolDist[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前字典没有" + itemName + "对象池");
        }
    }

    private GameObject GetResources(string itemName)
    {
        GameObject itemGo;
        string itemLoadPath = loadPath + itemName;
        if (factoryDict.ContainsKey(itemName))
        {
            itemGo = factoryDict[itemName];
        }
        else
        {
            
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            //Debug.Log(itemGo+itemLoadPath);
            factoryDict.Add(itemName, itemGo);
        }

        if (itemGo == null)
        {
            Debug.Log(itemName+"添加资源路径"+itemLoadPath+"不正确");
        }
        return itemGo;
    }
}
