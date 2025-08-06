using UnityEngine;

public class ITweenTestComplete : MonoBehaviour
{
    [SerializeField] GameObject _sphere1;
    [SerializeField] GameObject _sphere2;
    [SerializeField] GameObject _sphere3;


    void Start()
    {
        SphereMoveFrom();

    }

    void SphereMoveTo()
    {
        iTween.MoveBy(_sphere3,
                      iTween.Hash("x", 0f, "y", 0.5f, "z", -3.12f, "time", 5f
                                  , "easetype", iTween.EaseType.linear));
    }
    void SphereMoveFrom()
    {
        iTween.MoveFrom(_sphere1,
                        iTween.Hash("z", -20f, "x", -8.5f, "time", 7.0f, "oncomplete", "SphereMoveBy"
                                    , "oncompletetarget", gameObject));
    }
    void SphereMoveBy()
    {
        iTween.MoveBy(_sphere2,
                      iTween.Hash("x", -1f, "y", 8f, "z", 12.6f, "time", 3.8f
                                  , "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "SphereMoveTo"
                                  , "oncompletetarget", gameObject));
    }

}
