using UnityEngine;

public class MoveFromITweenTest : MonoBehaviour
{
    private void Start()
    {
        iTween.MoveFrom(gameObject, iTween.Hash("z", 18.0f, "y", 4.0f, "x", 7.0f, "time", 7.0f, "delay", 5.0f));

    }

}
