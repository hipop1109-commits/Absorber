using UnityEngine;

public class E_JumpState : IState
{
    BaseEnemy enemy;
    public E_JumpState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
