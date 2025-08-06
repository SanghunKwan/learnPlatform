using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("미니맵 설정")]
    [SerializeField] float _minFOVValue = 20;
    [SerializeField] float _maxFOVValue = 80;
    [SerializeField] float _zoomChangeValue = 10;

    [SerializeField] float _minSizeValue = 4;
    [SerializeField] float _maxSizeValue = 30;
    [SerializeField] float _zoomSizeVolume = 2;

    [SerializeField] bool _is3D = true;

    bool _isActive;

    Camera _minimapCam;

    //임시 ==
    private void Start()
    {
        InitMinimapBox();
    }
    //==


    private void Update()
    {
#if UNITY_ANDROID
            ZoomCamMobile();
#else
        if (_isActive)
        {
            ZoomCamWin();
        }
#endif
    }


    public void InitMinimapBox()
    {
        _minimapCam = GameObject.FindGameObjectWithTag("MiniMapCamera").GetComponent<Camera>();
    }

    void ZoomCamWin()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel");
        if (_is3D)
        {
            distance *= -1 * _zoomChangeValue;

            if (distance != 0)
            {
                _minimapCam.fieldOfView =
                Mathf.Clamp(_minimapCam.fieldOfView + distance, _minFOVValue, _maxFOVValue);
            }
        }
        else
        {
            distance *= -1 * _zoomSizeVolume;

            if (distance != 0)
            {
                _minimapCam.orthographicSize =
                Mathf.Clamp(_minimapCam.orthographicSize + distance, _minFOVValue, _maxFOVValue);
            }
        }

    }
    void ZoomCamMobile()
    {
        if (Input.touchCount > 1)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            //이전 프레임 위치
            Vector2 touchFirstPrev = firstTouch.position - firstTouch.deltaPosition;
            Vector2 touchSecondPrev = secondTouch.position - secondTouch.deltaPosition;

            //이전과 현재의 거리를 각각 구함.
            float prevTouchDeltaMagnitude = (touchFirstPrev - touchSecondPrev).magnitude;
            float currTouchDeltaMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            //이전과 현재의 거리 차이. 양수이면 서로 가까워짐.
            float deltaMagnitudeDiff = prevTouchDeltaMagnitude - currTouchDeltaMagnitude;

            if (_is3D)
            {
                _minimapCam.fieldOfView =
                    Mathf.Clamp(_minimapCam.fieldOfView + deltaMagnitudeDiff * _zoomChangeValue, _minFOVValue, _maxFOVValue);
            }
            else
            {
                _minimapCam.orthographicSize =
                    Mathf.Clamp(_minimapCam.orthographicSize + deltaMagnitudeDiff * _zoomSizeVolume, _minSizeValue, _maxSizeValue);
            }
        }

    }

    void ZoomIn2D()
    {
        _minimapCam.orthographicSize =
            Mathf.Clamp(_minimapCam.orthographicSize - _zoomSizeVolume, _minSizeValue, _maxSizeValue);
    }
    void ZoomOut2D()
    {
        _minimapCam.orthographicSize =
            Mathf.Clamp(_minimapCam.orthographicSize + _zoomSizeVolume, _minSizeValue, _maxSizeValue);
    }
    void ZoomIn3D()
    {
        _minimapCam.fieldOfView =
            Mathf.Clamp(_minimapCam.fieldOfView - _zoomChangeValue, _minFOVValue, _maxFOVValue);
    }
    void ZoomOut3D()
    {
        _minimapCam.fieldOfView =
            Mathf.Clamp(_minimapCam.fieldOfView + _zoomChangeValue, _minFOVValue, _maxFOVValue);
    }
    public void ClickZoomInButton()
    {
        if (_is3D)
            ZoomIn3D();
        else
            ZoomIn2D();
    }
    public void ClickZoomOutButton()
    {
        if (_is3D)
            ZoomOut3D();
        else
            ZoomOut2D();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isActive = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _isActive = false;
    }


}
