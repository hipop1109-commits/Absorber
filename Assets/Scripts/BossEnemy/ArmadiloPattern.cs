using System.Collections;
using UnityEngine;

public class ArmadiloPattern : MonoBehaviour
{
    private bool isInCloseRange = false;
    private bool isInFarRange = false;
    private bool isInAirRange = false;

    [SerializeField] private GameObject CloseRange;
    [SerializeField] private GameObject FarRange;
    [SerializeField] private GameObject AirRange;

    private Animator ani;
    private bool isTransitioning = false;

    private Rigidbody2D rb;
    private bool isTackling = false; // 돌진 상태 관리
    private Vector2 tackleDirection; // 돌진 방향
    private float tackleSpeed = 30f; // 돌진 속도
    private float tackleDuration = 1.5f; // 돌진 지속 시간
    private float tackleTimer = 0f; // 돌진 타이

    private void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isTackling)
        {
            // 돌진 중이면 Rigidbody2D에 속도 적용
            rb.linearVelocity = tackleDirection * tackleSpeed;

            // 타이머 업데이트
            tackleTimer -= Time.fixedDeltaTime;

            if (tackleTimer <= 0f)
            {
                // 돌진 종료
                rb.linearVelocity = Vector2.zero; // 속도 초기화
                isTackling = false; // 돌진 상태 해제
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("감지1");

            if (!isTransitioning)
            {
                if (collision.IsTouching(CloseRange.GetComponent<Collider2D>()))
                {
                    if (!isInCloseRange)
                    {
                        Debug.Log("감지2: 가까운 거리");
                        isInCloseRange = true;
                        StartCoroutine(CloseAttack());
                    }
                    isInFarRange = false;
                }

                // FarRange의 영역 안에 플레이어가 있는지 확인
                else if (collision.IsTouching(FarRange.GetComponent<Collider2D>()))
                {
                    if (!isInFarRange)
                    {
                        Debug.Log("감지3: 먼 거리");
                        isInFarRange = true;
                        StartCoroutine(FarAttack());
                    }
                }

                else if (collision.IsTouching(AirRange.GetComponent<Collider2D>()))
                {
                    if (!isInAirRange)
                    {
                        Debug.Log("감지3: 공중 거리");
                        isInAirRange = true;
                        StartCoroutine(AirAttack());
                    }
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision == CloseRange.GetComponent<Collider2D>())
            {
                Debug.Log("감지2");
                isInCloseRange = false;
            }

            // FarRange의 콜라이더와 비교
            if (collision == FarRange.GetComponent<Collider2D>())
            {
                Debug.Log("감지3");
                isInFarRange = false;
            }

            if (collision == AirRange.GetComponent<Collider2D>())
            {
                Debug.Log("감지3");
                isInAirRange = false;
            }
        }
    }

    private IEnumerator CloseAttack()
    {
       
            isTransitioning = true;
            Stomp();
            yield return new WaitForSeconds(5);
            isInCloseRange = false;
            isTransitioning = false;
        ani.SetTrigger("Idle");

    }

    private IEnumerator FarAttack()
    {
        isTransitioning = true;
            Tackle();
            yield return new WaitForSeconds(5);
            isInFarRange = false;
        isTransitioning = false;
        ani.SetTrigger("Idle");

    }

    private IEnumerator AirAttack()
    {
        isTransitioning = true;
        SpineAttack();
        yield return new WaitForSeconds(5);
        isInFarRange = false;
        isTransitioning = false;
        ani.SetTrigger("Idle");

    }

    private void Stomp()
    {
        ani.SetTrigger("Stomp");
    }

    private void Tackle()
    {
        StartCoroutine(TackleRoutine());
        ani.SetTrigger("Idle");

    }

    private IEnumerator TackleRoutine()
    {
        ani.SetTrigger("Angry");
        yield return new WaitForSeconds(1f); // 애니메이션 시작 후 0.4초 대기

        // 돌진 설정
        isTackling = true;
        tackleDirection = Vector2.left; // 돌진 방향 설정 (예: 오른쪽으로 돌진)
        tackleTimer = tackleDuration; // 돌진 지속 시간 설정

        yield return new WaitForSeconds(tackleDuration + 0.4f); // 돌진 종료 후 대기
        isTackling = false;
        
    }


    private void SpineAttack()
    {
        ani.SetTrigger("Spine");
    }


}
