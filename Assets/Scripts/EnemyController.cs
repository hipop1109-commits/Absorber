using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; //적 이동 속도
    [SerializeField] private int hp = 20;
    [SerializeField] private int damage = 10;

    [SerializeField] private float groundCheckRadius = 0.1f; //바닥 감지 반경

    [SerializeField] private LayerMask groundLayer; //ground태그가 있는 레이어 설정


    [SerializeField] private Collider2D frontCollider;
    [SerializeField] private Collider2D frontBottomCollider;

    [SerializeField] private PlayerController playerController;

    Vector2 vx;

    private void Start()
    {
        vx = Vector2.left * moveSpeed;
        Debug.Log("적 체력 : "+hp);
    }

    private void Update()
    {
/*        //벽감지
        bool isTouchingWall = Physics2D.OverlapCircle(frontCollider.transform.position, groundCheckRadius, groundLayer);

        //바닥감지
        bool isGrounded = Physics2D.OverlapCircle(frontBottomCollider.transform.position, groundCheckRadius, groundLayer);

        if(isTouchingWall || !isGrounded) 
        {
            vx = -vx;
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        }*/
    }

    private void FixedUpdate()
    {
       // transform.Translate(vx * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 데미지 입히기
            playerController.TakeDamage(10);
            Debug.Log("Hurt");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Attack attack = collision.GetComponent<Attack>();
            if(attack != null)
            {
                TakeDamage(attack.damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"적{gameObject} 체력 : {hp}"); 
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
