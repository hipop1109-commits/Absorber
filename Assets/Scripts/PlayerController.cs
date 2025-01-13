using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpSpeed = 5;
    public Collider2D bottomCollider;

    private Rigidbody2D rb;
    private float prevVx = 0;
    private float vx = 0;
    private bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vx = Input.GetAxisRaw("Horizontal") * speed;
        float vy = rb.linearVelocity.y;

        if (vx < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (vx > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (isGround)
        {
            if (vx == 0)
            {
                GetComponent<Animator>().SetTrigger("Idle");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            vy = jumpSpeed;
        }

        prevVx = vx;

        rb.linearVelocity = new Vector2(vx, vy);
    }

    // 바닥에 닿았는지
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("TESt");
            isGround = true;
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
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }

    }

}
