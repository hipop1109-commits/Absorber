using UnityEngine;
using System.Collections;

public class EnemyAttackRange : MonoBehaviour
{
    private EnemyMove enemyMove; // EnemyMove 스크립트 참조
    private EnemyController enemyController;

    [SerializeField] private EnemyTrace enemyTrace; // EnemyTrace 스크립트 참조
    
    public EnemyStateMachine a_stateMachine; // 적의 상태를 관리할 스테이트 머신
    
    private bool isAttacking = false; // 공격 상태 관리
    
    void Awake()
    {
    }

    void Start()
    {
        // enemyController 할당
        enemyController = GetComponentInParent<EnemyController>();

        // enemyMove 할당
        enemyMove = GetComponentInParent<EnemyMove>();
        
        // 스테이트 머신 초기화
        a_stateMachine = enemyController.stateMachine;
    }

    void Update()
    {

    }

    // 플레이어가 감지 범위에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttacking) // "Player" 태그로 플레이어를 감지
        {
            StartCoroutine(AttackPlayer());
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 상태를 idle로 전환
            a_stateMachine.TransitionTo(a_stateMachine.idleState);
            Debug.Log("공격중지");
            
            enemyTrace.enabled = true;
            enemyMove.enabled = true; // EnemyMove 활성화
            
            // 공격 중단 플래그 초기화
            isAttacking = false;
        }
    }

    // 코루틴으로 일정 시간 후 재공격 처리
    private IEnumerator AttackPlayer()
    {
        isAttacking = true; // 공격 중 상태로 전환
            Debug.Log("공격!");

        // 상태를 attack으로 전환
        a_stateMachine.TransitionTo(a_stateMachine.attackState);
        
        enemyMove.enabled = false; // EnemyMove 비활성화
        enemyTrace.enabled = false;

        // 3초 대기
        yield return new WaitForSeconds(3f);

        // 상태를 idle 전환
        a_stateMachine.TransitionTo(a_stateMachine.idleState);

        isAttacking = false; // 공격 가능 상태로 전환
    }
}
