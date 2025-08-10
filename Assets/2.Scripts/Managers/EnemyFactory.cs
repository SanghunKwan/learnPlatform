using System.Collections.Generic;
using UnityEngine;
using DefineSDK;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] GameObject _prefabSpawnEnemy;
    EnemyCharacter[] _spawnedEnemies;

    [SerializeField] int _maxCount;
    [SerializeField] int _maxDead;

    int _nextSpawnIndex;

    [SerializeField] float _arriveDelay = 2;
    [SerializeField] float _spawnDelay = 5;


    float _timer;

    Queue<int> _emptyIndex;
    Transform[] _patrolPoints;
    MainCharacter _player;

    private void Awake()
    {
        if (_patrolPoints == null)
        {
            _patrolPoints = GetComponentsInChildren<Transform>();
        }
        _timer = 0;

        _spawnedEnemies = new EnemyCharacter[_maxCount];
        _emptyIndex = new Queue<int>(_maxCount);
        _nextSpawnIndex = Random.Range(1, _patrolPoints.Length);
        for (int i = 0; i < _maxCount; i++)
        {
            _emptyIndex.Enqueue(i);
        }
    }


    private void Update()
    {
        if (_maxDead <= 0)
        {
            enabled = false;
            IngameManager._instance.OnSpawnFinishFactory();
            return;
        }

        if (_emptyIndex.Count > 0)
        {
            _timer += Time.deltaTime;
        }
        else
            _timer = 0;

        if (_timer > _spawnDelay)
        {
            _timer = 0;

            GameObject spawnedObj = Instantiate(_prefabSpawnEnemy, _patrolPoints[_nextSpawnIndex].position, Quaternion.identity);
            EnemyCharacter spawnedEnemy = spawnedObj.GetComponent<EnemyCharacter>();

            int emptyIndex = _emptyIndex.Dequeue();
            _spawnedEnemies[emptyIndex] = spawnedEnemy;
            spawnedEnemy.InitMonster(this, emptyIndex);

            if (_player == null)
                OnArrive(emptyIndex, _nextSpawnIndex);
            else
                spawnedEnemy.DetectTarget(_player);

            _nextSpawnIndex = Random.Range(1, _patrolPoints.Length);
        }

    }


    private void OnDrawGizmos()
    {
        if (_patrolPoints == null)
        {
            _patrolPoints = GetComponentsInChildren<Transform>();
        }

        int length = _patrolPoints.Length;
        Gizmos.color = Color.yellow;
        for (int i = 1; i < length; i++)
        {
            int lastPointIndex = (i - 1 != 0) ? i - 1 : length - 1;

            Gizmos.DrawLine(_patrolPoints[lastPointIndex].position, _patrolPoints[i].position);
        }

    }


    public void OnArrive(int monsterIndex, int arrivePointIndex)
    {
        if (arrivePointIndex == _nextSpawnIndex)
            _timer = 0;

        StartCoroutine(DefineSDKUtils.DelayAction(_arriveDelay, () => GoNextPoint(_spawnedEnemies[monsterIndex], arrivePointIndex)));
    }
    void GoNextPoint(EnemyCharacter waitingEnemy, int arrivePointIndex)
    {
        if (waitingEnemy == null) return;

        if (arrivePointIndex == _nextSpawnIndex)
            _timer = 0;

        int nextPointIndex = (arrivePointIndex + 1 < _patrolPoints.Length) ? arrivePointIndex + 1 : 1;
        waitingEnemy.Walk(_patrolPoints[nextPointIndex].position, nextPointIndex);
    }


    public void EnemyDown(int enemyIndex)
    {
        IngameManager._instance.AddCorpse(_spawnedEnemies[enemyIndex]);
        _spawnedEnemies[enemyIndex] = null;

        _emptyIndex.Enqueue(enemyIndex);
        _maxDead--;
        if ((_maxDead <= 0) && (_emptyIndex.Count == _spawnedEnemies.Length))
        {
            IngameManager._instance.OnClearFactory();
        }
    }

    public void DetectPlayer(MainCharacter player)
    {
        if (_player != null) return;
        _player = player;

        foreach (var item in _spawnedEnemies)
        {
            if (item == null) continue;

            item.DetectTarget(player);
        }

    }
}
