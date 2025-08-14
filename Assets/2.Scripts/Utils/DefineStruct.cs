using System;
using System.Collections.Generic;
using UnityEngine;


namespace DefineStruct
{
    public struct PlayerInfo
    {
        public string _name;
        public string _id;
        public int _records;
        public Texture2D _image;
    }
    [Serializable]
    public struct PlayerRecordsPair
    {
        public string _id;
        public int _records;

        public PlayerRecordsPair(in string str, int record)
        {
            _id = str;
            _records = record;
        }
    }
    [Serializable]
    public class PlayerRecordsList
    {
        public List<PlayerRecordsPair> _list = new List<PlayerRecordsPair>();

        public void Call()
        {
            foreach (var item in _list)
            {
                Debug.Log(item._id + ":" + item._records);
            }
        }

        public bool HasKey(in string key, out int index)
        {
            index = -1;
            for (int i = 0; i < _list.Count; i++)
            {
                if (string.Compare(_list[i]._id, key) == 0)
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }
    }
}
