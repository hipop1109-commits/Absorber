using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    [SerializeField] private Transform player; // 플레이어 Transform
    private Transform enemyTransform; // 부모 Enemy Transform 참조

    [SerializeField] private float moveSpeed = 2.5f; // 추적 이동 속도

    private EnemyMove enemyMove; // EnemyMove 스크립트 참조
    public bool isFollowing = false; // 추적 여부

    void Awake()
    {
        enemyMove = GetComponentInParent<EnemyMove>();
        enemyTransform = transform.parent;
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            FollowPlayer(); // 플레이어 추적
        }
    }

    // 플레이어를 추적하는 로직
    void FollowPlayer()
    {
        float direction = player.position.x - enemyTransform.position.x;
        int nextMove = direction > 0 ? -1 : 1; // 플레이어가 오른쪽에 있으면 -1, 왼쪽에 있으면 1

        enemyTransform.Translate(Vector2.right * nextMove * moveSpeed * Time.deltaTime);

        // 방향 전환
        if (nextMove == 1)
            enemyTransform.rotation = Quaternion.Euler(0, 180, 0); // 왼쪽
        else
            enemyTransform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽
    }
    // 플레이어가 감지 범위에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그로 플레이어를 감지
        {
            isFollowing = true; // 추적 시작
            Debug.Log("추적중");
            enemyMove.enabled = false; // EnemyMove 비활성화
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFollowing = false; // 추적 중지
            Debug.Log("추적중지");
            enemyMove.enabled = true; // EnemyMove 활성화
        }
    }
}
