using DefineSDK;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIShowProgress : FadeEffectBase
{
    Animator _anim;
    TextMeshProUGUI _countText;
    TextMeshProUGUI _titleText;

    IEnumerator _ienum;

    public void InitUI()
    {
        _anim = GetComponent<Animator>();
        Transform countTransform = transform.Find("Count");
        if (countTransform != null)
            _countText = countTransform.GetComponent<TextMeshProUGUI>();

        _titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();

        _anim.SetTrigger(_startTransparentHash);
        _anim.SetBool(_isTransparentHash, true);
    }


    public void ShowDetail()
    {
        _anim.SetBool(_isTransparentHash, false);

        if (_ienum != null)
            StopCoroutine(_ienum);
        _ienum = DefineSDKUtils.DelayAction(3, () => _anim.SetBool(_isTransparentHash, true));
        StartCoroutine(_ienum);
    }
    public void HideDetail()
    {
        if (_ienum != null)
            StopCoroutine(_ienum);
        _ienum = null;
        _anim.SetBool(_isTransparentHash, true);
    }
    public void SetDetailText(in string str)
    {
        _countText.text = str;
    }
    public void SetTitleText(in string str)
    {
        _titleText.text = str;
    }
}
