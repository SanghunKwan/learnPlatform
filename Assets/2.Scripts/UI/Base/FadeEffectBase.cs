using UnityEngine;

public abstract class FadeEffectBase : MonoBehaviour
{
    protected static readonly int _isTransparentHash = Animator.StringToHash("IsTransparent");
    protected static readonly int _startTransparentHash = Animator.StringToHash("StartWithTransparent");

}
