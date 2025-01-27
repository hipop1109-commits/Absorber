using UnityEngine;

public class FlyEnemy : BaseEnemy
{

    public float speed = 2f; // 이동 속도
    public float distance = 5f; // 왔다 갔다 하는 거리
    private Vector3 startPosition; // 시작 위치
    private float time;

    protected override void Start()
    {
        base.Start();
        // 비행형 적의 초기 상태를 idleState로 설정
        stateMachine.Initalize(stateMachine.idleState);
        startPosition = transform.position; // 시작 위치 기록
    }

    protected override void PerformMovement()
    {
        Fly();
    }

    void Fly()
    {
        time += Time.deltaTime * speed; // 시간을 증가시키며 이동
        float offset = Mathf.Sin(time) * distance; // 사인 함수를 이용해 왔다 갔다 하는 값 계산
        transform.position = startPosition + new Vector3(offset, 0, 0); // 위치 업데이트
    }
}
