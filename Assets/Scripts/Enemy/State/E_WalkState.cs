using UnityEngine;

public class E_WalkState : IState
{
    EnemyController enemy;
    public E_WalkState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Walk");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
