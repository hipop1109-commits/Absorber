using UnityEngine;
using System.Collections;

public class EnemyAttackRange : MonoBehaviour
{
    private BaseEnemy enemy; // 적의 기본 정보를 관리하는 BaseEnemy 클래스
    private FlyEnemy flyEnemy; // FlyEnemy 클래스에 대한 참조를 추가
    private MovingEnemy movingEnemy;

    public EnemyStateMachine a_stateMachine; // 적의 상태를 관리하는 상태 머신

   // private bool isPlayerInRange = false; // 플레이어가 공격 범위에 있는지 확인하는 변수

    void Start()
    {
        // BaseEnemy 클래스 가져오기
        enemy = GetComponentInParent<BaseEnemy>();

        // FlyEnemy 클래스가 존재하면 가져오기
        flyEnemy = GetComponentInParent<FlyEnemy>();

        movingEnemy = GetComponentInParent<MovingEnemy>();

        // 상태 머신 초기화
        a_stateMachine = enemy.stateMachine;

        // 공격 루프 코루틴 시작
        StartCoroutine(AttackLoop());
    }

    // 플레이어가 공격 범위 안에 들어왔는지 확인
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemy.isDie == false)
        {
            Debug.Log($"{transform.parent.gameObject}이(가) 공격 범위에 플레이어를 감지했습니다.");
           // isPlayerInRange = true;
        }
    }

/*    // 플레이어가 공격 범위를 벗어났을 때 호출
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; // 플레이어 범위 초기화
        }
    }*/

    // 반복적인 공격 동작을 처리하는 코루틴
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            // 공격 대기 시간 (3초에서 5초 사이 랜덤)
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            if (enemy.isDie == false)
            {
                // 공격 상태로 전환
                a_stateMachine.TransitionTo(a_stateMachine.attackState);

                // movingEnemy 경우 공격 후 1초 후 walk 상태로 전환
                if (movingEnemy != null)
                {
                    yield return new WaitForSeconds(1.5f); // 공격 애니메이션 시간 대기 (1초)
                    a_stateMachine.TransitionTo(a_stateMachine.walkState); // FlyEnemy는 idle 상태로 전환
                }


                // FlyEnemy의 경우 공격 후 1초 후 idle 상태로 전환
                if (flyEnemy != null)
                {
                    yield return new WaitForSeconds(2f); // 공격 애니메이션 시간 대기 (1초)
                    a_stateMachine.TransitionTo(a_stateMachine.idleState); // FlyEnemy는 idle 상태로 전환
                }
            }
        }
    }
}
