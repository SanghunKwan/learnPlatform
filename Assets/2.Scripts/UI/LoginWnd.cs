using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : MonoBehaviour
{

    [SerializeField] Button _btnInit;
    [SerializeField] Button _btnLogin;

    [SerializeField] RawImage _myPic;

    private void Awake()
    {
        _btnInit.gameObject.SetActive(true);
        _btnLogin.gameObject.SetActive(false);
    }
    public void SettingInit()
    {
        _btnInit.gameObject.SetActive(false);
        _btnLogin.gameObject.SetActive(true);
    }

    public void SettingLogIn()
    {
        _btnInit.gameObject.SetActive(false);
        _btnLogin.gameObject.SetActive(false);
    }

    public void OnClickFacebookInitBtn()
    {
        FacebookManager._Instance.InitFacebook(this);
    }

    public void OnClickFacebookLogin()
    {
        FacebookManager._Instance.FacebookLogin();
    }
}
