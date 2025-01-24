using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("일반 이동 설정")]
    [SerializeField] private float moveSpeed = 2f; // 일반 이동 속도
    [SerializeField] private Transform groundCheck; // 낭떠러지를 감지하는 위치
    [SerializeField] private Transform wallCheck;   // 벽을 감지하는 위치
    [SerializeField] private float checkRadius = 0.2f; // 감지 반경
    [SerializeField] private LayerMask groundLayer; // Ground 레이어
    [SerializeField] private bool movingRight = true; // 현재 이동 방향 (true: 오른쪽, false: 왼쪽)

    [Header("플레이어 추적 설정")]
    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private float traceSpeed = 2.5f; // 추적 속도
    [SerializeField] private Collider2D traceCollider; // 추적 감지 범위

    private Rigidbody2D rb;
    private bool isFollowing = false; // 추적 여부
    public EnemyStateMachine e_stateMachine; // 적의 상태를 관리할 스테이트 머신
    private EnemyController enemyController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 초기화
        enemyController = GetComponent<EnemyController>();
        e_stateMachine = enemyController.stateMachine;
    }

    void Update()
    {
        if (isFollowing)
        {
            FollowPlayer(); // 플레이어 추적
            e_stateMachine.TransitionTo(e_stateMachine.walkState);
        }
        else
        {
            Patrol(); // 일반 이동
            e_stateMachine.TransitionTo(e_stateMachine.walkState);
        }
    }

    void Patrol()
    {
        // 낭떠러지 감지: groundCheck 위치에서 낭떠러지가 있는지 확인
        bool isGroundAhead = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 높은 땅 감지: wallCheck 위치에서 높은 땅을 감지
        bool isWallAhead = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        // 낭떠러지거나 높은 땅(벽)이 감지되면 방향 전환
        if (!isGroundAhead || isWallAhead)
        {
            movingRight = !movingRight; // 방향 전환
        }

        // 이동 처리
        float moveDirection = movingRight ? -1 : 1; // 오른쪽이면 1, 왼쪽이면 -1
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);


        // 적의 스프라이트 방향 조정
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (moveDirection > 0 ? -1 : 1), transform.localScale.y, transform.localScale.z);
    }

    void FollowPlayer()
    {
        // 플레이어가 없으면 동작하지 않음
        if (player == null) return;

        // 방향 계산
        float direction = player.position.x - transform.position.x;
        float moveDirection = direction > 0 ? -1 : 1; // 플레이어가 오른쪽에 있으면 1, 왼쪽에 있으면 -1

        // 속도 설정
        rb.linearVelocity = new Vector2(moveDirection * traceSpeed, rb.linearVelocity.y);

        // 스프라이트 방향 조정
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (moveDirection > 0 ? -1 : 1), transform.localScale.y, transform.localScale.z);
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그로 플레이어를 감지
        {
            isFollowing = true; // 추적 시작
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFollowing = false; // 추적 중지
        }
    }

    // Gizmos를 사용하여 groundCheck와 wallCheck의 감지 반경을 시각적으로 확인
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
        }

        if (traceCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(traceCollider.bounds.center, traceCollider.bounds.size);
        }
    }
}
