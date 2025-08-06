using UnityEngine;

public class MainUIScreen : MonoBehaviour
{
    [SerializeField] GameObject _prefabWndScreenOption;

    public void ClickOptionButton()
    {
        GameObject go = Instantiate(_prefabWndScreenOption);

        UIScreenOptionWnd wnd = go.GetComponent<UIScreenOptionWnd>();
        wnd.OpenWnd();
    }


}
