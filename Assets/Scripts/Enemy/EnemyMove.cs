using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] private int nextMove;
    [SerializeField] private float raycastLength = 2f;
    [SerializeField] private float raycastHorizontalLength = 0.5f;

    public LayerMask groundLayer; // 발판을 감지할 레이어

    public EnemyStateMachine e_stateMachine; // 적의 상태를 관리할 스테이트 머신


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 스테이트 머신 초기화
        e_stateMachine = GetComponent<EnemyController>().stateMachine;

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
        Vector2 origin = new Vector2(rb.position.x + nextMove * raycastHorizontalLength, rb.position.y);

        //raycastLength 조정하면 보스나 일반몹이 인지하는 ray 달라짐
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastLength, groundLayer);

        // 디버그용 레이 시각화
        Debug.DrawRay(origin, Vector2.down * raycastLength, Color.green);

        // 발판이 감지되면 true 반환
        return hit.collider != null;
    }

    //왼쪽인지 오른쪽인지 판단
    void Think()
    {
        // -1 또는 1만 랜덤으로 설정
        nextMove = Random.Range(0, 2) == 0 ? -1 : 1;

        // 애니메이션 설정
        e_stateMachine.TransitionTo(e_stateMachine.walkState);

        // 방향 전환
        if (nextMove == 1)
            transform.rotation = Quaternion.Euler(0, 180, 0); // 왼쪽
        else
            transform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽

        // 다음 Think 호출 예약
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        // 방향 전환만 수행
        nextMove *= -1;

        if (nextMove == 1)
            transform.rotation = Quaternion.Euler(0, 180, 0); // 왼쪽
        else
            transform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽
    }
}
