using UnityEngine;

public class A_StompState : IState
{
    EnemyController enemy;
    public A_StompState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Stomp");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
