using UnityEngine;

public class C_JumpState : IState
{
    EnemyController enemy;
    public C_JumpState(EnemyController enemy)
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
