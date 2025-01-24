using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("일반 이동 설정")]
    [SerializeField] private float moveSpeed = 2f; // 일반 이동 속도
    [SerializeField] private Transform groundCheck; // 낭떠러지를 감지하는 위치
    [SerializeField] private Transform wallCheck;   // 벽을 감지하는 위치
    [SerializeField] private float checkRadius = 0.2f; // 감지 반경
    [SerializeField] private LayerMask groundLayer; // Ground 레이어
    [SerializeField] private bool movingRight = false; // 현재 이동 방향

    [Header("플레이어 추적 설정")]
    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private float traceSpeed = 2.5f; // 플레이어 추적 속도
    [SerializeField] private Collider2D traceCollider; // 플레이어 추적 감지 범위

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private bool isFollowing = false; // 플레이어 추적 여부
    private Vector3 originalScale; // 초기 로컬 스케일 저장
    private EnemyController enemyController; // 적 컨트롤러
    private EnemyStateMachine e_stateMachine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 초기화
        enemyController = GetComponent<EnemyController>(); // EnemyController 초기화
        e_stateMachine = enemyController.stateMachine;

        originalScale = transform.localScale; // 초기 로컬 스케일 저장
    }

    private void Update()
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
        // Rigidbody2D를 이용한 이동
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        e_stateMachine.TransitionTo(e_stateMachine.walkState);

        // 스프라이트 방향 조정
        AdjustSpriteDirection(speed);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 감지 시 추적 시작
        if (collision.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어 추적 중지
        if (collision.CompareTag("Player"))
        {
            isFollowing = false;
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
