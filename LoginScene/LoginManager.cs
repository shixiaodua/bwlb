using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Xml;
using System;

public class LoginManager : MonoBehaviour {
    private static LoginManager _instance;
    public static LoginManager Ins {
        get {
            return _instance;
        }
    }
    public TMP_InputField login_account_input_field;
    public TMP_InputField login_password_input_field;
    public TMP_InputField register_account_input_field;
    public TMP_InputField register_password_input_field;
    public TMP_InputField register_confirm_password_input_field;
    public GameObject login_panel;
    public GameObject register_panel;
    public GameObject register_success_panel;
    public Toggle is_remember_password_toggle;
    public TMP_Text login_account_tip_text;
    public TMP_Text login_password_tip_text;
    public TMP_Text register_account_tip_text;
    public TMP_Text register_password_tip_text;
    public TMP_Text register_confirm_password_tip_text;
    private string Url="http://129.211.6.217:38310";
    private Dictionary<string,string> accountDict=new Dictionary<string, string>();
    private void Awake() {
        _instance = this;
    }
    private void Start() {
        login_account_tip_text.text = "";
        login_password_tip_text.text = "";
        register_account_tip_text.text = "";
        register_password_tip_text.text = "";
        register_confirm_password_tip_text.text = "";
        login_account_input_field.text = PlayerPrefs.GetString("account", "");
        login_password_input_field.text = PlayerPrefs.GetString("password", "");
        LoginPanel();
    }
    public void Login() {
        if (CheckLoginAccount() && CheckLoginPassword()) {
            var account = login_account_input_field.text;
            var password = login_password_input_field.text;
            Debug.Log("[Login] " + account + ": " + password);
            StartCoroutine(GetPostLogin(account,password));
        }
    }

    IEnumerator GetPostLogin(string account,string password)
    {
        Debug.Log("�����¼��������");
        using (UnityWebRequest request = new UnityWebRequest(Url+"/login", "POST"))
        {
            Debug.Log("׼����ʼ����");
            LoginRequest logdata=new LoginRequest(){
                account=account,
                password=password,
            };
            string _jsonText = JsonUtility.ToJson(logdata);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            Debug.Log("���л�");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            string _msg = request.downloadHandler.text;
                Debug.Log(_msg);
                LoginResponse _textback = JsonUtility.FromJson<LoginResponse>(_msg);
            if (_textback.is_success) {
                Debug.Log("��¼�ɹ�");
                //GrpcService.Ins.user_id = result.user_id;
                if (is_remember_password_toggle.isOn) {
                    PlayerPrefs.SetString("account", account);
                    PlayerPrefs.SetString("password", password);
                }
                else {
                    PlayerPrefs.SetString("account", account);
                    PlayerPrefs.SetString("password", "");
                }
                //TODO
                SceneManager.LoadScene(0);
            }
            else {
                Debug.Log("��¼ʧ��");
            }

        }
    }
    public void Register() {
        Debug.Log("p");
        if (CheckRegisterAccount() && CheckRegisterPassword() && CheckRegisterConfirmPassword()) {
            var account = register_account_input_field.text;
            var password = register_password_input_field.text;
            Debug.Log("[Register] " + account + ": " + password);
            StartCoroutine(GetPostRegister(account, password));
        }
    }
     IEnumerator GetPostRegister(string account,string password)
    {
        Debug.Log("����ע�᷽������");
        using (UnityWebRequest request = new UnityWebRequest(Url+"/register", "POST"))
        {
            Debug.Log("׼����ʼ����");
            RegisterRequest regdata=new RegisterRequest(){
                account=account,
                password=password,
            };
            string _jsonText = JsonUtility.ToJson(regdata);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            Debug.Log("���л�");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            string _msg = request.downloadHandler.text;
                Debug.Log(_msg);
                RegisterResponse _textback = JsonUtility.FromJson<RegisterResponse>(_msg);
            if (_textback.is_success) {
                Debug.Log("ע��ɹ�");
            }
            else {
                Debug.Log("ע��ʧ��");
            }

        }
    }
    public void RegisterPanel() {
        register_panel.SetActive(true);
        login_panel.SetActive(false);
    }
    public void LoginPanel() {
        register_panel.SetActive(false);
        register_success_panel.SetActive(false);
        login_panel.SetActive(true);
    }


    public bool CheckLoginAccount() {
        if (login_account_input_field.text == "") {
            login_account_tip_text.text = "����������";
            return false;
        }
        login_account_tip_text.text = "";
        return true;
    }
    public void EndEditLoginAccount() {
        CheckLoginAccount();
    }
    public bool CheckLoginPassword() {
        if (login_password_input_field.text == "") {
            login_password_tip_text.text = "�������˺�";
            return false;
        }
        login_password_tip_text.text = "";
        return true;
    }
    public void EndEditLoginPassword() {
        CheckLoginPassword();
    }
    public bool CheckRegisterAccount() {
        if (register_account_input_field.text == "") {
            register_account_tip_text.text = "请输入账�??";
            return false;
        }
        register_account_tip_text.text = "";
        return true;
    }
    public void EndEditRegisterAccount() {
        CheckRegisterAccount();
    }
    public bool CheckRegisterPassword() {
        if (register_password_input_field.text == "") {
            register_password_tip_text.text = "请输入密�??";
            return false;
        }
        if (register_password_input_field.text.Length < 6) {
            register_password_tip_text.text = "���벻��С����λ";
            return false;
        }
        register_password_tip_text.text = "";
        return true;
    }
    public void EndEditRegisterPassword() {
        if (CheckRegisterPassword()) {
            register_confirm_password_input_field.text = "";
        }
    }
    public bool CheckRegisterConfirmPassword() {
        if (register_confirm_password_input_field.text == "") {
            register_confirm_password_tip_text.text = "请重复你的密�??";
            return false;
        }
        if (register_confirm_password_input_field.text != register_password_input_field.text) {
            register_confirm_password_tip_text.text = "请确保两次密码输入一�??";
            return false;
        }
        register_confirm_password_tip_text.text = "";
        return true;
    }
    public void EndEditRegisterConfirmPassword() {
        CheckRegisterConfirmPassword();
    }
}

public enum PostType{
    login,
    regist,
}
[Serializable]
public class RegisterRequest{
   public string account;
    public string password;
}
[Serializable]
public class RegisterResponse{
    public bool is_success;
    public string reason;
    public string account;
    public string password;
}
[Serializable]
public class LoginRequest{
   public string account;
    public string password;
}
[Serializable]
public class LoginResponse{
   public bool is_success;
   public string reason;
   public string user_id;
   public string account;
   public string password;
}