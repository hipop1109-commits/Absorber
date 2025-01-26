using UnityEngine;

public class FixedEnemy : BaseEnemy
{
    protected override void PerformMovement()
    {
        Jump();

    }

    private void Jump()
    {
        stateMachine.TransitionTo(stateMachine.jumpState);
    }

}
