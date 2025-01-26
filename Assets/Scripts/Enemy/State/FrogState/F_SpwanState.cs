using UnityEngine;

public class F_SpwanState : IState
{
    EnemyController enemy;
    public F_SpwanState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Spwan");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
