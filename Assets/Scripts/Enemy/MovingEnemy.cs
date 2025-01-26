using UnityEngine;

public class MovingEnemy : BaseEnemy
{
    [Header("이동 관련 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float traceSpeed = 7f;
    [SerializeField] private float detectionRange = 5f;
    private bool isFollowing = false; // 플레이어 추적 여부
    [SerializeField] private Transform groundCheck, wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 2f;
    [SerializeField] private Collider2D traceCollider; // 플레이어 추적 감지 범위

    [SerializeField] private bool movingRight = true;
    [SerializeField] private Transform player;
    private Vector3 originalScale; // 초기 로컬 스케일 저장

    private void Start()
    {
        originalScale = transform.localScale; // 초기 로컬 스케일 저장
    }
    protected override void PerformMovement()
    {
        if (isFollowing)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
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
        Move(movingRight ? traceSpeed : -traceSpeed);
    }

    private void Move(float speed)
    {
        if (!isDie)
        {
            // 이동 처리
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

/*            // 상태 전환은 한 번만 수행
            if (stateMachine.CurrentState != stateMachine.walkState)
            {
            }*/
            stateMachine.TransitionTo(stateMachine.walkState);

            Debug.Log("speed : " + speed);
            // 스프라이트 방향 조정
            AdjustSpriteDirection(speed);
        }
    }


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

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 감지 시 추적 시작
        if (collision.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }

    private bool IsGroundAhead() =>
        Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

    private bool IsWallAhead() =>
        Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);


    private void AdjustSpriteDirection(float moveDirection)
    {
        // 이동 방향에 따라 스프라이트 뒤집기
        if ((moveDirection < 0 && transform.localScale.x > 0) ||
            (moveDirection > 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector3(transform.localScale.x, originalScale.y, originalScale.z);
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

    }
}
