using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIScreenOptionWnd : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _resolutionList;

    Camera _uiCam;
    Canvas _myCanvas;

    Resolution[] _availableResolutions;
    Resolution _currResolution;

    int _selectResolutionIndex;

    List<int> _resolution2Available;

    public void OpenWnd()
    {
        _uiCam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        _myCanvas = GetComponent<Canvas>();
        _myCanvas.worldCamera = _uiCam;

        _availableResolutions = Screen.resolutions;
        Dictionary<string, int> _optionDic = new Dictionary<string, int>();

        Resolution tempResolution;
        for (int i = 0; i < _availableResolutions.Length; i++)
        {
            tempResolution = _availableResolutions[i];
            if (!IsRateSameWithUI(tempResolution)) continue;

            string tempKey = ResolutionDic(tempResolution);

            if (!_optionDic.ContainsKey(tempKey))
            {
                _optionDic.Add(tempKey, i);
            }
        }

        _resolution2Available = new List<int>(_optionDic.Count);
        List<string> optionList = new List<string>(_optionDic.Count);

        foreach (var item in _optionDic)
        {
            _resolution2Available.Add(item.Value);
            optionList.Add(item.Key);
        }
        _resolutionList.AddOptions(optionList);
    }
    string ResolutionDic(Resolution resolution)
    {
        //���� ���� ������� ��
        //��� ������ ���� ���� �������� ���� ���� �翬��.
        if (resolution.width > resolution.height)
            return GetHorizontalWidth(resolution) + "X" + resolution.height;
        else
            return resolution.width + "X" + resolution.height;
    }
    bool IsRateSameWithUI(Resolution resolution)
    {
        //������ ��� ������ �����ϱ� ������ ��� true.
        if (resolution.width > resolution.height)
            return true;
        else
            return resolution.width == (resolution.height * 9 / 16);
    }
    public static int GetHorizontalWidth(Resolution resolution)
    {
        return Mathf.CeilToInt(resolution.height * 9 / 16f);
    }

    public void ClickDicisionButton()
    {
        //�ػ� ����.
        _currResolution = _availableResolutions[_resolution2Available[_resolutionList.value]];

        Screen.SetResolution(_currResolution.width > _currResolution.height
                            ? GetHorizontalWidth(_currResolution) : _currResolution.width
                            , _currResolution.height, FullScreenMode.Windowed
                            , _currResolution.refreshRateRatio);

        CloseWindow();
    }
    public void CloseWindow()
    {
        Destroy(gameObject);
    }

    public void ClickNormal()
    {
        Screen.SetResolution(540, 960, false);
    }
    public void ClickDiffent()
    {
        Screen.SetResolution(720, 1280, false);
    }
}
