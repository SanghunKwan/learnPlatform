using DefineSDK;
using DefineStruct;
using System;
using System.Collections.Generic;
using System.IO;
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
    PlayerInfoObject _playerInfo;

    public MainCharacter _Player => _player;



    private void Awake()
    {
        _instance = this;

    }

    public void Start()
    {
        GameObject infoObject = GameObject.FindGameObjectWithTag("PlayerInfoObject");
        _playerInfo = infoObject.GetComponent<PlayerInfoObject>();
        _mainUIScreen.InitUI(_playerInfo);
        _killCount = _playerInfo._Info._records;

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
            //���� Ŭ����.
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
        //mainUi�� ���� factory �� ǥ�� ����.
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
        //���� ��ü ����.
        //�÷��̾� ��ġ �̵�.
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
            //���� ����������.
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

    public void QuitGame()
    {
        PlayerRecordsPair newRecord = new PlayerRecordsPair(_playerInfo._Info._id, _killCount);

        if (_playerInfo._playerRecordList.HasKey(_playerInfo._Info._id, out int index))
        {
            _playerInfo._playerRecordList._list[index] = newRecord;
        }
        else
        {
            _playerInfo._playerRecordList._list.Add(newRecord);
        }

        string jsonString = JsonUtility.ToJson(_playerInfo._playerRecordList);


        using (StreamWriter sw = new StreamWriter(LoginWnd.recordPath))
        {
            sw.Write(jsonString);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
