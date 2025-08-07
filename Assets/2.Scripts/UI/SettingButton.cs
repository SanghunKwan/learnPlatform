using UnityEngine;

public class SettingButton : MonoBehaviour
{

    public void OnRotateRight()
    {
        //iTween.RotateTo(gameObject, iTween.Hash("z", 180, "time", 1));
        iTween.RotateBy(gameObject, iTween.Hash("z", 1, "time", 1));
    }

    public void OnRotateLeft()
    {
        //iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", 1));
        iTween.RotateTo(gameObject, iTween.Hash("z", -360, "time", 1));
    }

    

}
