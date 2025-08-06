using UnityEngine;

public class SettingButton : MonoBehaviour
{
    public void OnRotateLeft()
    {
        iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", 1));
        //iTween.RotateBy(gameObject, iTween.Hash("z", 1, "time", 1));
    }

    public void OnRotateRight()
    {
        iTween.RotateTo(gameObject, iTween.Hash("z", 180, "time", 1));
        //iTween.RotateBy(gameObject, iTween.Hash("z", - gameObject.transform.rotation.eulerAngles.z / 360, "time", 1));
    }

}
