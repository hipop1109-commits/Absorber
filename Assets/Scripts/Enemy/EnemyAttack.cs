using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private BaseEnemy enemy;
    void Start()
    {
        // BaseEnemy 할당
        enemy = GetComponentInParent<BaseEnemy>();
    }
    // 공격 범위에 들어왔는지 확인
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그로 플레이어를 감지
        {
            Attack();
        }

    }

    /// <summary>
    /// 적이 플레이어를 공격하는 메서드
    /// </summary>
    public void Attack()
    {
        //공격 데미지
        enemy.Attack(enemy.damage);
    }
}
