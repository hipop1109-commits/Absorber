using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 캐릭터의 스프라이트 렌더러

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

    private bool isSelecting; // Space바가 눌린 상태인지 여부
    private bool isFirstSelect = true; // 첫 번째 선택인지 여부
    public int currentSelection1 = 1; // 첫 번째 선택된 값 (1, 2, 3)
    public int currentSelection2 = 1; // 두 번째 선택된 값 (1, 2, 3)

    private void Awake()
    {
        // 컴포넌트 초기화
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 상태 머신의 초기 상태를 Idle로 설정
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
        // Space 입력 처리
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isSelecting = true;
            Debug.Log(isFirstSelect ? "첫 번째 선택 시작" : "두 번째 선택 시작");
        }
        else if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isSelecting = false;

            if (isFirstSelect)
            {
                Debug.Log($"첫 번째 선택 확정: {currentSelection1}");
                isFirstSelect = false; // 다음 선택으로 전환
            }
            else
            {
                Debug.Log($"두 번째 선택 확정: {currentSelection2}");
                Combination(); // 두 번째 선택 확정 후 조합 실행
                isFirstSelect = true; // 첫 번째 선택으로 초기화
            }
        }

        // 마우스 휠 입력 처리 (Space바를 누르고 있는 동안)
        if (isSelecting)
        {
            float scrollValue = Mouse.current.scroll.ReadValue().y;
            if (scrollValue > 0)
            {
                if (isFirstSelect)
                {
                    currentSelection1 = currentSelection1 >= 3 ? 1 : currentSelection1 + 1;
                    Debug.Log($"현재 첫 번째 선택 변경: {currentSelection1}");
                }
                else
                {
                    currentSelection2 = currentSelection2 >= 3 ? 1 : currentSelection2 + 1;
                    Debug.Log($"현재 두 번째 선택 변경: {currentSelection2}");
                }
            }
            else if (scrollValue < 0)
            {
                if (isFirstSelect)
                {
                    currentSelection1 = currentSelection1 <= 1 ? 3 : currentSelection1 - 1;
                    Debug.Log($"현재 첫 번째 선택 변경: {currentSelection1}");
                }
                else
                {
                    currentSelection2 = currentSelection2 <= 1 ? 3 : currentSelection2 - 1;
                    Debug.Log($"현재 두 번째 선택 변경: {currentSelection2}");
                }
            }
        }

        // 캐릭터 스프라이트 방향 설정
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(UnityEngine.Camera.main.transform.position.z))
        );

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

    //select1값을 가져오는 메소드
    public int GetSelect1()
    {
        return currentSelection1;
    }

    //select2값을 가져오는 메소드
    public int GetSelect2()
    {
        return currentSelection2;
    }


    private void Combination()
    {
        Dictionary<(int, int), string> combinations = new Dictionary<(int, int), string>
        {
            { (1, 1), "water"}, // 물+물
            { (2, 2), "treeVine" }, // 풀+풀
            { (3, 3), "rockBomb" }, // 바위+바위
            { (1, 2), "potion" }, // 물+풀
            { (2, 1), "potion" }, // 풀+물
            { (2, 3), "platform" }, // 풀+바위
            { (3, 2), "platform" }, // 바위+풀
            { (1, 3), "bullet" }, // 물+바위
            { (3, 1), "bullet" }  // 바위+물
        };

        var selectedCombination = (currentSelection1, currentSelection2);

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
        moveDirection = value.Get<Vector2>();
    }

    // 점프 입력 처리
    public void OnJump(InputValue value)
    {
        if (value.isPressed && !isJump)
        {
            isJump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeed);
        }
    }

    // 대쉬 입력 처리
    public void OnSprint(InputValue value)
    {
        if (value.isPressed && !isDashing && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash()); // 대쉬 코루틴 실행
        }
    }

    // 이동 로직 처리
    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y); // 이동 속도 적용
    }   

    // 대쉬 로직 처리
    private IEnumerator Dash()
    {
        isDashing = true; // 대쉬 플래그 설정
        Vector2 dashForce = new Vector2(moveDirection.x * dashSpeed, 0); // 대쉬 방향 설정
        rb.AddForce(dashForce, ForceMode2D.Impulse); // 대쉬 힘 적용
        yield return new WaitForSeconds(dashDuration); // 대쉬 지속 시간 대기
        isDashing = false; // 대쉬 플래그 해제
        dashCooldownTimer = dashCooldown; // 대쉬 쿨타임 설정
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false;
        }
    }
}
