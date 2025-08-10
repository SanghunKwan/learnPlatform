using UnityEngine;
using DefineSDK;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIClearWindow : MonoBehaviour
{
    [SerializeField] UIResultSlot[] slots;
    Transform buttonsParentTransform;


    public void InitWnd()
    {
        Transform detailsTransform = transform.GetChild(0);
        int buttonsIndex = detailsTransform.childCount - 1;
        buttonsParentTransform = detailsTransform.GetChild(buttonsIndex);
    }

    public void OpenWnd()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            UIResultSlot tempSlot = slots[i];
            tempSlot.InitSlot();
            StartCoroutine(DefineSDKUtils.DelayAction((i * 2) + 1, () => { tempSlot.ActivateSlot(); }));
        }

        StartCoroutine(DefineSDKUtils.DelayAction((slots.Length * 2) + 1, () => buttonsParentTransform.gameObject.SetActive(true)));
    }

    public void SetDetails(float time, int killCount, int damages)
    {

        slots[0].SetDetail(GetTime(time));
        slots[1].SetDetail(killCount.ToString());
        slots[2].SetDetail(damages.ToString());
    }
    string GetTime(float time)
    {
        int integerTime = (int)time;

        return (integerTime / 60).ToString("D2") + ":" + (integerTime % 60).ToString("D2");

    }



    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
