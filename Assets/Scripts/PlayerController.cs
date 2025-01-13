using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 10f; //이동 속도
    [SerializeField] private float jumpSpeed = 10f; //점프 속도
    [SerializeField] private float dashSpeed = 30f; // 대쉬 속도
    [SerializeField] private float dashDuration = 0.2f; // 대쉬 지속 시간
    [SerializeField] private float dashCooldown = 1f; // 대쉬 쿨타임

    private Rigidbody2D rb;
    private Vector2 moveDirection; //이동 방향

    private bool isDash;
    private bool isJump;

    private float dashCooldownTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isDash) // 대쉬 중이 아니면 일반 이동 처리
        {
            ApplyMovement();
        }
    }

    private void Update()
    {
        // 마우스 위치를 받아와 캐릭터의 방향을 설정
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(UnityEngine.Camera.main.transform.position.z))
        );

        // 캐릭터 Sprite 중심을 기준으로 비교
        if (mouseWorldPosition.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // 오른쪽
        }
        else
        {
            spriteRenderer.flipX = true; // 왼쪽
        }

        //대쉬 쿨타임 타이머 업데이트
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void OnMove(InputValue value)
    {
        // 이동 방향을 입력값으로 설정
        moveDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // 점프 입력이 수행된 상태이고, 플레이어가 점프 중이 아닐 때
        if (value.isPressed && !isJump)
        {
            isJump = true;
            // 위쪽 방향으로 힘을 가해서 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
    }

    public void OnSprint(InputValue value)
    {
        if(value.isPressed && !isDash && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash());
        }
    }


    private void ApplyMovement()
    {
        // Rigidbody를 이용한 캐릭터 이동
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    IEnumerator Dash()
    {
        isDash = true; //대쉬 상태 설정
        float originalMoveSpeed = moveSpeed; //기존 속도 설정
        moveSpeed = dashSpeed; //대쉬 속도로 설정

        rb.linearVelocity = new Vector2(moveDirection.x * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration); //대쉬 지속 시간만큼 대기

        moveSpeed = originalMoveSpeed; //기존 속도로 복원
        isDash = false; //대쉬 상태 해제
        dashCooldownTimer = dashCooldown; //쿨타임 설정
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false;
        }
    }

}
