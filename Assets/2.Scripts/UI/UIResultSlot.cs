using TMPro;
using UnityEngine;

public class UIResultSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _detailText;

    public void InitSlot()
    {

    }

    public void ActivateSlot()
    {
        gameObject.SetActive(true);
    }

    public void SetDetail(in string str)
    {
        _detailText.text = str;
    }
}