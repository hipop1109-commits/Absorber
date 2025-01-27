using UnityEngine;

public class FixedEnemy : BaseEnemy
{
    protected override void PerformMovement()
    {
        Jump();

    }

    protected override void Start()
    {
        base.Start();
        // 고정형 적의 초기 상태를 idleState로 설정
        stateMachine.Initalize(stateMachine.idleState);
    }
    private void Jump()
    {
        stateMachine.TransitionTo(stateMachine.jumpState);
    }

}
