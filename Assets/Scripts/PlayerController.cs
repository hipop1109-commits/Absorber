using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 10f; //이동 속도
    [SerializeField] private float jumpSpeed = 10f; //점프 속도
    [SerializeField] private float dashSpeed = 30f; // 대쉬 속도
    [SerializeField] private float dashDuration = 0.2f; // 대쉬 지속 시간
    [SerializeField] private float dashCooldown = 1f; // 대쉬 쿨타임
    private int a = 1;

    private Rigidbody2D rb;
    private float prevVx = 0;
    public float vx = 0;
    private bool isGround;
    public int trash = 1;

    void Start()
    {
        a = 10;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }

    }

}
