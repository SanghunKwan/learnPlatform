using DefineStruct;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginWnd : MonoBehaviour
{
    readonly string recordPath = Path.Combine(Application.streamingAssetsPath, "asdf.json");

    [SerializeField] Button _btnInit;
    [SerializeField] Button _btnLogin;
    [SerializeField] Button _btnLogout;
    [SerializeField] Button _btnGameStart;

    [SerializeField] RawImage _myPic;
    [SerializeField] TextMeshProUGUI _textName;
    [SerializeField] TextMeshProUGUI _textUserID;

    GameObject _boxPlayerInfo;

    PlayerRecordsList _recordsList;



    private void Awake()
    {
        _boxPlayerInfo = _myPic.transform.parent.gameObject;

        _btnInit.gameObject.SetActive(true);
        _btnLogin.gameObject.SetActive(false);
        _btnLogout.gameObject.SetActive(false);
        _btnGameStart.gameObject.SetActive(false);

        _boxPlayerInfo.SetActive(false);


        FileStream st = new FileStream(recordPath, FileMode.OpenOrCreate);
        using (StreamReader sr = new StreamReader(st))
        {
            string tempJsonRead = sr.ReadToEnd();
            _recordsList = JsonUtility.FromJson<PlayerRecordsList>(tempJsonRead);
        }
        if (_recordsList == null)
            _recordsList = new PlayerRecordsList();
        _recordsList.Call();
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
        _btnLogout.gameObject.SetActive(true);

        _boxPlayerInfo.SetActive(true);
    }

    public void SetPlayerIcon(Texture2D icon)
    {
        _myPic.texture = icon;
    }
    public void SetUserID(string id)
    {
        _textUserID.text = id;
    }
    public void SetName(string name)
    {
        _textName.text = name;
    }
    public void ReadytoStart()
    {
        _btnGameStart.gameObject.SetActive(true);
    }

    public int GetRecord(in string id)
    {
        if (_recordsList.HasKey(id, out int index))
            return _recordsList._list[index]._records;

        return 0;
    }

    public void OnClickFacebookInitBtn()
    {
        FacebookManager._Instance.InitFacebook(this);
    }

    public void OnClickFacebookLogin()
    {
        FacebookManager._Instance.FacebookLogin();
    }

    public void OnClickFacebookLogout()
    {
        FacebookManager._Instance.FacebookLogOut();
    }

    public void OnClickGameStart()
    {
        SceneManager.LoadScene(1);
    }
}
