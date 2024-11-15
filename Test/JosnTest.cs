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
        //        "王者荣耀", "刺激战场", "及你太美"
        //    }
        //};
        SaveByJson();
       // Debug.Log(String.Join(", ", LoadByJson().appList));
    }
    public void SaveByJson()//存储json文件
    {
        string path = "D:/unity duqi/保卫萝卜/Assets/Resources/Json/App.json";
        string appToJson = JsonMapper.ToJson(app);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(appToJson);
        sw.Close();
    }

    public App LoadByJson()//读取json文件
    {
        string path = "D:/unity duqi/保卫萝卜/Assets/Resources/Json/App.json";
        App appGo = new App();
        string json;//存储json文件中的字符串
        if (File.Exists(path))//判断文件路径是否存在
        {
            StreamReader sr = new StreamReader(path);
            json = sr.ReadToEnd();
            sr.Close();
            appGo = JsonMapper.ToObject<App>(json);
        }
        if(appGo==null)
        {
            Debug.Log("读取信息失败");
        }
        return appGo;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
