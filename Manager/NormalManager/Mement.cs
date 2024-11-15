using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class Mement 
{
    //������Ϣ
    public void SavePlayerManager()
    {
        //��ȡ������
        PlayerManager playerManager = GameManager.Instance.playerManager;
        string savePath = Application.dataPath + "/StreamingAssets/Json/PlayerManager.json";
        string json = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(savePath);//����һ��·��ΪsavePath��д����
        sw.Write(json);//д��json����ַ���
        sw.Close();//�ر�д����
    }
    //��ȡ��Ϣ
    public PlayerManager LoadPlayerManager()
    {
        string loadPath = "";
        PlayerManager playerManager;
        if (GameManager.Instance.isInitPlayerManager)//������Ϸ
        {
            loadPath = Application.dataPath + "/StreamingAssets/Json/InitPlayerManager.json";
        }
        else
        {
            loadPath = Application.dataPath + "/StreamingAssets/Json/PlayerManager.json";
        }
        if (File.Exists(loadPath))//�жϵ�ǰ�ļ�·���Ƿ����
        {
            StreamReader sr = new StreamReader(loadPath);
            string json = sr.ReadToEnd();
            sr.Close();
            //����ֻ���޸���ָ��ָ��ĵ�ַ��û���޸�playermanager����
            //GameManager.Instance.playerManager = JsonMapper.ToObject<PlayerManager>(json);
            playerManager= JsonMapper.ToObject<PlayerManager>(json);
            return playerManager;
        }
        else
        {
            Debug.Log("��ǰ�ļ�·��������:" + loadPath);
            return null;
        }
    }
}
