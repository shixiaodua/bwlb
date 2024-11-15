using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class Mement 
{
    //保存信息
    public void SavePlayerManager()
    {
        //获取到引用
        PlayerManager playerManager = GameManager.Instance.playerManager;
        string savePath = Application.dataPath + "/StreamingAssets/Json/PlayerManager.json";
        string json = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(savePath);//创建一个路径为savePath的写入流
        sw.Write(json);//写入json这个字符串
        sw.Close();//关闭写入流
    }
    //读取信息
    public PlayerManager LoadPlayerManager()
    {
        string loadPath = "";
        PlayerManager playerManager;
        if (GameManager.Instance.isInitPlayerManager)//重置游戏
        {
            loadPath = Application.dataPath + "/StreamingAssets/Json/InitPlayerManager.json";
        }
        else
        {
            loadPath = Application.dataPath + "/StreamingAssets/Json/PlayerManager.json";
        }
        if (File.Exists(loadPath))//判断当前文件路径是否存在
        {
            StreamReader sr = new StreamReader(loadPath);
            string json = sr.ReadToEnd();
            sr.Close();
            //这样只是修改了指针指向的地址，没有修改playermanager本身
            //GameManager.Instance.playerManager = JsonMapper.ToObject<PlayerManager>(json);
            playerManager= JsonMapper.ToObject<PlayerManager>(json);
            return playerManager;
        }
        else
        {
            Debug.Log("当前文件路径不存在:" + loadPath);
            return null;
        }
    }
}
