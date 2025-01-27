using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadiloPattern : MonoBehaviour
{
    private enum BossState { Idle, Moving, Attacking }
    private enum RangeState { Out, Far, Close, Air, Back}

    private BossState currentState = BossState.Idle;
    private RangeState rangeState = RangeState.Out;

    private Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
    private bool isExecutingAction = false;
    private bool isCooldown = false;

    [SerializeField] private GameObject CloseRange;
    [SerializeField] private GameObject FarRange;
    [SerializeField] private GameObject AirRange;
    [SerializeField] private GameObject BackRange;

    [SerializeField] private GameObject[] StompEffect;
    [SerializeField] private float effectDuration = 0.33f;

    [SerializeField] private GameObject IceJumpEffect;

    [SerializeField] private GameObject spinePrefab; // 가시 프리팹
    [SerializeField] private Transform[] spawnPoints; // 가시 발사 지점
    [SerializeField] private float fireRate = 0.5f; // 가시 발사 간격
    [SerializeField] private int totalSpines = 5;

    private Animator ani;
    private bool isTransitioning = false;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float cooldownTime = 3f;
    public PlayerController player;
    public EnemyStateMachine a_stateMachine;
    public EnemyStateMachine c_stateMachine;
    public EnemyStateMachine f_stateMachine;

    [SerializeField] private GameObject PlayerEnter;

    private void Awake()
    {
    }
    private void Start()
    {
        if (gameObject.CompareTag("ArmadilloBoss")) // 이름이 "Armadillo"인 경우
        {
            a_stateMachine = GetComponent<BaseEnemy>().stateMachine;
        }
        else if (gameObject.CompareTag("CatapillarBoss")) // 이름이 "Armadillo"인 경우
        {
            c_stateMachine = GetComponent<BaseEnemy>().stateMachine;
        }
        else if (gameObject.CompareTag("FrogBoss")) // 이름이 "Armadillo"인 경우
        {
            f_stateMachine = GetComponent<BaseEnemy>().stateMachine;
        }
        playerTransform = player.transform;
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        currentState = BossState.Idle;
        rangeState = RangeState.Out;
        StartCoroutine(ProcessActions());

        foreach (var effect in StompEffect)
        {
            effect.SetActive(false);
        }

        IceJumpEffect.SetActive(false);
    }

   
    private void FixedUpdate()
    {
        if (currentState == BossState.Moving || rangeState == RangeState.Back)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (rangeState == RangeState.Back || rangeState == RangeState.Far || rangeState == RangeState.Air)
        {
            // 플레이어를 따라가며 방향 전환
            direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // 보스의 방향 전환
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-12, 12, 1); // 오른쪽 보기
            }
            else
            {
                transform.localScale = new Vector3(12, 12, 1); // 왼쪽 보기
            }
        }
    }

  
    public void ActivateMovingState()
    {
        currentState = BossState.Moving;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCooldown)
        {
            if (collision.IsTouching(CloseRange.GetComponent<Collider2D>()))
            {
                EnqueueAction(CloseAttack());
                rangeState = RangeState.Close;
            }
            else if (collision.IsTouching(FarRange.GetComponent<Collider2D>()))
            {
                EnqueueAction(FarAttack());
                rangeState = RangeState.Far;
            }
            else if (collision.IsTouching(AirRange.GetComponent<Collider2D>()))
            {
                EnqueueAction(AirAttack());
                rangeState = RangeState.Air;
            }
            else if (collision.IsTouching(BackRange.GetComponent<Collider2D>()))
            {
                Debug.Log("뒷방향");
                rangeState = RangeState.Back;
            }
            else
            {
                rangeState = RangeState.Out; // 범위 밖
            }
        }
    }

    private IEnumerator ProcessActions()
    {
        while (true)
        {
            if (actionQueue.Count > 0 && !isExecutingAction && !isCooldown)
            {
                var action = actionQueue.Dequeue();
                yield return StartCoroutine(action);
            }
            yield return null;
        }
    }


    private void EnqueueAction(IEnumerator action)
    {
        if (!isExecutingAction && !isCooldown)
        {
            actionQueue.Enqueue(action);
        }
    }

    private IEnumerator CloseAttack()
    {
        isExecutingAction = true;
        currentState = BossState.Attacking;
        rb.linearVelocity = Vector2.zero;
        // 근거리 공격 애니메이션 실행
        if (gameObject.CompareTag("ArmadilloBoss")) // 이름이 "Armadillo"인 경우
        {
            Stomp();
        }
        else if (gameObject.CompareTag("CatapillarBoss")) // 특정 태그로 구분
        {
            IceStomp();
        }
        else if (gameObject.CompareTag("FrogBoss")) // 특정 태그로 구분
        {
            FrogTounge();
        }
        yield return new WaitForSeconds(2f); // 공격 시간
        StartCooldown();
    }

    private IEnumerator FarAttack()
    {
        isExecutingAction = true;
        currentState = BossState.Attacking;
        rb.linearVelocity = Vector2.zero;
        // 먼 거리 공격 애니메이션 실행
        if (gameObject.CompareTag("ArmadilloBoss"))// 이름이 "Armadillo"인 경우
        {
            Angry();
        }
        else if (gameObject.CompareTag("CatapillarBoss")) // 특정 태그로 구분
        {
            IceDrop();
        }
        else if (gameObject.CompareTag("FrogBoss")) // 특정 태그로 구분
        {
            FrogJump();
        }
        yield return new WaitForSeconds(2f); // 공격 시간
        StartCooldown();
    }

    private IEnumerator AirAttack()
    {
        isExecutingAction = true;
        currentState = BossState.Attacking;
        rb.linearVelocity = Vector2.zero;
        // 공중 공격 애니메이션 실행
        if (gameObject.CompareTag("ArmadilloBoss")) // 이름이 "Armadillo"인 경우
        {
            Spine();
        }
        else if (gameObject.CompareTag("CatapillarBoss")) // 특정 태그로 구분
        {
            IceJump();
        }
        else if (gameObject.CompareTag("FrogBoss")) // 특정 태그로 구분
        {
            FrogSpwan();
        }
        yield return new WaitForSeconds(2f); // 공격 시간
        StartCooldown();
    }

    private void StartCooldown()
    {
        isExecutingAction = false; // 현재 행동 종료
        if(rangeState == RangeState.Far || rangeState == RangeState.Air)
        {
            currentState = BossState.Moving;
        }
        else 
        { 
            currentState = BossState.Idle; 
        }
       
        StartCoroutine(Cooldown()); // 쿨타임 시작
    }


    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    
    private void Stomp()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_StompState);
        StartCoroutine(PlayStompEffects());
    }

    private void Angry()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_AngryState);
    }


    private void Spine()
    {
        a_stateMachine.TransitionTo(a_stateMachine.a_SpineState);
        StartCoroutine(SpineAttackRoutine());
    }

    private IEnumerator PlayStompEffects()
    {
        yield return new WaitForSeconds(2.35f);
        foreach (var effect in StompEffect)
        {
            // 이펙트 활성화
            effect.SetActive(true);

            // 설정한 시간 동안 활성 상태 유지
            yield return new WaitForSeconds(effectDuration);

            // 이펙트 비활성화
            yield return new WaitForSeconds(effectDuration); // 효과 지속 시간
            effect.SetActive(false); // 효과 비활성화
            yield return new WaitForSeconds(0.2f);
        }
    }

        private IEnumerator SpineAttackRoutine()
    {
        for (int i = 0; i < totalSpines; i++)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                // 가시 생성
                GameObject spine = Instantiate(spinePrefab, spawnPoint.position, spawnPoint.rotation);
                spine.transform.right = spawnPoint.right; // 발사 방향 설정
            }

            yield return new WaitForSeconds(fireRate); // 발사 간격 대기
        }

    }

    private IEnumerator IceJumpEffects()
    {
            IceJumpEffect.SetActive(true);
            yield return new WaitForSeconds(1.5f); // 효과 지속 시간
            IceJumpEffect.SetActive(false); // 효과 비활성화
    }

    private IEnumerator IceDropEffects()
    {
        IceJumpEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f); // 효과 지속 시간
        IceJumpEffect.SetActive(false); // 효과 비활성화
    }

    private void IceStomp()
    {
        c_stateMachine.TransitionTo(c_stateMachine.c_StompState);
    }

    private void IceDrop()
    {
        c_stateMachine.TransitionTo(c_stateMachine.c_DropState);
        StartCoroutine(IceDropEffects());
    }

    private void IceJump()
    {
        c_stateMachine.TransitionTo(c_stateMachine.c_JumpState);
        StartCoroutine(IceJumpEffects());
    }
    
    
    private void FrogJump()
    {

    }

    private void FrogTounge()
    {

    }

    private void FrogSpwan()
    {

    }
 }
