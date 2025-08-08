using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] GameObject _prefabSpawnEnemy;
    EnemyCharacter[] _spawnedEnemies;

    [SerializeField] int _maxCount;
    [SerializeField] int _maxDead;
    [SerializeField] float _arriveDelay = 2;
    [SerializeField] float _spawnDelay = 5;


    float _timer;

    Queue<int> _emptyIndex;
    Transform[] _patrolPoints;


    private void Awake()
    {
        if (_patrolPoints == null)
        {
            _patrolPoints = GetComponentsInChildren<Transform>();
        }
        _timer = 0;

        _spawnedEnemies = new EnemyCharacter[_maxCount];
        _emptyIndex = new Queue<int>(_maxCount);
        for (int i = 0; i < _maxCount; i++)
        {
            _emptyIndex.Enqueue(i);
        }

    }

    private void Update()
    {
        if (_maxDead <= 0) return;

        if (_emptyIndex.Count > 0)
        {
            _timer += Time.deltaTime;
        }
        else
            _timer = 0;

        if (_timer > _spawnDelay)
        {
            _timer = 0;

            GameObject spawnedObj = Instantiate(_prefabSpawnEnemy, _patrolPoints[1].position, Quaternion.identity);
            EnemyCharacter spawnedEnemy = spawnedObj.GetComponent<EnemyCharacter>();

            int emptyIndex = _emptyIndex.Dequeue();
            _spawnedEnemies[emptyIndex] = spawnedEnemy;
            spawnedEnemy.InitMonster(this, emptyIndex);
            OnArrive(emptyIndex, 1);
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
        if (arrivePointIndex == 1)
            _timer = 0;

        StartCoroutine(DelayAction(_arriveDelay, () => GoNextPoint(_spawnedEnemies[monsterIndex], arrivePointIndex)));
    }
    void GoNextPoint(EnemyCharacter waitingEnemy, int arrivePointIndex)
    {
        if (waitingEnemy == null) return;

        if (arrivePointIndex == 1)
            _timer = 0;

        int nextPointIndex = (arrivePointIndex + 1 < _patrolPoints.Length) ? arrivePointIndex + 1 : 1;
        waitingEnemy.Walk(_patrolPoints[nextPointIndex].position, nextPointIndex);
    }
    IEnumerator DelayAction(float delaySecond, Action delayedAction)
    {
        yield return new WaitForSeconds(delaySecond);
        delayedAction();
    }

    public void EnemyDown(int enemyIndex)
    {
        _spawnedEnemies[enemyIndex] = null;

        _emptyIndex.Enqueue(enemyIndex);
        _maxDead--;
    }
}
