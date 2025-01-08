using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpSpeed = 5;
    public Collider2D bottomCollider;
    public Collider2D terrainCollider;

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
        float vy = GetComponent<Rigidbody2D>().linearVelocityY;

        if (vx < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (vx > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        //땅에 닿아있는가
        if (bottomCollider.IsTouching(terrainCollider))
        {
            if (!isGround)
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
                if (vx != prevVx)
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
            }
        }
        else
        {
            if (isGround)
            {
                GetComponent<Animator>().SetTrigger("Jump");
            }
        }

        //플레이어 발이 바닥 콜라이더와 닿았다면
        isGround = bottomCollider.IsTouching(terrainCollider);

        if (Input.GetButtonDown("Jump") && isGround == true)
        {
            vy = jumpSpeed;
        }
        prevVx = vx;

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(vx, vy);
    }
}
