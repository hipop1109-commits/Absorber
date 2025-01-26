using UnityEngine;

public class C_JumpState : IState
{
    BaseEnemy enemy;
    public C_JumpState(BaseEnemy enemy)
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
