using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rb;
    public int nextMove;
    Animator animator;
    public LayerMask groundLayer; // 발판을 감지할 레이어

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Think();

        //딜레이 주기
        Invoke("Think", 5);
    }

    // 물리 기반이기 때문에 FixedUpdate 사용
    void FixedUpdate()
    {
        // 옆으로 움직이게
        rb.linearVelocity = new Vector2(nextMove, rb.linearVelocity.y);

        // 발 아래 발판 감지
        if (!IsGroundAhead())
        {
            Turn(); // 낭떠러지 감지 시 방향 전환
        }
    }

    // 발 아래 발판 감지
    bool IsGroundAhead()
    {
        // 발 앞부분에 레이캐스트 발사
        Vector2 origin = new Vector2(rb.position.x + nextMove * 0.5f, rb.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);

        // 디버그용 레이 시각화
        Debug.DrawRay(origin, Vector2.down * 1f, Color.green);

        // 발판이 감지되면 true 반환
        return hit.collider != null;
    }

    //왼쪽인지 오른쪽인지 판단
    void Think()
    {
        // -1 또는 1만 랜덤으로 설정
        nextMove = Random.Range(0, 2) == 0 ? -1 : 1;

        // 애니메이션 설정
        animator.SetInteger("WalkSpeed", nextMove);

        // 방향 전환
        if (nextMove == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽
        else
            transform.rotation = Quaternion.Euler(0, 180, 0); // 왼쪽

        // 다음 Think 호출 예약
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        // 방향 전환만 수행
        nextMove *= -1;

        if (nextMove == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽
        else
            transform.rotation = Quaternion.Euler(0, 180, 0); // 왼쪽
    }
}
