using DefineStruct;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _records;
    [SerializeField] RawImage _imageTexture;


    public void InitBox(PlayerInfoObject playerInfoObject)
    {
        PlayerInfo info = playerInfoObject._Info;

        _nameText.text = info._name;
        _imageTexture.texture = info._image;
        _records.text = info._records.ToString();
    }

}
