using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float dashSpeed = 20f; // 대쉬 속도
    [SerializeField] private float dashDuration = 0.2f; // 대쉬 지속 시간
    [SerializeField] private float dashCooldown = 1f; // 대쉬 쿨타임

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isJump;
    private bool isDashing; // 대쉬 상태 확인
    private float dashCooldownTimer; // 대쉬 쿨타임 타이머

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isDashing) // 대쉬 중이 아니면 일반 이동 처리
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

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !isJump)
        {
            isJump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
    }

    public void OnSprint(InputValue value)
    {
        if (value.isPressed && !isDashing && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash());
        }
    }
    //Q
    public void OnSelect1(InputValue value)
    {
        Debug.Log("Q");
    }

    //E
    public void OnSelect2(InputValue value)
    {
        Debug.Log("E");

    }

    //좌클릭 흡수
    public void OnAbsorb(InputValue value)
    {
        Debug.Log("흡수");
    }

    //우클릭 흡수
    public void OnEmit(InputValue value)
    {
        Debug.Log("방출");

    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }
    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashForce = new Vector2(moveDirection.x * dashSpeed, 0);
        rb.AddForce(dashForce, ForceMode2D.Impulse); // 대쉬에 Impulse 사용
        yield return new WaitForSeconds(dashDuration); // 대쉬 지속 시간
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false;
        }
    }
}
