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

public class UIPanel : BasePanel
{
    //����
    private PlayerManager playerManager;
    public TextMeshProUGUI sendTxt;
    private Sprite[] playerSpr;
    //����
    private int level, ID;
    private bool isSend = true;
    protected override void Awake()
    {
        Debug.Log("����uipanel");
        playerSpr = new Sprite[3];
        for (int i = 1; i <= 3; i++)
        {
            playerSpr[i - 1] = GameManager.Instance.GetSprite("Pictures/MonsterNest/UIPanel/MonsterPlayer/" + i.ToString());
        }
        base.Awake();
    }
    public override void InitPanel()
    {

        base.InitPanel();
        transform.gameObject.SetActive(false);
    }
    public void ToTishPanel(int lev, int id)
    {
        Debug.Log("����");
        ID = id;
        level = lev;
        EnterPanel();
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        transform.gameObject.SetActive(true);
        isSend = true;
        //��ȡ��ʷ�����¼
        playerManager = mUIFacade.mPlayerManager;
        if (playerManager.AIDataList == null||playerManager.AIDataList.Count==0)
        {
            playerManager.AIDataList = new Dictionary<string, List<SendData>>();
            for (int i = 0; i < 3; i++)
            {
                playerManager.AIDataList[i.ToString()] = new List<SendData>();
            }
        }
        Debug.Log(ID);
        Debug.Log(playerManager.AIDataList[ID.ToString()].Count);
        if(playerManager.AIDataList[ID.ToString()].Count>=0){
        for (int i = 0; i < playerManager.AIDataList[ID.ToString()].Count; i++)
        {
            m_DataList.Add(playerManager.AIDataList[ID.ToString()][i]);
        }
        }
        //��ʼ������
        for (int i = 3; i < m_DataList.Count; i++)
        {
            if (m_DataList[i].role == StringManager.assistant)
            {
                LeftMessage(m_DataList[i].role, m_DataList[i].content);
            }
            else
            {
                RightMessage(m_DataList[i].role, m_DataList[i].content);
            }
        }
        //��һ������ʱ���������
        if (m_DataList.Count == 0)
        {
            Debug.Log(ID);
            m_DataList.Add(new SendData("system", Prompt));
            StartCoroutine(GetPostData(MonsterConfig.initAi[ID], Key, null));
        }
    }
    //��չ�߶�
    private void UpdateContent()
    {
        Transform k = transform.Find("BG/Scroll View/Viewport/Content");
        
        float h=k.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;
        if(h>=412){
            k.GetComponent<RectTransform>().sizeDelta=new Vector2(0,h-412);
        }
    }
    private void UpdateMessage(float gd)
    {
        Transform k = transform.Find("BG/Scroll View/Viewport/Content");
        for(int i=0;i<k.childCount;i++){
            RectTransform re=k.GetChild(i).GetComponent<RectTransform>();
            re.anchoredPosition=new Vector2(0, re.anchoredPosition.y+gd);
        }
    }
    public void ClickSend()
    {
        if (sendTxt.text != "" && isSend)
        {
            isSend = false;
            StartCoroutine(GetPostData(sendTxt.text, Key, Sendcallback));
            //����
            RightMessage(StringManager.user, sendTxt.text);
        }
    }
    //������Ϣ���غ�Ļص�
    private void Sendcallback(string txt)
    {
        isSend = true;
        //����
        LeftMessage(StringManager.assistant, txt);
    }
    //����
    private void LeftMessage(string name, string txt)
    {
        GameObject left = mUIFacade.GetGameObject(FactoryType.UIFactory, "PlayerLeft");
        left.transform.Find("MessageTxt").GetComponent<TextMeshProUGUI>().text = txt;
        left.transform.Find("NameTxt").GetComponent<Text>().text = name;
        left.transform.Find("Image").GetComponent<Image>().sprite = playerSpr[ID];
        float gd=left.transform.Find("MessageTxt").GetComponent<TextMeshProUGUI>().preferredHeight+50;
        Debug.Log("��ǰ�ı���߶ȣ�"+gd);
        UpdateMessage(gd);
        left.transform.SetParent(transform.Find("BG/Scroll View/Viewport/Content"));
        left.transform.localScale = new Vector3(1f, 1f, 1f);
        left.transform.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,gd);
        UpdateContent();
    }
    private void RightMessage(string name, string txt)
    {
        GameObject left = mUIFacade.GetGameObject(FactoryType.UIFactory, "PlayerRight");
        left.transform.Find("MessageTxt").GetComponent<TextMeshProUGUI>().text = txt;
        left.transform.Find("NameTxt").GetComponent<Text>().text = name;
        float gd=left.transform.Find("MessageTxt").GetComponent<TextMeshProUGUI>().preferredHeight+50;
        Debug.Log("��ǰ�ı���߶ȣ�"+gd);
        UpdateMessage(gd);
        left.transform.SetParent(transform.Find("BG/Scroll View/Viewport/Content"));
        left.transform.localScale = new Vector3(1f, 1f, 1f);
        left.transform.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,gd);
        UpdateContent();
    }
    public void ClickReturnBtn()
    {
        ExitPanel();
        mUIFacade.currentSencePanelDist[StringManager.MonsterNestPanel].EnterPanel();
    }
    //�˳����
    public override void ExitPanel()
    {
        base.ExitPanel();  
        //��ʼ��content
        Transform kk = transform.Find("BG/Scroll View/Viewport/Content");
        Debug.Log(transform,kk);
        kk.GetComponent<RectTransform>().sizeDelta=new Vector2(0,0);
        transform.gameObject.SetActive(false);
        //������Ϣ
        List<SendData> k1 = new List<SendData>();
        m_DataList.ForEach(i => k1.Add(i));
        playerManager.AIDataList[ID.ToString()] = k1;
        playerManager.SaveData();
        //����Ϣȫ����������
        GameObject k = transform.Find("BG/Scroll View/Viewport/Content").gameObject;
        for (int i = 0; i < k.transform.childCount; i++)
        {
            Destroy(k.transform.GetChild(i).gameObject);
        }
        //�ж��Ƿ�洢��������
        if (playerManager.AIDataList[ID.ToString()].Count >= 100) playerManager.AIDataList[ID.ToString()].Clear();
        //�����Ϣ
        m_DataList.Clear();
    }
    public string Key = "sk-e830133716cc4840a63f6ace8d60712e";
    public string m_ApiUrl = "https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation";

    /// <summary>
    /// gpt-3.5-turbo
    /// </summary>
    //public string m_gptModel = "gpt-3.5-turbo";
    /// <summary>p
    /// ����Ի�
    /// </summary>
    [SerializeField] private List<SendData> m_DataList = new List<SendData>();
    /// <summary>
    /// AI����
    /// </summary>
    private string Prompt = "You are a helpful assistant.";

    public
    IEnumerator GetPostData(string _postWord, string _openAI_Key, System.Action<string> _callback)
    {
        //����user���ң����͵���Ϣ
        m_DataList.Add(new SendData("user", _postWord));
        using (UnityWebRequest request = new UnityWebRequest(m_ApiUrl, "POST"))
        {
            PostData _postData = new PostData
            {
                model = "qwen-turbo",
                input = new Input(m_DataList),
                parameters = new Parameters("message")
            };

            string _jsonText = JsonUtility.ToJson(_postData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            //�ϴ��������
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            //���ش������
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", _openAI_Key));
            yield return request.SendWebRequest();

            string _msg = request.downloadHandler.text;
            MessageBack _textback = JsonUtility.FromJson<MessageBack>(_msg);
            if (_textback != null && _textback.output != null && _textback.output.choices.Count > 0)
            {
                string _backMsg = _textback.output.choices[0].message.content;
                //prTxt.text=_backMsg;
                //��Ӽ�¼
                m_DataList.Add(new SendData("assistant", _backMsg));

                if (_callback != null) _callback(_backMsg);
            }

        }
    }

    #region ���ݰ�

    [Serializable]
    public class PostData
    {
        public string model;
        public Input input;
        public Parameters parameters;

    }
    [Serializable]
    public class Parameters
    {
        public string result_format;
        public Parameters(string s)
        {
            result_format = s;
        }
    }
    [Serializable]
    public class Input
    {
        public List<SendData> messages;
        public Input(List<SendData> m)
        {
            messages = m;
        }
    }

    [Serializable]
    public class SendData
    {
        public string role;
        public string content;
        public SendData() { }
        public SendData(string _role, string _content)
        {
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