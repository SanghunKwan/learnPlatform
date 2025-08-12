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
        Debug.Log(AccessToken.CurrentAccessToken);
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

            Debug.Log(acT.UserId);

            _uiLogInMain.SettingLogIn();
        }
        else
        {
            if (result.Error != null)
                Debug.LogFormat("���̽��� �α��� ���� : Error Code [{0}]", result.Error);
            else
                Debug.Log("���̽��� �α��� ����(User Canceled)");
        }
    }
}
