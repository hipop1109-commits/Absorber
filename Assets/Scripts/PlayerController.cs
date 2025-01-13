using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer; // 캐릭터의 스프라이트 렌더러
    private Animator animator; // 캐릭터 애니메이션을 제어할 Animator

    [SerializeField] private float moveSpeed = 10f; // 캐릭터 이동 속도
    [SerializeField] private float jumpSpeed = 10f; // 캐릭터 점프 속도
    [SerializeField] private float dashSpeed = 20f; // 캐릭터 대쉬 속도
    [SerializeField] private float dashCooldown = 1f; // 대쉬 쿨타임
    private float dashDuration = 0.2f; // 대쉬 지속 시간
    private float dashCooldownTimer; // 대쉬 쿨타임을 계산하기 위한 타이머

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D

    private Vector2 moveDirection; // 입력된 이동 방향

    private bool isJump; // 점프 중인지 여부를 나타내는 플래그
    private bool isDashing; // 대쉬 중인지 여부를 나타내는 플래그

    public StateMachine stateMachine; // 캐릭터의 상태를 관리할 상태 머신

    private void Awake()
    {
        // 컴포넌트 초기화
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine(this); // 상태 머신 초기화
    }

    private void Start()
    {
        // 상태 머신의 초기 상태를 Idle로 설정
        stateMachine.Initalize(stateMachine.idleState);
    }

    private void FixedUpdate()
    {
        // 대쉬 중이 아닌 경우에만 이동 처리
        if (!isDashing)
        {
            ApplyMovement();
        }
    }

    private void Update()
    {
        // 상태 머신의 현재 상태를 실행
        stateMachine.Execute();

        // 마우스 위치를 기반으로 캐릭터의 스프라이트 방향 설정
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(UnityEngine.Camera.main.transform.position.z))
        );

        // 마우스 위치와 캐릭터의 현재 위치를 비교해 캐릭터의 방향 설정
        if ((mouseWorldPosition.x > transform.position.x && spriteRenderer.flipX) ||
            (mouseWorldPosition.x < transform.position.x && !spriteRenderer.flipX))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // 대쉬 쿨타임 타이머 업데이트
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // 애니메이터 상태 업데이트
        UpdateAnimatorStates();
    }

    // 이동 입력 처리
    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>(); // 이동 방향 설정
        if (moveDirection.x != 0)
        {
            stateMachine.TransitionTo(stateMachine.walkState); // 이동 중 상태로 전환
        }
    }

    // 점프 입력 처리
    public void OnJump(InputValue value)
    {
        if (value.isPressed && !isJump)
        {
            isJump = true; // 점프 플래그 설정
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed); // 점프 속도 적용
            stateMachine.TransitionTo(stateMachine.jumpState); // 점프 상태로 전환
        }
    }

    // 대쉬 입력 처리
    public void OnSprint(InputValue value)
    {
        if (value.isPressed && !isDashing && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash()); // 대쉬 코루틴 실행
            stateMachine.TransitionTo(stateMachine.dashState); // 대쉬 상태로 전환
        }
    }

    // 이동 로직 처리
    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y); // 이동 속도 적용
    }

    // 대쉬 로직 처리
    private System.Collections.IEnumerator Dash()
    {
        isDashing = true; // 대쉬 플래그 설정
        animator.SetBool("isDashing", true); // 대쉬 애니메이션 활성화
        Vector2 dashForce = new Vector2(moveDirection.x * dashSpeed, 0); // 대쉬 방향 설정
        rb.AddForce(dashForce, ForceMode2D.Impulse); // 대쉬 힘 적용
        yield return new WaitForSeconds(dashDuration); // 대쉬 지속 시간 대기
        isDashing = false; // 대쉬 플래그 해제
        dashCooldownTimer = dashCooldown; // 대쉬 쿨타임 설정
        animator.SetBool("isDashing", false); // 대쉬 애니메이션 비활성화
    }

    // 바닥 충돌 감지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false; // 점프 플래그 해제
            stateMachine.TransitionTo(stateMachine.idleState); // Idle 상태로 전환
        }
    }

    // 애니메이터 상태 업데이트
    private void UpdateAnimatorStates()
    {
        animator.SetBool("isWalking", moveDirection.x != 0 && !isJump && !isDashing); // 걷기 상태
        animator.SetBool("isJumping", isJump); // 점프 상태
    }
}
