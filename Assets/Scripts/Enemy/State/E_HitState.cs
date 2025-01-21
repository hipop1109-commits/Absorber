using UnityEngine;

public class E_HitState : IState
{
    EnemyController enemy;
    public E_HitState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Hit");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
