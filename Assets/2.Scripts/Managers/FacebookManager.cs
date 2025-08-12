using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;


public class FacebookManager : MonoBehaviour
{
    public static FacebookManager _Instance { get; private set; }

    LoginWnd _uiLogInMain;


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
        Debug.Log(AccessToken.CurrentAccessToken);
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

            Debug.Log(acT.UserId);

            _uiLogInMain.SettingLogIn();
        }
        else
        {
            if (result.Error != null)
                Debug.LogFormat("페이스북 로그인 실패 : Error Code [{0}]", result.Error);
            else
                Debug.Log("페이스북 로그인 실패(User Canceled)");
        }
    }
}
