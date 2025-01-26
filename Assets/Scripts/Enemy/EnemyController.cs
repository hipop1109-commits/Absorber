using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("적 상태 설정")]
    public bool isDie = false;
    private bool isAttack = false;
    private bool isFollowing = false; // 플레이어 추적 여부
    [SerializeField] private int hp = 20;
    public int damage = 10;
    Vector2 vx; 
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private Vector3 originalScale; // 초기 로컬 스케일 저장

    [Header("일반 이동 설정")]
    [SerializeField] private float moveSpeed = 8f; //적 이동 속도
    [SerializeField] private Transform groundCheck; // 낭떠러지를 감지하는 위치
    [SerializeField] private Transform wallCheck;   // 벽을 감지하는 위치
    [SerializeField] private float checkRadius = 0.2f; // 감지 반경
    [SerializeField] private LayerMask groundLayer; // Ground 레이어
    [SerializeField] private bool movingRight = false; // 현재 이동 방향

    [Header("플레이어 추적 설정")]
    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private float traceSpeed = 15f; // 플레이어 추적 속도
    [SerializeField] private Collider2D traceCollider; // 플레이어 추적 감지 범위

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject itemPrefab;
    public EnemyStateMachine stateMachine; // 적의 상태를 관리할 상태 머신

    private void Awake()
    {
        // 상태 머신 초기화
        stateMachine = new EnemyStateMachine(this);
        // 상태 머신의 초기 상태를 Idle로 설정
        stateMachine.Initalize(stateMachine.idleState);

    }

    private void Start()
    {
        vx = Vector2.left * moveSpeed;
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 초기화

        originalScale = transform.localScale; // 초기 로컬 스케일 저장
    }

    private void FixedUpdate()
    {
        if (isDie)
        {
            if (stateMachine.CurrentState != stateMachine.dieState)
            {
                stateMachine.TransitionTo(stateMachine.dieState);
            }
            return;
        }

        else
        {
            if (isFollowing)
            {
                FollowPlayer(); // 플레이어 추적 동작
            }
            else
            {
                Patrol(); // 일반 이동 동작
            }
        }

        Debug.Log("IsGroundAhead() : " + IsGroundAhead());
    }


    private void Patrol()
    {
        // 낭떠러지 또는 벽 감지 시 방향 전환
        if (!IsGroundAhead() || IsWallAhead())
        {
            movingRight = !movingRight;
        }

        // 이동 처리
        Move(movingRight ? moveSpeed : -moveSpeed);
    }

    private void FollowPlayer()
    {
        if (player == null) return; // 플레이어가 없으면 리턴

        // 플레이어와의 x축 거리 계산
        float directionToPlayer = player.position.x - transform.position.x;

        // 낭떠러지 또는 벽 감지 시 방향 전환
        if (!IsGroundAhead() || IsWallAhead())
        {
            movingRight = !movingRight;
        }

        // 플레이어 위치에 따라 방향 설정
        movingRight = directionToPlayer < 0;

        // 이동 처리
        Move(movingRight ? -traceSpeed : traceSpeed);
    }

    private void Move(float speed)
    {
        if (!isDie)
        {
            // 이동 처리
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

            // 상태 전환은 한 번만 수행
            if (stateMachine.CurrentState != stateMachine.walkState)
            {
                stateMachine.TransitionTo(stateMachine.walkState);
            }

            // 스프라이트 방향 조정
            AdjustSpriteDirection(speed);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 데미지 입히기
            playerController.TakeDamage(damage);
            Debug.Log("일반 데미지");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 감지 시 추적 시작
        if (collision.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }

    //적이 공격을 받으면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie||isAttack) return; // 이미 죽은 적은 충돌 무시
        if (collision.gameObject.CompareTag("Attack"))
        {
            Attack attack = collision.GetComponent<Attack>();
            if (attack != null)
            {
                EnemyTakeDamage(attack.damage);

                //일정 시간동안 추가 공격 무시
                isAttack = true;
                Invoke("ResetAttackState", 1f); // 0.5초 후 다시 공격 가능
            }
        }
    }

    //일정 시간동안 추가 공격 무시 메서드
    private void ResetAttackState()
    {
        isAttack = false; // 공격 가능 상태로 복귀
    }

    // 플레이어가 범위에서 벗어나면
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어 추적 중지
        if (collision.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }

    //적이 피해를 입는 메서드
    public void EnemyTakeDamage(int damage)
    {
        //if (isDie) return; // 이미 죽은 상태라면 무시

        //죽었을때
        if (hp <= 0 && !isDie)
        {
            isDie = true;

            Invoke("DestroyEnemy", 2f);
        }
        //맞았을때
        else
        {
            hp -= damage;

            // 적이 피해를 입는 애니메이션
            stateMachine.TransitionTo(stateMachine.hitState);
            Debug.Log($"적{gameObject} 체력 : {hp}");

        }
    }


    void DestroyEnemy()
    {
        //적 사라짐
        Destroy(gameObject);
        //아이템 드랍
        SpawnItem();
    }

    //적이 공격하는 메서드
    public void Attack(int damage)
    {
        //스킬 데미지(TakeDamage는 적에게 닿았을때 체력이 닳는 메서드기 때문에 Attack을 받았을때 조금 더 닳게 함)
        playerController.TakeDamage(damage + 10);
        Debug.Log("스킬 데미지");
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


    private bool IsGroundAhead()
    {
        // 낭떠러지 감지
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    private bool IsWallAhead()
    {
        // 벽 감지
        return Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);
    }

    private void AdjustSpriteDirection(float moveDirection)
    {
        // 이동 방향에 따라 스프라이트 뒤집기
        if ((moveDirection < 0 && transform.localScale.x < 0) ||
            (moveDirection > 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, originalScale.y, originalScale.z);
        }
    }



    private void OnDrawGizmosSelected()
    {
        // 낭떠러지 감지 반경 시각화
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        // 벽 감지 반경 시각화
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
        }

        // 플레이어 추적 범위 시각화
        if (traceCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(traceCollider.bounds.center, traceCollider.bounds.size);
        }
    }

}
