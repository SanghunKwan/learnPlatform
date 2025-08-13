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



    //페이스북 초기화 함수
    public void InitFacebook(LoginWnd loginWnd)
    {
        _uiLogInMain = loginWnd;
        if (!FB.IsInitialized)
        {
            Debug.Log("페이북 SDK 초기화 시작");
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            Debug.Log("페이스북 SDK 초기화 완료");
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

    //페이스북 초기화 실행 후 결과시 호출 함수
    void InitCallBack()
    {

        if (FB.IsInitialized)
        {
            Debug.Log("페이스북 SDK 초기화 완료");
            FB.ActivateApp();
            _uiLogInMain.SettingInit();
        }
        else
            Debug.Log("페이스북 SDK 초기화 실패");

    }
    //페이스북 초기화 실행전, 후에 호출되는 함수로 isGameShow가 false, true로 호출.
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
            Debug.Log("페이스북 로그인 성공");
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
                Debug.LogFormat("페이스북 로그인 실패 : Error Code [{0}]", result.Error);
            else
                Debug.Log("페이스북 로그인 실패(User Canceled)");
        }
    }

    void GetUserInfoCallBack(IResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("유저 정보 읽기 실패");
            return;
        }

        Dictionary<string, object> userInfo
            = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

        if (userInfo == null)
            Debug.LogError("유저 정보의 파싱에 실패했습니다. Reason:" + result.RawResult);

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
    //        Debug.Log("유저 정보 읽기 실패");
    //        return;
    //    }

    //    Dictionary<string, object> userInfo
    //        = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

    //    if (userInfo == null)
    //        Debug.LogError("유저 정보의 파싱에 실패했습니다. Reason:" + result.RawResult);

    //    if (userInfo.ContainsKey("picture_small"))
    //    {
    //        Dictionary<string, object> dataDic = userInfo["picture_small"] as Dictionary<string, object>;
    //        Dictionary<string, object> lastDic = dataDic["data"] as Dictionary<string, object>;

    //        string url = lastDic["url"] as string;

    //        Debug.Log(url);

    //        StartCoroutine(WaitTexture(url));
    //    }
    //    else
    //        Debug.Log("이거 아님");
    //}

    void GetUserInfoCallBack2(IGraphResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("유저 정보 읽기 실패");
            return;
        }
        Texture2D texture = result.Texture;
        _uiLogInMain.SetPlayerIcon(texture);
        _playerInfoObject.SetImage(texture);
        _uiLogInMain.ReadytoStart();

        //Dictionary<string, object> userInfo
        //    = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;

        //if (userInfo == null)
        //    Debug.LogError("유저 정보의 파싱에 실패했습니다. Reason:" + result.RawResult);

        //if (userInfo.ContainsKey("picture_small"))
        //{
        //    Dictionary<string, object> dataDic = userInfo["picture_small"] as Dictionary<string, object>;
        //    Dictionary<string, object> lastDic = dataDic["data"] as Dictionary<string, object>;

        //    string url = lastDic["url"] as string;

        //    Debug.Log(url);

        //    StartCoroutine(WaitTexture(url));
        //}
        //else
        //    Debug.Log("이거 아님");
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
