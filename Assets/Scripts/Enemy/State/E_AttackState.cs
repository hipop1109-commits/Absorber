using UnityEngine;

public class E_AttackState : IState
{
    BaseEnemy enemy;
    public E_AttackState(BaseEnemy enemy)
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
