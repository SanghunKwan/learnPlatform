
using UnityEngine;
using DefineSDK;
using UnityEngine.EventSystems;

public class UIFadeEffect : FadeEffectBase
{
    Animator _anim;
    EventTrigger _eventTrigger;



    public void InitEffect(bool isStartTransparent)
    {
        _anim = GetComponent<Animator>();
        _eventTrigger = GetComponent<EventTrigger>();

        if (isStartTransparent)
        {
            _anim.SetTrigger(_startTransparentHash);
            StartCoroutine(DefineSDKUtils.DelayAction(0.5f, () => _eventTrigger.enabled = true));
        }
        else
        {
            _eventTrigger.enabled = true;
        }
    }



    public void OnPlayerInput()
    {
        _anim.SetBool(_isTransparentHash, true);
        _eventTrigger.enabled = false;
        Destroy(gameObject, 1);
        IngameManager._instance.OnStartStage();
    }


}