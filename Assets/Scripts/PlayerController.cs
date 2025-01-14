using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 캐릭터의 스프라이트 렌더러
    private Player player;

    private float dashCooldownTimer; // 대쉬 쿨타임을 계산하기 위한 타이머

    [SerializeField] private int select1 = 1;
    [SerializeField] private int select2 = 1;

    [SerializeField] private float moveSpeed = 10f; // 캐릭터 이동 속도
    [SerializeField] private float jumpSpeed = 10f; // 캐릭터 점프 속도

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

        player = new Player(
                maxHp: 100, // 최대 HP
/*                moveSpeed: 10f, // 이동 속도
                jumpSpeed: 10f, // 점프 속도*/
                dashSpeed: 20f, // 대쉬 속도
                dashCooldown: 1f, // 대쉬 쿨타임
                dashDuration: 0.2f // 대쉬 지속 시간
            );

        // 상태 머신 초기화
        stateMachine = new StateMachine(this);
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
    }


    //Q(물(1), 풀(2), 바위(3))
    public void OnSelect1(InputValue value)
    {
        if (select1 >= 3)
        {
            select1 = 1;
        }
        else
        {
            select1 += 1;
        }
    }

    //E
    public void OnSelect2(InputValue value)
    {
        if (select2 >= 3)
        {
            select2 = 1;
        }
        else
        {
            select2 += 1;
        }
    }

    //우클릭 흡수
    public void OnAbsorb(InputValue value)
    {
        Debug.Log("흡수");
        WeaponController.Instance.AbsorbClick();
    }

    //우클릭 흡수
    public void OnAbsorbCancle(InputValue value)
    {
        Debug.Log("흡수 취소");
        WeaponController.Instance.AbsorbClickUp();
    }

    //좌클릭 흡수
    public void OnEmit(InputValue value)
    {
        Debug.Log("방출");
        Combination();
    }


    //조합 결과
    private void Combination()
    {
        Dictionary<(int, int), string> combinations = new Dictionary<(int, int), string>
        {
            { (1, 1), "파도타기"}, //물+물
            { (2, 2), "나무덩쿨" }, // 풀+풀
            { (3, 3), "바위 폭탄" }, // 바위+바위
            { (1, 2), "힐링 포션" }, // 물+풀
            { (2, 1), "힐링 포션" }, // 풀+물
            { (2, 3), "발판 생성" }, // 풀+바위
            { (3, 2), "발판 생성" }, // 바위+풀
            { (1, 3), "바위 총알" }, // 물+바위
            { (3, 1), "바위 총알" }  // 바위+물
        };

        //선택된 조합
        var selectedCombination = (select1, select2);

        // 조합에 해당하는 결과 출력
        if (combinations.TryGetValue(selectedCombination, out string result))
        {
            Debug.Log($"방출: {result}");
        }
        else
        {
            Debug.Log("유효하지 않은 조합입니다.");
        }
    }


    // 이동 입력 처리
    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>(); // 이동 방향 설정

        if (moveDirection.x != 0)
        {
            // 이동 입력이 있는 경우 Walk 상태로 전환
            stateMachine.TransitionTo(stateMachine.walkState);
        }
        else
        {
            // 이동 입력이 없는 경우 Idle 상태로 전환
            stateMachine.TransitionTo(stateMachine.idleState);
        }
    }


    // 점프 입력 처리
    public void OnJump(InputValue value)
    {
        if (value.isPressed && !isJump)
        {
            isJump = true; // 점프 플래그 설정
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeed); // 점프 속도 적용
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
        Vector2 dashForce = new Vector2(moveDirection.x * player.DashSpeed, 0); // 대쉬 방향 설정
        rb.AddForce(dashForce, ForceMode2D.Impulse); // 대쉬 힘 적용
        yield return new WaitForSeconds(player.DashDuration); // 대쉬 지속 시간 대기
        isDashing = false; // 대쉬 플래그 해제
        dashCooldownTimer = player.DashCooldown; // 대쉬 쿨타임 설정
        stateMachine.TransitionTo(stateMachine.idleState); // Idle 상태로 전환
    }

    // 바닥 충돌 감지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false; // 점프 플래그 해제
            if (moveDirection.x != 0)
            {
                // 이동 입력이 있는 경우 Walk 상태로 전환
                stateMachine.TransitionTo(stateMachine.walkState);
            }
            else
            {
                // 이동 입력이 없는 경우 Idle 상태로 전환
                stateMachine.TransitionTo(stateMachine.idleState);
            }
        }
    }

}
