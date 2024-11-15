using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory
{
    //��ǰӵ�е�gameobject���͵���Դ��ui��uipanel��game����Ψһ�ĸ���
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();

    //������ֵ䣬ĳ�������Ӧ�ĸ������
    protected Dictionary<string, Stack<GameObject>> objectPoolDist = new Dictionary<string, Stack<GameObject>>();

    protected string loadPath;//������ǰ������Դ�ļ��е�·��

    public BaseFactory()//���캯��
    {
        loadPath = "Prefabs/";
    }
    //���һ������Ҫ�������أ���ô��һ�����ȴӶ�������ó���
    public GameObject GetItem(string itemName)//��ȡ����
    {
        GameObject item = null;//��ʼ������Ϊ��

        if (objectPoolDist.ContainsKey(itemName))//�������
        {
            if (objectPoolDist[itemName].Count > 0)
                item = objectPoolDist[itemName].Pop();
            else
            {
                item = GameManager.Instance.CreateItem(factoryDict[itemName]);//��ȡ���ű���Ӧ��ʵ��������ʵ������Ĵ������巽��
            }
            
        }
        else
        {
            objectPoolDist.Add(itemName, new Stack<GameObject>());//���������
            GameObject go = GetResources(itemName);//��ȡ��Դ
            item = GameManager.Instance.CreateItem(go);//��ȡ���ű���Ӧ��ʵ��������ʵ������Ĵ������巽��
        }
        if (item == null)
        {
            Debug.Log("�޷��ӹ����л�ȡ��" + itemName + "����");
        }
        item.SetActive(true);//�ó�������λ��ʾ
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
            Debug.Log("��ǰ�ֵ�û��" + itemName + "�����");
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
            Debug.Log(itemName+"�����Դ·��"+itemLoadPath+"����ȷ");
        }
        return itemGo;
    }
}
