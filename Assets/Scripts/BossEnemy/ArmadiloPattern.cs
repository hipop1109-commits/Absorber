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
    private float tackleSpeed = 40f; // 돌진 속도
    private float tackleDuration = 0.6f; // 돌진 지속 시간
    private float tackleTimer = 0f; // 돌진 타이머

    public PlayerController player;
    public EnemyStateMachine a_stateMachine;

    private void Awake()
    {
        a_stateMachine = GetComponent<EnemyController>().stateMachine;
    }
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
        if (isTransitioning == false)
        {
            isTransitioning = true;
            Stomp();
            yield return new WaitForSeconds(5);
            isInCloseRange = false;
            isTransitioning = false;
            
        }
    }

    private IEnumerator FarAttack()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;
            StartCoroutine(TackleRoutine());
            yield return new WaitForSeconds(5);
            isInFarRange = false;
            isTransitioning = false;
           
        }
    }

    private IEnumerator AirAttack()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;
            SpineAttack();
            yield return new WaitForSeconds(5);
            isInFarRange = false;
            isTransitioning = false;
           
        }
    }

    private void Stomp()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_StompState);
    }

    
    private IEnumerator TackleRoutine()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_AngryState);
        yield return new WaitForSeconds(1f); // 애니메이션 시작 후 0.4초 대기

        // 돌진 설정
        isTackling = true;
        tackleDirection = (player.transform.position - transform.position).normalized;
        tackleTimer = tackleDuration; // 돌진 지속 시간 설정

        yield return new WaitForSeconds(tackleDuration + 1f); // 돌진 종료 후 대기
        isTackling = false;
        
    }


    private void SpineAttack()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_SpineState);
    }


}
