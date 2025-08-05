using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("발사체 자원")]
    [SerializeField] GameObject _prefabNoneTarget;
    [SerializeField] GameObject _prefabTarget;
    [Header("발사체 설정")]
    [SerializeField] float _moveForce = 200;
    [SerializeField] float _rotAngle = 180;
    [SerializeField] float _lifeTime = 10;

    MainCharacter _owner;
    Rigidbody _rgbd3D;

    Transform _trMe;
    Transform _target;

    private void Update()
    {
        if (_target != null)
        {
            Quaternion goalRot = Quaternion.LookRotation(_target.position - _trMe.position);
            _trMe.rotation = Quaternion.RotateTowards(transform.rotation, goalRot, Time.deltaTime * _rotAngle);

        }
    }


    public void InitProjectile(MainCharacter owner, in Vector3 direction, Transform target = null)
    {
        _owner = owner;
        _rgbd3D = GetComponent<Rigidbody>();
        _trMe = transform;
        if (target != null)
        {
            _target = target;
        }

        _rgbd3D.AddForce(direction * _moveForce);

        Destroy(gameObject, _lifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Map"))
        {
            //Instantiate(_prefabNoneTarget, transform.position, _prefabTarget.transform.rotation);
            Instantiate(_prefabNoneTarget, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            Instantiate(_prefabTarget, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
