using UnityEngine;

public class A_AngryState : IState
{
    BaseEnemy enemy;
    public A_AngryState(BaseEnemy enemy)
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