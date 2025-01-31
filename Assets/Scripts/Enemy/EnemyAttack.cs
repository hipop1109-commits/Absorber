using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerController playerController; // 플레이어 컨트롤러 참조
    private BaseEnemy baseEnemy;
    private void Start()
    {
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }

    // 플레이어가 닿으면 피가 닳는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController = collision.gameObject.GetComponent<PlayerController>();
        playerController.TakeDamage(baseEnemy.damage);
    }
}
