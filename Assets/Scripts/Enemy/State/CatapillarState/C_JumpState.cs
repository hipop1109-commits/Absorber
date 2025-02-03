using UnityEngine;

public class B_AttackState : IState
{
    BaseEnemy enemy;
    public B_AttackState(BaseEnemy enemy)
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
