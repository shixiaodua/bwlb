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
    //�����ַ��https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation

}
 /// <summary>
    /// api��ַ
    /// </summary>
    public string m_ApiUrl = "http://129.211.6.217:38310";
    /// <summary>
    /// gpt-3.5-turbo
    /// </summary>
    //public string m_gptModel = "gpt-3.5-turbo";
    /// <summary>
    /// ����Ի�
    /// </summary>
    [SerializeField]public List<SendData> m_DataList = new List<SendData>();
    /// <summary>
    /// AI����
    /// </summary>
    public string Prompt="You are a helpful assistant.";

    private void Start()
    {
        //����ʱ���������
        m_DataList.Add(new SendData("system", Prompt));
        StartCoroutine(GetPostData("����һ����������Ů�����ס����趨����������ʶ�Ҫ������趨�����з����趨�Ļظ�",Key,null));
    }

    public 

    /// <summary>
    /// ���ýӿ�
    /// </summary>
    /// <param name="_postWord">���͵���Ϣ</param>
    /// <param name="_openAI_Key">��Կ</param>
    /// <param name="_callback">GPT�Ļص�</param>
    /// <returns></returns>
     IEnumerator GetPostData(string _postWord,string _openAI_Key, System.Action<string> _callback)
    {
        Debug.Log("����Э��");
        //����user���ң����͵���Ϣ
        m_DataList.Add(new SendData("user", _postWord));
        Debug.Log("�����Ϣ");
        using (UnityWebRequest request = new UnityWebRequest(m_ApiUrl, "GET"))
        {
            Debug.Log("��������");
            PostData _postData = new PostData
            {
                // model = "qwen-turbo",
                // input = new Input(m_DataList),
                // parameters=new Parameters("message")
            };

            string _jsonText = JsonUtility.ToJson(_postData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            Debug.Log("��Ϣ��תbyte");
            //�ϴ��������
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            //���ش������
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", _openAI_Key));
            Debug.Log("����������ͷ����������");
            yield return request.SendWebRequest();
            Debug.Log("���󷵻�״̬��");
            Debug.Log(request.responseCode);

                string _msg = request.downloadHandler.text;
                Debug.Log(_msg);
                MessageBack _textback = JsonUtility.FromJson<MessageBack>(_msg);
                if (_textback != null &&_textback.output != null&& _textback.output.choices.Count > 0)
                {

                    string _backMsg = _textback.output.choices[0].message.content;
                    Debug.Log(_backMsg);
                    prTxt.text=_backMsg;
                    //��Ӽ�¼
                    m_DataList.Add(new SendData("assistant", _backMsg));
                    
                    _callback(_backMsg);
                }

        }
    }

    #region ���ݰ�

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
