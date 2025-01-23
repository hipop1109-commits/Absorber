using UnityEngine;

public class A_SpineState : IState
{
    EnemyController enemy;
    public A_SpineState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Spine");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}