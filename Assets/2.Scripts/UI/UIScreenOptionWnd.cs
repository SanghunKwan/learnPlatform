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

    List<int> _resolution2Available;

    public static int _selectResolutionIndex { get; set; }

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
        int selectedIndex = 0;
        foreach (var item in _optionDic)
        {
            _resolution2Available.Add(item.Value);
            optionList.Add(item.Key);
            if (item.Value == _selectResolutionIndex)
            {
                selectedIndex = optionList.Count - 1;
            }
        }
        _resolutionList.AddOptions(optionList);
        _resolutionList.SetValueWithoutNotify(selectedIndex);
    }
    string ResolutionDic(Resolution resolution)
    {
        //가로 방향 모니터일 때
        //사용 가능한 세로 길이 기준으로 가로 길이 재연산.
        if (resolution.width > resolution.height)
            return GetHorizontalWidth(resolution) + "X" + resolution.height;
        else
            return resolution.width + "X" + resolution.height;
    }
    public static bool IsRateSameWithUI(Resolution resolution)
    {
        //가로의 경우 비율을 연산하기 때문에 모두 true.
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
        //해상도 적용.
        _currResolution = _availableResolutions[_resolution2Available[_resolutionList.value]];
        _selectResolutionIndex = _resolution2Available[_resolutionList.value];

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
