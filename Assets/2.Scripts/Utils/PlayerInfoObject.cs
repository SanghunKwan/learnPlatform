using UnityEngine;
using DefineStruct;

public class PlayerInfoObject : MonoBehaviour
{
    PlayerInfo info;
    public PlayerInfo _Info => info;
    public PlayerRecordsList _playerRecordList { get; private set; }

    public void SetRecord(in string id, int record)
    {
        info._records = record;
        info._id = id;
    }
    public void SetName(in string nameStr)
    {
        info._name = nameStr;
    }
    public void SetImage(in Texture2D texture)
    {
        info._image = texture;
    }
    public void SetList(in PlayerRecordsList list)
    {
        _playerRecordList = list;
    }
}
