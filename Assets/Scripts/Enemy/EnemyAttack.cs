using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerController playerController; // 플레이어 컨트롤러 참조
    private BaseEnemy baseEnemy;

    protected virtual void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    private void Start()
    {
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }

    // 플레이어가 닿으면 피가 닳는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
          if(collision.CompareTag("Player"))
        {
            playerController.TakeDamage(baseEnemy.damage);
        }

        Debug.Log("확인 : " + transform.parent.name);

        //playerController = collision.gameObject.GetComponent<PlayerController>();
        //playerController.TakeDamage(baseEnemy.damage);
    }
}
