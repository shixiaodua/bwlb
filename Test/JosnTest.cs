using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class JosnTest : MonoBehaviour
{
    private App app;
    void Start()
    {
        //app = new App()
        //{
        //    phoneState = true,
        //    appNum = 3,
        //    appList = new List<string>()
        //    {
        //        "������ҫ", "�̼�ս��", "����̫��"
        //    }
        //};
        SaveByJson();
       // Debug.Log(String.Join(", ", LoadByJson().appList));
    }
    public void SaveByJson()//�洢json�ļ�
    {
        string path = "D:/unity duqi/�����ܲ�/Assets/Resources/Json/App.json";
        string appToJson = JsonMapper.ToJson(app);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(appToJson);
        sw.Close();
    }

    public App LoadByJson()//��ȡjson�ļ�
    {
        string path = "D:/unity duqi/�����ܲ�/Assets/Resources/Json/App.json";
        App appGo = new App();
        string json;//�洢json�ļ��е��ַ���
        if (File.Exists(path))//�ж��ļ�·���Ƿ����
        {
            StreamReader sr = new StreamReader(path);
            json = sr.ReadToEnd();
            sr.Close();
            appGo = JsonMapper.ToObject<App>(json);
        }
        if(appGo==null)
        {
            Debug.Log("��ȡ��Ϣʧ��");
        }
        return appGo;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
