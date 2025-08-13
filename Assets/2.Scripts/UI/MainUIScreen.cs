using UnityEngine;

public class MainUIScreen : MonoBehaviour
{
    [SerializeField] GameObject _prefabWndScreenOption;
    [SerializeField] GameObject _prefabFadeEffect;

    [SerializeField] UIHitEffect _hitEffect;
    [SerializeField] UIShowProgress _showProgress;
    [SerializeField] UIShowProgress _showEliminate;
    [SerializeField] MiniMapBox _miniMapBox;
    [SerializeField] UIInfoBox _infoBox;
    MainCharacter _player;



    public void InitUI()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>();

        _player.OnHitEvent += ActivateHitEffect;
        _showProgress.InitUI();
        _showEliminate.InitUI();
        _miniMapBox.InitMinimapBox();

        GameObject tempInfo = GameObject.FindGameObjectWithTag("PlayerInfoObject");
        _infoBox.InitBox(tempInfo.GetComponent<PlayerInfoObject>());

        Destroy(tempInfo, 1);

    }


    public void ClickOptionButton()
    {
        GameObject go = Instantiate(_prefabWndScreenOption);

        UIScreenOptionWnd wnd = go.GetComponent<UIScreenOptionWnd>();
        wnd.OpenWnd();
    }

    public void ActivateHitEffect()
    {
        _hitEffect.ActivateEffect();
    }

    public void ActivateFadeEffect(bool isTransparent)
    {
        GameObject effectObj = Instantiate(_prefabFadeEffect, transform);
        UIFadeEffect effect = effectObj.GetComponent<UIFadeEffect>();

        effect.InitEffect(isTransparent);
    }

    public void RenewProgress(int count)
    {
        _showProgress.SetDetailText(count.ToString());
        _showProgress.ShowDetail();
    }
    public void ShowEliminate()
    {
        _showEliminate.ShowDetail();
        _showProgress.HideDetail();
    }
    public void ShowClear()
    {
        _showEliminate.SetTitleText("Clear!!");
        _showEliminate.ShowDetail();
    }

    public void SetMinimapPosition(in Vector3 vec)
    {
        _miniMapBox.SetPosition(vec);
    }
}
