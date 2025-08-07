using UnityEngine;

public class ScreenOptionLoader : MonoBehaviour
{
    [SerializeField] Vector2Int defaultScreenSize = new Vector2Int(540, 960);

    Resolution[] _availableResolutions;


    private void Awake()
    {
        _availableResolutions = Screen.resolutions;

        //해상도 배열에서 동일한 것을 찾음 => height 기준.
        if (IsSameResolutionExist(Screen.width, Screen.height, out Resolution similarResolution))
        {
            //동일한 것 있음.
            //9:16
            //window에 표시만 바꾸면 됨.
        }
        else
        {
            //동일한 것 없음.
            //default 혹은 default와 가장 가까운 것으로 screenSize 설정.
            IsSameResolutionExist(defaultScreenSize.x, defaultScreenSize.y, out similarResolution);
            Screen.SetResolution(UIScreenOptionWnd.GetHorizontalWidth(similarResolution), similarResolution.height, false);
        }



        //기본값 기준 => 540 : 960
        //해당 값 존재하지 않으면 가장 유사한 값 사용.

        //사용한 값 기록.

    }

    bool IsSameResolutionExist(int width, int height, out Resolution similarResolution)
    {
        similarResolution = new Resolution { height = int.MaxValue };

        //동일한 것 찾기

        

        for (int i = 0; i < _availableResolutions.Length; i++)
        {
            Resolution tempSolution = _availableResolutions[i];
            if (tempSolution.height != height)
            {
                if (Mathf.Abs(similarResolution.height - height) > Mathf.Abs(tempSolution.height - height))
                {
                    similarResolution = tempSolution;
                }
                continue;
            }

            if (width > height || tempSolution.width == width)
            {
                similarResolution = tempSolution;
                return true;
            }
        }

        return false;
    }



    public void SaveScreenSize()
    {

    }
}
