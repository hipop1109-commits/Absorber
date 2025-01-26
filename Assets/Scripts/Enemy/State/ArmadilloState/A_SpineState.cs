using UnityEngine;

public class A_SpineState : IState
{
    BaseEnemy enemy;
    public A_SpineState(BaseEnemy enemy)
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