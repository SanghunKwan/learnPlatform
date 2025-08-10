using UnityEngine;

public class UIHitEffect : MonoBehaviour
{
    [SerializeField] Animator _anim;

    static readonly int triggerHash = Animator.StringToHash("FadeOut");

    public void ActivateEffect()
    {
        _anim.SetTrigger(triggerHash);
    }

}
