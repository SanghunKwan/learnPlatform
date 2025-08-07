using UnityEngine;

public class ScreenOptionLoader : MonoBehaviour
{
    [SerializeField] Vector2Int defaultScreenSize = new Vector2Int(540, 960);

    Resolution[] _availableResolutions;


    private void Awake()
    {
        _availableResolutions = Screen.resolutions;

        //�ػ� �迭���� ������ ���� ã�� => height ����.
        if (IsSameResolutionExist(Screen.width, Screen.height, out Resolution similarResolution))
        {
            //������ �� ����.
            //9:16
            //window�� ǥ�ø� �ٲٸ� ��.
        }
        else
        {
            //������ �� ����.
            //default Ȥ�� default�� ���� ����� ������ screenSize ����.
            IsSameResolutionExist(defaultScreenSize.x, defaultScreenSize.y, out similarResolution);
            Screen.SetResolution(UIScreenOptionWnd.GetHorizontalWidth(similarResolution), similarResolution.height, false);
        }



        //�⺻�� ���� => 540 : 960
        //�ش� �� �������� ������ ���� ������ �� ���.

        //����� �� ���.

    }

    bool IsSameResolutionExist(int width, int height, out Resolution similarResolution)
    {
        similarResolution = new Resolution { height = int.MaxValue };

        //������ �� ã��

        

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
