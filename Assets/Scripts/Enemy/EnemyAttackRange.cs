using UnityEngine;
using System.Collections;

public class EnemyAttackRange : MonoBehaviour
{
    private EnemyMove enemyMove; // EnemyMove 스크립트 참조
    private EnemyController enemyController;

    public EnemyStateMachine a_stateMachine; // 적의 상태를 관리할 스테이트 머신

    private bool isAttacking = false; // 공격 상태 관리
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    void Start()
    {
        // enemyController 할당
        enemyController = GetComponentInParent<EnemyController>();

        // enemyMove 할당
        enemyMove = GetComponentInParent<EnemyMove>();

        // 스테이트 머신 초기화
        a_stateMachine = enemyController.stateMachine;
    }

    // 플레이어가 감지 범위에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttacking)
        {
            isPlayerInRange = true;
            Debug.Log("공격");
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

            enemyMove.enabled = true; // EnemyMove 활성화

            // 상태 초기화
            isAttacking = false;
            isPlayerInRange = false; // 플레이어 범위 플래그 초기화
        }
    }

    // 코루틴으로 일정 시간 후 재공격 처리
    private IEnumerator AttackPlayer()
    {
        isAttacking = true; // 공격 중 상태로 전환

        // 상태를 attack으로 전환
        a_stateMachine.TransitionTo(a_stateMachine.attackState);

        // 공격 중 이동 및 추적 비활성화
        enemyMove.enabled = false;

        // 애니메이션이 끝난 후 상태를 idle로 전환
        yield return new WaitForSeconds(1f); // 애니메이션 지속 시간(1초)
        a_stateMachine.TransitionTo(a_stateMachine.idleState);

        // 이동 및 추적 활성화
        enemyMove.enabled = true;
        // 3초 대기 후 다음 공격 가능 여부 확인
        yield return new WaitForSeconds(3f);

        if (isPlayerInRange)
        {
            Debug.Log("다시 공격 준비");
            isAttacking = false; // 다음 공격을 허용
            StartCoroutine(AttackPlayer()); // 다시 공격
        }
        else
        {
            isAttacking = false; // 공격 상태 해제
        }
    }
}
