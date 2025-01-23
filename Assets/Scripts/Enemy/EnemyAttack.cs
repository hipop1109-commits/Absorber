using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController enemyController;

    void Start()
    {
        // enemyController 할당
        enemyController = GetComponentInParent<EnemyController>();
    }
    // 공격 범위에 들어왔는지 확인
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그로 플레이어를 감지
        {
            Attack();
        }
    }

    public void Attack()
    {
        //공격 데미지
        enemyController.Attack(enemyController.damage);
    }
}
