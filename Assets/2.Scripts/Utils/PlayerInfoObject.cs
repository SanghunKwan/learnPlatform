using UnityEngine;
using DefineStruct;

public class PlayerInfoObject : MonoBehaviour
{
    PlayerInfo info;
    public PlayerInfo _Info => info;

    public void SetRecord(int record)
    {
        info._records = record;
        Debug.Log(record);
    }
    public void SetName(in string nameStr)
    {
        info._name = nameStr;
    }
    public void SetImage(in Texture2D texture)
    {
        info._image = texture;
    }
}
