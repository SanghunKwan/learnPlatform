using UnityEngine;

public class EnemyDetectTrigger : MonoBehaviour
{
    [SerializeField] EnemyCharacter _owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Fireball"))
        {
            _owner.DetectTarget(IngameManager._instance._Player);
        }
    }

}
