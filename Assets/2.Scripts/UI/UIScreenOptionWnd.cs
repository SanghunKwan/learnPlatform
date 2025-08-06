using UnityEngine;

public class UIScreenOptionWnd : MonoBehaviour
{
    Camera _uiCam;
    Canvas _myCanvas;

    public void OpenWnd()
    {
        _uiCam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        _myCanvas = GetComponent<Canvas>();
        _myCanvas.worldCamera = _uiCam;
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
