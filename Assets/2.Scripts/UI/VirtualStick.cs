using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    Image _bg;
    Image _stick;

    Vector3 _inputVector;



    public float _horizontalValue => _inputVector.x;
    public float _verticalValue => _inputVector.y;
    public float _horizontalRawValue => _inputVector.x < 0 ? -1 : Mathf.Ceil(_inputVector.x);
    public float _verticalRawValue => _inputVector.y < 0 ? -1 : Mathf.Ceil(_inputVector.y);


    private void Awake()
    {
        _bg = transform.GetChild(0).GetComponent<Image>();
        _stick = _bg.transform.GetChild(0).GetComponent<Image>();


#if UNITY_STANDALONE_WIN
        gameObject.SetActive(false);

#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _bg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _bg.rectTransform.sizeDelta.y);


            _inputVector = new Vector3(pos.x, pos.y, 0);
            _inputVector = (_inputVector.sqrMagnitude > 1) ? _inputVector.normalized : _inputVector;

            _stick.rectTransform.anchoredPosition = new Vector3(_inputVector.x * (_bg.rectTransform.sizeDelta.x / 2), _inputVector.y * (_bg.rectTransform.sizeDelta.y / 2));

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector3.zero;
        _stick.rectTransform.anchoredPosition = Vector3.zero;
    }
}
