using UnityEngine;

public class E_DieState : IState
{
    EnemyController enemy;
    public E_DieState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Die");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
