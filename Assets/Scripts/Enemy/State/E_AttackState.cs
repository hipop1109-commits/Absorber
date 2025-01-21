using UnityEngine;

public class E_AttackState : IState
{
    EnemyController enemy;
    public E_AttackState(EnemyController enemy)
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
