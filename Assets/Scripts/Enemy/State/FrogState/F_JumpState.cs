using UnityEngine;

public class F_JumpState : IState
{
    EnemyController enemy;
    public F_JumpState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Jump");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
