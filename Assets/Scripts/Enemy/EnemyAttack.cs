using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerController playerController;
    private BaseEnemy baseEnemy;

    protected virtual void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    private void Start()
    {
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.TakeDamage(baseEnemy.damage);
        }


        //playerController = collision.gameObject.GetComponent<PlayerController>();
        //playerController.TakeDamage(baseEnemy.damage);
    }
}
