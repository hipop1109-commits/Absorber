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
        float vy = rb.velocity.y;

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

        rb.velocity = new Vector2(vx, vy);
    }

    // 바닥에 닿았는지 확인
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }
}
