using UnityEngine;
using DefineEnums;
using UnityEngine.AI;

public class EnemyCharacter : MonoBehaviour
{
    const float _toleranceDistance = 0.1f;

    [Header("½ºÅÈ")]
    [SerializeField] int _maxHp;
    int _currentHp;
    [SerializeField] int _damage;

    static readonly int _aniStateHash = Animator.StringToHash("AniState");

    int _destinationIndex;
    int _factoryIndex;

    [SerializeField] float _walkSpeed = 2;
    [SerializeField] float _runSpeed = 4;
    [SerializeField] float _attackRange = 1;

    float _attackTimer;

    AniState _aniState;
    ActState _actState;


    Animator _aniController;
    NavMeshAgent _agent;

    MainCharacter _player;
    SphereCollider _attackZoneCollider;


    EnemyFactory _parentFactory;


    private void Update()
    {
        if (_attackTimer > 0)
            _attackTimer -= Time.deltaTime;

        switch (_actState)
        {
            case ActState.None:

                break;
            case ActState.Patrol:
                if (!_agent.hasPath) return;

                if (_agent.remainingDistance < _toleranceDistance)
                {
                    _parentFactory.OnArrive(_factoryIndex, _destinationIndex);
                    _agent.isStopped = true;
                    Idle();
                }
                break;
            case ActState.Follow:
                if (_attackTimer > 0 || _player == null) return;

                if (Vector3.Distance(transform.position, _player.transform.position) <= _toleranceDistance + _attackRange)
                {
                    Attack();
                }
                else
                {
                    Run();
                }
                break;
        }
    }

    public void InitMonster(EnemyFactory factory, int factoryIndex)
    {
        _parentFactory = factory;

        _factoryIndex = factoryIndex;

        _currentHp = _maxHp;
        _aniState = AniState.Idle;
        _actState = ActState.None;

        _aniController = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _attackZoneCollider = transform.Find("AttackZone").GetComponent<SphereCollider>();

        _attackZoneCollider.center = new Vector3(_attackZoneCollider.center.x, _attackZoneCollider.radius, _attackRange - _attackZoneCollider.radius + 0.3f);
    }

    public void Walk(Vector3 destination, int destinationIndex)
    {
        _destinationIndex = destinationIndex;

        if (_actState != ActState.Follow)
        {
            _actState = ActState.Patrol;
            Walk(destination);
        }
    }
    void Walk(Vector3 destination)
    {
        _agent.isStopped = false;

        _agent.speed = _walkSpeed;
        _agent.SetDestination(destination);
        _aniState = AniState.Walk;
        SynchronizeAniController();
    }
    void SynchronizeAniController()
    {
        _aniController.SetInteger(_aniStateHash, (int)_aniState);
    }

    void Run()
    {
        _agent.isStopped = false;
        _agent.speed = _runSpeed;
        _agent.SetDestination(_player.transform.position);

        _aniState = AniState.Run;
        SynchronizeAniController();

    }

    void Attack()
    {
        _aniState = AniState.Attack;
        SynchronizeAniController();
        _aniController.SetTrigger("Attack1");

        _agent.isStopped = true;
        _attackTimer = 1.2f;
    }

    public void Hit(MainCharacter player)
    {
        _currentHp -= player._damage;

        if (_currentHp <= 0)
        {
            Dead();

        }
        else
        {
            _aniState = AniState.Hit;
            SynchronizeAniController();
            _aniController.SetTrigger("Hit");
            _agent.isStopped = true;
            _attackTimer = Mathf.Max(_attackTimer, 0.6f);

            if (_actState != ActState.Follow)
            {
                DetectTarget(player);
            }
        }
    }
    public void Dead()
    {
        _attackZoneCollider.enabled = false;
        _agent.isStopped = true;
        _agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        _currentHp = 0;
        _aniState = AniState.Dead;
        SynchronizeAniController();
        _actState = ActState.None;

        _parentFactory.EnemyDown(_factoryIndex);
    }

    public void Idle()
    {
        _aniState = AniState.Idle;
        SynchronizeAniController();
    }


    public void DetectTarget(MainCharacter player)
    {
        _player = player;

        _actState = ActState.Follow;

        _agent.stoppingDistance = _attackRange;

    }



    public void OnAttackZoneEnable()
    {
        _attackZoneCollider.enabled = true;
    }

    public void OnAttackZoneDisable()
    {
        _attackZoneCollider.enabled = false;
    }
}
