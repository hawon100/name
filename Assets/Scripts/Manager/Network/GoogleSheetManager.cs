using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}

public class GoogleSheetManager : MonoBehaviour
{
    #region Variable
    public static GoogleSheetManager instance;
    public GoogleData GD;

    const string URL = "https://script.google.com/macros/s/AKfycbxujHfqvijOx7oX9UuFlI9k7OAmnMXsLpC_hJO5vogLCFuELkawZkE2ybQUcHomjaZL/exec";

    public string Id;
    public string Password;

    public string valueText;

    [SerializeField] private bool isLogin = false;
    [SerializeField] private bool isPost = false;
    #endregion
    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void OnApplicationQuit()
    {
        ApplicationQuit();
    }
    #endregion

    #region Function
    private bool _SetIdPassword(string id, string password)
    {
        if (id.Trim() == "" | password.Trim() == "") return false;
        else return true;
    }

    private bool _CheckIDLenght(string id) 
    {
        char[] _id = id.ToCharArray();
        if (_id.Length > 5 && _id.Length <= 16) return true; // 6자 이상 16자 이하
        else return false;
    }

    private bool _CheckPasswordLength(string password) 
    {
        char[] _password = password.ToCharArray();
        if (_password.Length > 7 && _password.Length <= 20) return true; // 8자 이상 20자 이하
        else return false;
    }

    private void _Register(string id, string password)
    {
        Debug.Log("Register");

        if (!_SetIdPassword(id, password)) { Debug.Log("ID 혹은 패스워드가 비어있습니다."); return; }

        if (!_CheckIDLenght(id)) { LobbyManager.RegisterFailVisible("아이디가 너무 짧습니다.", 2f); return; }

        if (!_CheckPasswordLength(password)) { LobbyManager.RegisterFailVisible("비밀번호가 너무 짧습니다.", 2f); return; }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("password", password);
        form.AddField("id", id);
        Debug.Log(id + " + " + password);

        Post(form);
    }
    public static void Register(string id, string password) => instance._Register(id, password);

    private void _Login(string id, string password)
    {
        if (isPost == false) 
        {
            if (!_SetIdPassword(id, password))
            {
                print("아이디 또는 비밀번호가 비어있습니다");
                return;
            }

            WWWForm form = new WWWForm();
            form.AddField("order", "login");
            form.AddField("id", id);
            form.AddField("pass", password);

            Post(form);
        }
    }
    public static void Login(string id, string password) => instance._Login(id, password);

    private void _Response(string json) 
    {
        if (string.IsNullOrEmpty(json)) return;
        Debug.Log(json);
        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR")
        {
            print(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
            return;
        }

        print(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

        if (GD.msg == "Login Complete") 
        { 
            LobbyManager.LoginComplete();
            isLogin = true;
        }

        if (GD.order == "getValue")
        {
            valueText = GD.value;
        }
    }
    public static void Response(string json) => instance._Response(json);

    private IEnumerator _Post(WWWForm form)
    {
        Debug.Log(form + " (Unity) ");
        isPost = true;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            isPost = false; 
            if (www.isDone) Response(www.downloadHandler.text);
            else Debug.LogError("웹의 응답이 없습니다.");
        }
    }
    public static void Post(WWWForm form) => instance.StartCoroutine(instance._Post(form));

    private void _ApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");
    }
    public static void ApplicationQuit() => instance._ApplicationQuit();

    private void _SetValue(string value)
    {
        WWWForm form = new WWWForm();

        form.AddField("order", "setValue");
        form.AddField("value", value);

        Post(form);
    }
    public static void SetValue(string value) => instance._SetValue(value);

    private void _GetValue()
    {
        WWWForm form = new WWWForm();

        form.AddField("order", "getValue");

        Post(form);
    }
    public static void GetValue() => instance._GetValue();
    #endregion
}
