using UnityEngine;

public class E_DieState : IState
{
    BaseEnemy enemy;
    public E_DieState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Die");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
