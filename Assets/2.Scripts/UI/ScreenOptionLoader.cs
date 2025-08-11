using UnityEngine;

public class ScreenOptionLoader : MonoBehaviour
{
    [SerializeField] Vector2Int defaultScreenSize = new Vector2Int(540, 960);

    Resolution[] _availableResolutions;


    private void Awake()
    {
        _availableResolutions = Screen.resolutions;

        //해상도 배열에서 동일한 것을 찾음 => height 기준.
        if (IsSameResolutionExist(Screen.width, Screen.height, out int similarResolutionIndex))
        {
            //동일한 것 있음.
            //9:16
            //window에 표시만 바꾸면 됨.
        }
        else
        {
            //동일한 것 없음.
            //default 혹은 default와 가장 가까운 것으로 screenSize 설정.
            IsSameResolutionExist(defaultScreenSize.x, defaultScreenSize.y, out similarResolutionIndex);
            Resolution mostSimilarOne = _availableResolutions[similarResolutionIndex];
            Screen.SetResolution(UIScreenOptionWnd.GetHorizontalWidth(mostSimilarOne), mostSimilarOne.height, false);
        }
        UIScreenOptionWnd._selectResolutionIndex = similarResolutionIndex;
    }

    bool IsSameResolutionExist(int width, int height, out int similarResolutionIndex)
    {
        int similarResolutionHeight = int.MaxValue;
        similarResolutionIndex = 0;
        //동일한 것 찾기

        for (int i = 0; i < _availableResolutions.Length; i++)
        {
            Resolution tempSolution = _availableResolutions[i];

            if (tempSolution.height != height)
            {
                if (UIScreenOptionWnd.IsRateSameWithUI(tempSolution)
                    && Mathf.Abs(similarResolutionHeight - height) > Mathf.Abs(tempSolution.height - height))
                {
                    similarResolutionHeight = tempSolution.height;
                    similarResolutionIndex = i;
                }
                continue;
            }

            if (Screen.mainWindowDisplayInfo.width > Screen.mainWindowDisplayInfo.height
                && UIScreenOptionWnd.GetHorizontalWidth(tempSolution) == width)
            {
                similarResolutionIndex = i;
                return true;
            }
        }

        return false;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 200, 200, 100), Screen.width + ":" + Screen.height);
    }

}
