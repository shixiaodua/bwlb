using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;  
using System.Text;
using UnityEngine.UI;

public class uiTest : MonoBehaviour
{
    public GameObject btn;
    public TextMeshProUGUI txt;
    public Text prTxt;

    public string Key="sk-e830133716cc4840a63f6ace8d60712e";
public void ClickBtn(){
    string s=txt.text;
    Debug.Log(s);
    StartCoroutine(GetPostData(s,Key,null));
    //sk-e830133716cc4840a63f6ace8d60712e
    //请求地址：https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation

}
 /// <summary>
    /// api地址
    /// </summary>
    public string m_ApiUrl = "http://129.211.6.217:38310";
    /// <summary>
    /// gpt-3.5-turbo
    /// </summary>
    //public string m_gptModel = "gpt-3.5-turbo";
    /// <summary>
    /// 缓存对话
    /// </summary>
    [SerializeField]public List<SendData> m_DataList = new List<SendData>();
    /// <summary>
    /// AI人设
    /// </summary>
    public string Prompt="You are a helpful assistant.";

    private void Start()
    {
        //运行时，添加人设
        m_DataList.Add(new SendData("system", Prompt));
        StartCoroutine(GetPostData("你是一个傲娇美少女，请记住这个设定，后面的提问都要以这个设定来进行符合设定的回复",Key,null));
    }

    public 

    /// <summary>
    /// 调用接口
    /// </summary>
    /// <param name="_postWord">发送的消息</param>
    /// <param name="_openAI_Key">密钥</param>
    /// <param name="_callback">GPT的回调</param>
    /// <returns></returns>
     IEnumerator GetPostData(string _postWord,string _openAI_Key, System.Action<string> _callback)
    {
        Debug.Log("进入协程");
        //缓存user（我）发送的信息
        m_DataList.Add(new SendData("user", _postWord));
        Debug.Log("添加信息");
        using (UnityWebRequest request = new UnityWebRequest(m_ApiUrl, "GET"))
        {
            Debug.Log("定义请求");
            PostData _postData = new PostData
            {
                // model = "qwen-turbo",
                // input = new Input(m_DataList),
                // parameters=new Parameters("message")
            };

            string _jsonText = JsonUtility.ToJson(_postData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            Debug.Log("消息体转byte");
            //上传处理程序
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            //下载处理程序
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", _openAI_Key));
            Debug.Log("设置完请求头，发送请求");
            yield return request.SendWebRequest();
            Debug.Log("请求返回状态码");
            Debug.Log(request.responseCode);

                string _msg = request.downloadHandler.text;
                Debug.Log(_msg);
                MessageBack _textback = JsonUtility.FromJson<MessageBack>(_msg);
                if (_textback != null &&_textback.output != null&& _textback.output.choices.Count > 0)
                {

                    string _backMsg = _textback.output.choices[0].message.content;
                    Debug.Log(_backMsg);
                    prTxt.text=_backMsg;
                    //添加记录
                    m_DataList.Add(new SendData("assistant", _backMsg));
                    
                    _callback(_backMsg);
                }

        }
    }

    #region 数据包

    [Serializable]public class PostData
    {
        public string model;
        public Input input;
        public Parameters parameters;
        
    }
     [Serializable]public class Parameters
     {
        public string result_format;
        public Parameters(string s){
            result_format=s;
        }
     }
    [Serializable]public class Input
    {
        public List<SendData> messages;
        public Input(List<SendData> m) {
            messages=m;
        }
    }

    [Serializable]
    public class SendData
    {
        public string role;
        public string content;
        public SendData() { }
        public SendData(string _role,string _content) {
            role = _role;
            content = _content;
        }

    }
    [Serializable]
    public class MessageBack
    {
        public string id;
        public string created;
        public string model;
        public OutPut output;
    }
    [Serializable]
    public class OutPut
    {
        public List<MessageBody> choices;
    }
    [Serializable]
    public class MessageBody
    {
        public Message message;
        public string finish_reason;
        public string index;
    }
    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    #endregion

}
