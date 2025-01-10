using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : TopDownCharacterController
{
    SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;

    private Rigidbody2D rb;
    private Vector2 moveDirection; //이동 방향
    private bool isJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void Update()
    {
        // 새로운 Input System을 사용하여 마우스 위치 가져오기
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 0;

        // 마우스 위치와 캐릭터의 위치 차이
        float direction = mousePos.x - transform.position.x;
        Debug.Log(direction);
        // 마우스가 왼쪽에 있으면 flipX를 true로 설정, 오른쪽에 있으면 false
        if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        //이동 방향을 입력값으로
        moveDirection = _context.ReadValue<Vector2>();  
    }
    public void OnJump(InputAction.CallbackContext _context)
    {
        //점프 입력이 수행된 상태이고, 플레이어가 점프 중이 아닐 때
        if(_context.phase == InputActionPhase.Performed && !isJump)
        {
            isJump = true;
            //위쪽 방향으로 힘을 가해서 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
    }

/*    public void OnLook(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Performed)
        {
            // 입력값에서 마우스 포지션을 가져옴
            Vector2 mousePosition = _context.ReadValue<Vector2>();

            // 스크린 좌표를 월드 좌표로 변환
            mousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);

            // 캐릭터 위치를 기준으로 방향 계산
            Vector2 direction = mousePosition - (Vector2)transform.position;

            // 캐릭터가 해당 방향을 바라보도록 설정
            Look(direction);
        }
    }

    private void Look(Vector2 direction)
    {
        // x축 방향이 양수면 오른쪽, 음수면 왼쪽
        if (direction.x > 0)
        {
            characterSprite.flipX = false; // 오른쪽을 바라봄
        }
        else if (direction.x < 0)
        {
            characterSprite.flipX = true; // 왼쪽을 바라봄
        }
    }*/


    /*    //플레이어 이동 처리
        private void Move()
        {
            //이동 속도 계산
            Vector2 velocity = moveDirection * moveSpeed;
            //속도 설정
            rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        }*/

    private void ApplyMovement()
    {
        // Rigidbody를 이용한 캐릭터 이동
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = false;
            Debug.Log("Jump");
        }
    }

}
