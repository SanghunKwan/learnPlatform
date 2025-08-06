using UnityEngine;

public class MoveByITweenTest : MonoBehaviour
{
    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", -21.0f, "y", -0.5f, "z", 12.5f, "time", 10.0f, "delay", 6, "easetype", iTween.EaseType.easeOutBounce));
    }

}
