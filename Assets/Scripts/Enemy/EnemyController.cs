using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; //적 이동 속도
    [SerializeField] private int hp = 20;
    [SerializeField] private int damage = 10;
    Animator animator;

    [SerializeField] private float groundCheckRadius = 0.1f; //바닥 감지 반경

    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject itemPrefab;

    Vector2 vx;

    public EnemyStateMachine stateMachine; // 적의 상태를 관리할 상태 머신

    private void Awake()
    {
        // 상태 머신 초기화
        stateMachine = new EnemyStateMachine(this);

    }
    
    private void Start()
    {
        // 상태 머신의 초기 상태를 Idle로 설정
        stateMachine.Initalize(stateMachine.idleState);
        vx = Vector2.left * moveSpeed;
        Debug.Log("적 체력 : "+hp);
    }

    private void Update()
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 데미지 입히기
            playerController.TakeDamage(damage);
        }
    }

    //적이 공격을 받는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Attack attack = collision.GetComponent<Attack>();
            if(attack != null)
            {
                TakeDamage(attack.damage);
                //적이 피해를 입는 애니메이션
                stateMachine.Initalize(stateMachine.hitState);
                Debug.Log("Attack");
            }
        }
    }

    //적이 피해를 입는 메서드

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"적{gameObject} 체력 : {hp}"); 
        if(hp <= 0)
        {
            //적 죽는 애니메이션
            stateMachine.TransitionTo(stateMachine.dieState);
            Debug.Log(stateMachine.CurrentState);

            Invoke("DestroyEnemy", 1);
        }
    }
    void DestroyEnemy()
    {
        //적 사라짐
        Destroy(gameObject);
        //아이템 드랍
        SpawnItem();
    }

    //아이템 드랍 메서드
    public void SpawnItem()
    {
        if (itemPrefab != null)
        {
            // 적 위치보다 약간 위에 아이템 생성
            GameObject item = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 위로 올라가는 힘을 줌
                rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                // 중력을 해제하여 바닥으로 빠지는 문제 방지
                rb.gravityScale = 1f; // 중력을 줄여 자연스러운 움직임 유지
            }
        }
    }
}
