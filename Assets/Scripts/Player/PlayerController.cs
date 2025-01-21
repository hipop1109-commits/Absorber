using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 캐릭터의 스프라이트 렌더러
    public Player player;

    private float dashCooldownTimer; // 대쉬 쿨타임을 계산하기 위한 타이머

    public int select1 = 1;
    public int select2 = 1;

    [SerializeField] private bool isHit = false;
    [SerializeField] private bool isDie = false;

    [SerializeField] private float invincibilityTime = 1f; // 캐릭터 무적 시간
    [SerializeField] private float moveSpeed = 10f; // 캐릭터 이동 속도
    [SerializeField] private float jumpSpeed = 10f; // 캐릭터 점프 속도

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D

    private Vector2 moveDirection; // 입력된 이동 방향

    private bool isJump; // 점프 중인지 여부를 나타내는 플래그
    private bool isDashing; // 대쉬 중인지 여부를 나타내는 플래그

    public StateMachine stateMachine; // 캐릭터의 상태를 관리할 상태 머신
    RopeActive grappling;

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
                dashDuration: 0.2f// 대쉬 지속 시간
            );

        // 상태 머신 초기화
        stateMachine = new StateMachine(this);
    }

    private void Start()
    {
        // 상태 머신의 초기 상태를 Idle로 설정
        stateMachine.Initalize(stateMachine.idleState);
        grappling = GetComponent<RopeActive>();
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

        Debug.Log("isDie: " + isDie);
        Debug.Log("player.IsAlive(): " + player.IsAlive());
    }

    // 플레이어가 데미지를 받을 때
    public void TakeDamage(int damage)
    {
        if(player.IsAlive() && !isHit && !isDie) 
        {
            isHit = true; 
            player.Damage(damage);

            //player 색 바뀌게(다치는 모션 or 무적 모션)
            stateMachine.TransitionTo(stateMachine.hurtState);

            //Player 무적 시간
            Invoke("Invincibility",invincibilityTime);

            Debug.Log($"Player Hp : {player.PlayerHp}");
        }
        if(!player.IsAlive() && !isDie)
        {
            isDie = true;
            Die();
        }
    }

    //무적시간
    private void Invincibility()
    {
        isHit = false;  
    }


    public void Die()
    {
        stateMachine.TransitionTo(stateMachine.dieState);
        Debug.Log("Player Die");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }


    //select1값을 가져오는 메소드 
    public int GetSelect1()
    {
        return select1;
    }

    //select2값을 가져오는 메소드
    public int GetSelect2()
    {
        return select2;
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
        // SlowTime 작동 및 시간 복원 코루틴 시작
        //SlowTime.Instance.Slow();
        StopAllCoroutines(); // 이전 코루틴 종료 (중복 방지)
        //StartCoroutine(ResetTimeScale());
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
        // SlowTime 작동 및 시간 복원 코루틴 시작
        //SlowTime.Instance.Slow();
        StopAllCoroutines(); // 이전 코루틴 종료 (중복 방지)
        //StartCoroutine(ResetTimeScale());
    }

    private System.Collections.IEnumerator ResetTimeScale()
    {
        yield return new WaitForSecondsRealtime(SlowTime.Instance.slowLength);
        SlowTime.Instance.Back();
    }


    //우클릭 흡수
    public void OnAbsorb()
    {
        Debug.Log("흡수");
        WeaponController.Instance.AbsorbClick();
    }

    //우클릭 흡수 취소
    public void OnAbsorbCancle()
    {
        Debug.Log("흡수 취소");
        WeaponController.Instance.AbsorbClickUp();
    }

    //좌클릭 방출
    public void OnEmit()
    {
        Debug.Log("방출");
        Combination();
        WeaponController.Instance.WeaponSelect();
    }


    //좌클릭 방출 취소
    public void OnEmitCancle()
    {
        Debug.Log("방출취소");
        WeaponController.Instance.WeaponLeft();
    }


    //조합 결과
    private void Combination()
    {
        Dictionary<(int, int), string> combinations = new Dictionary<(int, int), string>
        {
            { (1, 1), "water"}, //물+물
            { (2, 2), "treeVine" }, // 풀+풀
            { (3, 3), "rockBomb" }, // 바위+바위
            { (1, 2), "potion" }, // 물+풀
            { (2, 1), "potion" }, // 풀+물
            { (2, 3), "platform" }, // 풀+바위
            { (3, 2), "platform" }, // 바위+풀
            { (1, 3), "bullet" }, // 물+바위
            { (3, 1), "bullet" }  // 바위+물
        };

        //선택된 조합
        var selectedCombination = (select1, select2);

        // 조합에 해당하는 결과 출력
        if (combinations.TryGetValue(selectedCombination, out string result))
        {
            Debug.Log($"방출: {result}");
            WeaponController.Instance.WeaponMode = result;
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
        if (grappling.isAttach)
        {
            rb.AddForce(new Vector2(moveDirection.x * moveSpeed, 0));
        }
        else
        {
            Vector2 targetVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.1f); // 부드러운 변화        }
        }
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
