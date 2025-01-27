using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("공통 설정")]
    public bool isDie = false; // 적이 사망했는지 여부
    protected bool isAttacked = false; // 적이 공격받는 중인지 여부
    protected Rigidbody2D rb; // Rigidbody2D 참조
    [SerializeField] protected int hp = 20; // 적 체력
    public int damage = 10; // 적 공격력
    [SerializeField] protected GameObject itemPrefab; // 드랍할 아이템 프리팹
    public PlayerController playerController; // 플레이어 컨트롤러 참조
    public EnemyStateMachine stateMachine; // 상태 머신 참조

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 상태 머신 초기화
        stateMachine = new EnemyStateMachine(this);

    }
    protected virtual void Start()
    {
        // 상태 머신의 초기 상태를 자식에서 설정
    }

    protected virtual void Update()
    {
        if (isDie) return;
        PerformMovement(); // 이동 로직
    }

    protected abstract void PerformMovement(); // 이동 로직(이동형/고정형에서 각각 구현)


    //적이 공격을 받으면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie || isAttacked) return; // 이미 죽은 적은 충돌 무시
        if (collision.gameObject.CompareTag("Attack"))
        {
            Attack attack = collision.GetComponent<Attack>();
            if (attack != null)
            {
                EnemyTakeDamage(attack.damage);

                //일정 시간동안 추가 공격 무시
                isAttacked = true;
                Invoke("ResetAttackState", 1f); // 0.5초 후 다시 공격 가능
            }
        }
    }

    //일정 시간동안 추가 공격 무시 메서드
    private void ResetAttackState()
    {
        isAttacked = false; // 공격 가능 상태로 복귀
    }
    // 데미지를 입을때
    public void EnemyTakeDamage(int damage)
    {
        if (isDie) return;

        hp -= damage;
        if (hp <= 0)
        {
            Die();
            Debug.Log("Die");
        }
        else
        {
            stateMachine.TransitionTo(stateMachine.hitState);
            Debug.Log($"{gameObject.name} 체력: {hp}");
        }
    }

    // 적의 Attack
    public void Attack(int damage)
    {
        //스킬 데미지(TakeDamage는 적에게 닿았을때 체력이 닳는 메서드기 때문에 Attack을 받았을때 조금 더 닳게 함)
        playerController.TakeDamage(damage + 10);
        Debug.Log("스킬 데미지");
    }

    // 적이 죽는 메서드
    protected virtual void Die()
    {
        isDie = true; // 사망 상태 설정
        stateMachine.TransitionTo(stateMachine.dieState);
        Debug.Log(stateMachine.CurrentState);
        Debug.Log($"{gameObject.name} 사망");

        // 일정 시간 후 적 제거
        Invoke("DestroyEnemy", 1f);
    }

    void DestroyEnemy()
    {
        //적 사라짐
        Destroy(gameObject);
        //아이템 드랍
        SpawnItem();
    }

    // 플레이어가 닿으면 피가 닳는 메서드
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("playerController : " + playerController);
            //플레이어에게 데미지 입히기
            playerController.TakeDamage(damage);
            Debug.Log("일반 데미지");
        }
    }

    protected virtual void SpawnItem()
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
