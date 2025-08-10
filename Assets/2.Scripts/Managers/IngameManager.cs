using DefineSDK;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager _instance { get; private set; }


    [SerializeField] Vector3[] _playerPosition;
    int _currentStageIndex;
    int _factoryCount;
    int _currentClearedFactoryCount;
    int _printClearCount;
    int _killCount;
    int _accumulatedDamages;

    float _playTime;
    float _stageStartTime;


    [SerializeField] MainCharacter _player;
    [SerializeField] MainUIScreen _mainUIScreen;


    [SerializeField] GameObject[] _prefabEnemyFactories;
    [SerializeField] GameObject _prefabClearWnd;

    GameObject _factoriesParentObject;

    List<EnemyCharacter> _corpseList;

    public MainCharacter _Player => _player;



    private void Awake()
    {
        _instance = this;

    }

    public void Start()
    {
        _mainUIScreen.InitUI();
        _mainUIScreen.ActivateFadeEffect(false);
        _player.InitCharacter();
        _currentStageIndex = -1;

        _corpseList = new List<EnemyCharacter>();
    }

    void SetStage(int stageIndex)
    {
        _currentStageIndex = stageIndex;

        _player.transform.position = _playerPosition[stageIndex];
        _mainUIScreen.SetMinimapPosition(_playerPosition[stageIndex] + Vector3.up * 50);

        _factoriesParentObject = Instantiate(_prefabEnemyFactories[_currentStageIndex]);
        _factoryCount = _factoriesParentObject.transform.childCount;
        _currentClearedFactoryCount = 0;
    }

    void ReleaseStage()
    {
        Destroy(_factoriesParentObject);
        _playTime += Time.time - _stageStartTime;

        if (_currentStageIndex == _prefabEnemyFactories.Length - 1)
        {
            //게임 클리어.
            _mainUIScreen.ShowClear();

            StartCoroutine(DefineSDKUtils.DelayAction(3, () =>
            {
                GameObject screenObj = Instantiate(_prefabClearWnd, _mainUIScreen.transform);
                UIClearWindow clearWnd = screenObj.GetComponent<UIClearWindow>();

                clearWnd.InitWnd();
                clearWnd.OpenWnd();

                clearWnd.SetDetails(_playTime, _killCount, _accumulatedDamages);
            }));
        }
        else
            StartCoroutine(DefineSDKUtils.DelayAction(3, () => _mainUIScreen.ActivateFadeEffect(true)));
    }

    public void OnSpawnFinishFactory()
    {
        //mainUi에 남은 factory 수 표기 변경.
        _printClearCount++;
        int leftFactoriesCount = _factoryCount - _printClearCount;

        if (leftFactoriesCount > 0)
        {
            _mainUIScreen.RenewProgress(leftFactoriesCount);
        }
        else
        {
            _mainUIScreen.ShowEliminate();
        }

    }
    public void OnStartStage()
    {
        _stageStartTime = Time.time;
        //몬스터 시체 삭제.
        //플레이어 위치 이동.
        ClearCorpse();

        SetStage(_currentStageIndex + 1);
        _mainUIScreen.RenewProgress(_factoryCount);
        _printClearCount = 0;
    }

    public void OnClearFactory()
    {
        _currentClearedFactoryCount++;


        if (_currentClearedFactoryCount == _factoryCount)
        {
            _currentClearedFactoryCount = 0;
            //다음 스테이지로.
            ReleaseStage();
        }
    }
    public void AddCorpse(EnemyCharacter deadEnemy)
    {
        _corpseList.Add(deadEnemy);
        _killCount++;
    }
    void ClearCorpse()
    {
        for (int i = 0; i < _corpseList.Count; i++)
        {
            Destroy(_corpseList[i].gameObject);
        }
        _corpseList.Clear();
    }
    public void AccumDamages(int damage)
    {
        _accumulatedDamages += damage;
    }
}
