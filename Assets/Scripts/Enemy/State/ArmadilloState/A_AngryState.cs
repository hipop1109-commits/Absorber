using UnityEngine;

public class A_AngryState : IState
{
    EnemyController enemy;
    public A_AngryState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Angry");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}