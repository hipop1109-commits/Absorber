using UnityEngine;
using System.Collections;

public class EnemyAttackRange : MonoBehaviour
{
    private BaseEnemy enemy;

    public EnemyStateMachine a_stateMachine; // 적의 상태를 관리할 스테이트 머신

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    void Start()
    {
        // enemyController 할당
        enemy = GetComponentInParent<BaseEnemy>();

        // 스테이트 머신 초기화
        a_stateMachine = enemy.stateMachine;

        //공격 루틴 시작
        StartCoroutine(AttackLoop());
    }

    // 플레이어가 감지 범위에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemy.isDie == false)
        {
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
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            if(enemy.isDie == false && isPlayerInRange)
            {
                a_stateMachine.TransitionTo(a_stateMachine.attackState);
            }
        }
    }
        
}
