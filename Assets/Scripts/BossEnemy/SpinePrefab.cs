using UnityEngine;

public class SpineProjectile : MonoBehaviour
{
    private Transform target; // 플레이어 타겟
    private Rigidbody2D rb;

    [SerializeField] private float initialSpeed = 5f; // 초기 속도
    [SerializeField] private float rotationSpeed = 200f; // 회전 속도
    [SerializeField] private float decelerationRate = 0.95f; // 감속 비율 (1보다 작아야 함)
    [SerializeField] private float dashSpeed = 10f; // 돌진 속도
    [SerializeField] private float dashTime = 2f; // 추적 후 돌진으로 전환되는 시간
    private bool isDashing = false; // 돌진 상태 플래그

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // 일정 시간 후 돌진 상태로 전환
        Invoke(nameof(StartDash), dashTime);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash(); // 직선 돌진
        }
        else
        {
            TrackAndCurve(); // 플레이어 추적 및 곡선 이동
        }
    }

    private void TrackAndCurve()
    {
        if (target == null) return;

        // 타겟 방향 계산
        Vector2 direction = (target.position - transform.position).normalized;

        // 가시 회전
        float rotateAmount = Vector3.Cross(direction, transform.right).z; // 회전량 계산
        rb.angularVelocity = -rotateAmount * rotationSpeed; // 회전 속도 적용

        // 가시 이동
        rb.linearVelocity = transform.right * initialSpeed;

        // 감속 처리
        if (initialSpeed > dashSpeed) // 감속이 돌진 속도보다 크면
        {
            initialSpeed *= decelerationRate;
        }
    }

    private void Dash()
    {
        // 직선 이동 (회전과 추적 멈춤)
        rb.angularVelocity = 0f; // 회전 멈춤
        rb.linearVelocity = transform.right * dashSpeed; // 직선 이동
    }

    private void StartDash()
    {
        isDashing = true; // 돌진 상태로 전환
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어나 벽 등에 닿았을 때 가시 제거
        if (collision.CompareTag("Player") || collision.CompareTag("ground"))
        {
            Destroy(gameObject); // 가시 제거
        }
    }
}
