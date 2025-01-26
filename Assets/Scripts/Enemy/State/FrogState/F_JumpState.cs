using UnityEngine;

public class F_JumpState : IState
{
    BaseEnemy enemy;
    public F_JumpState(BaseEnemy enemy)
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
