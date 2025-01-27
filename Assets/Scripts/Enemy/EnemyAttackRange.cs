using UnityEngine;
using System.Collections;

public class EnemyAttackRange : MonoBehaviour
{
    private BaseEnemy enemy;
    private FlyEnemy flyEnemy; // FlyEnemy 참조 변수 추가

    public EnemyStateMachine a_stateMachine; // 적의 상태를 관리할 스테이트 머신

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    void Start()
    {
        // enemyController 할당
        enemy = GetComponentInParent<BaseEnemy>();

        // FlyEnemy가 있다면 참조
        flyEnemy = GetComponentInParent<FlyEnemy>();

        // 스테이트 머신 초기화
        a_stateMachine = enemy.stateMachine;

        // 공격 루틴 시작
        StartCoroutine(AttackLoop());
    }

    // 플레이어가 감지 범위에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemy.isDie == false)
        {
            Debug.Log($"{transform.parent.gameObject}의 공격 범위 내");
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; // 플레이어 범위 플래그 초기화
        }
    }

    // 코루틴으로 일정 시간 후 재공격 처리
    private IEnumerator AttackLoop()
    {
        while(true)
        {
            // 일정 시간 대기 (랜덤하게 3초에서 5초 사이)
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            if(enemy.isDie == false)
            {
                // 공격 상태로 전환
                a_stateMachine.TransitionTo(a_stateMachine.attackState);

                // FlyEnemy일 경우에만 공격 후 idle로 전환
                if (flyEnemy != null)
                {
                    yield return new WaitForSeconds(1f); // 공격 애니메이션 지속 시간 (1초)
                    a_stateMachine.TransitionTo(a_stateMachine.idleState); // FlyEnemy만 idle로 전환
                }
            }
        }
    }
}
