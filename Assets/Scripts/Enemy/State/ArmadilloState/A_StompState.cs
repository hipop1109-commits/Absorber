using UnityEngine;

public class A_StompState : IState
{
    BaseEnemy enemy;
    public A_StompState(BaseEnemy enemy)
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
