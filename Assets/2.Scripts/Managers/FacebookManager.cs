using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
//using UnityEngine.Networking;
//using System.Collections;


public class FacebookManager : MonoBehaviour
{
    public static FacebookManager _Instance { get; private set; }

    LoginWnd _uiLogInMain;
    PlayerInfoObject _playerInfoObject;

    private void Awake()
    {
        _Instance = this;
    }



    //���̽��� �ʱ�ȭ �Լ�
    public void InitFacebook(LoginWnd loginWnd)
    {
        _uiLogInMain = loginWnd;
        if (!FB.IsInitialized)
        {
            Debug.Log("���̺� SDK �ʱ�ȭ ����");
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            Debug.Log("���̽��� SDK �ʱ�ȭ �Ϸ�");
            FB.ActivateApp();
        }
    }
    public void FacebookLogin()
    {
        List<string> param = new List<string>() { "public_profile", "email" };
        FB.LogInWithPublishPermissions(param, AuthCallback);
    }
    public void FacebookLogOut()
    {
        FB.LogOut();
        _uiLogInMain.SettingInit();
    }

    //���̽��� �ʱ�ȭ ���� �� ����� ȣ�� �Լ�
    void InitCallBack()
    {

        if (FB.IsInitialized)
        {
            Debug.Log("���̽��� SDK �ʱ�ȭ �Ϸ�");
            FB.ActivateApp();
            _uiLogInMain.SettingInit();
        }
        else
            Debug.Log("���̽��� SDK �ʱ�ȭ ����");

    }
    //���̽��� �ʱ�ȭ ������, �Ŀ� ȣ��Ǵ� �Լ��� isGameShow�� false, true�� ȣ��.
    void OnHideUnity(bool isGameShow)
    {
        if (isGameShow)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("���̽��� �α��� ����");
            AccessToken acT = AccessToken.CurrentAccessToken;
            _playerInfoObject = new GameObject("playerInfoObject").AddComponent<PlayerInfoObject>();
            _playerInfoObject.tag = "PlayerInfoObject";
            DontDestroyOnLoad(_playerInfoObject);

            Debug.Log(acT.UserId);

            _uiLogInMain.SettingLogIn();

            FB.API("/me?fields=id,name", HttpMethod.GET, GetUserInfoCallBack);
            FB.API(acT.UserId + "/picture", HttpMethod.GET, GetUserInfoCallBack2);
        }
        else
        {
            if (result.Error != null)
                Debug.LogFormat("���̽��� �α��� ���� : Error Code [{0}]", result.Error);
            else
                Debug.Log("���̽��� �α��� ����(User Canceled)");
        }
    }

    void GetUserInfoCallBack(IResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("���� ���� �б� ����");
            return;
        }

        Dictionary<string, object> userInfo
            = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

        if (userInfo == null)
            Debug.LogError("���� ������ �Ľ̿� �����߽��ϴ�. Reason:" + result.RawResult);

        if (userInfo.ContainsKey("name"))
        {
            string tempStr = userInfo["name"].ToString();
            _uiLogInMain.SetName(tempStr);
            _playerInfoObject.SetName(tempStr);
        }
        if (userInfo.ContainsKey("id"))
        {
            string tempStr = userInfo["id"].ToString();
            _uiLogInMain.SetUserID(tempStr);
            _playerInfoObject.SetRecord(_uiLogInMain.GetRecord(tempStr));

        }

    }

    //void GetUserInfoCallBack2(IResult result)
    //{
    //    if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
    //    {
    //        Debug.Log("���� ���� �б� ����");
    //        return;
    //    }

    //    Dictionary<string, object> userInfo
    //        = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

    //    if (userInfo == null)
    //        Debug.LogError("���� ������ �Ľ̿� �����߽��ϴ�. Reason:" + result.RawResult);

    //    if (userInfo.ContainsKey("picture_small"))
    //    {
    //        Dictionary<string, object> dataDic = userInfo["picture_small"] as Dictionary<string, object>;
    //        Dictionary<string, object> lastDic = dataDic["data"] as Dictionary<string, object>;

    //        string url = lastDic["url"] as string;

    //        Debug.Log(url);

    //        StartCoroutine(WaitTexture(url));
    //    }
    //    else
    //        Debug.Log("�̰� �ƴ�");
    //}

    void GetUserInfoCallBack2(IGraphResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("���� ���� �б� ����");
            return;
        }
        Texture2D texture = result.Texture;
        _uiLogInMain.SetPlayerIcon(texture);
        _playerInfoObject.SetImage(texture);
        _uiLogInMain.ReadytoStart();

        //Dictionary<string, object> userInfo
        //    = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

        //if (userInfo == null)
        //    Debug.LogError("���� ������ �Ľ̿� �����߽��ϴ�. Reason:" + result.RawResult);

        //if (userInfo.ContainsKey("picture_small"))
        //{
        //    Dictionary<string, object> dataDic = userInfo["picture_small"] as Dictionary<string, object>;
        //    Dictionary<string, object> lastDic = dataDic["data"] as Dictionary<string, object>;

        //    string url = lastDic["url"] as string;

        //    Debug.Log(url);

        //    StartCoroutine(WaitTexture(url));
        //}
        //else
        //    Debug.Log("�̰� �ƴ�");
    }

    //IEnumerator WaitTexture(string url)
    //{
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
    //    {
    //        yield return uwr.SendWebRequest();

    //        Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
    //        _uiLogInMain.SetPlayerIcon(texture);
    //    }
    //}
}
