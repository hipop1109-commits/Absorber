using UnityEngine;

public class E_IdleState : IState
{
    EnemyController enemy;
    public E_IdleState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Idle");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
