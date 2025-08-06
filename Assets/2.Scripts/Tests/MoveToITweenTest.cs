using UnityEngine;

public class MoveToITweenTest : MonoBehaviour
{
    private void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 10.0f, "z", 15.5f, "time", 5.0f));
    }

}
