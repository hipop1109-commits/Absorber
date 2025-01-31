using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ����
    private BaseEnemy baseEnemy;

    protected virtual void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    private void Start()
    {
        // enemyController 할당
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

    public void Attack()
    {
          if(collision.CompareTag("Player"))
        {
            playerController.TakeDamage(baseEnemy.damage);
        }

        Debug.Log("Ȯ�� : " + transform.parent.name);

        //playerController = collision.gameObject.GetComponent<PlayerController>();
        //playerController.TakeDamage(baseEnemy.damage);
    }
}
