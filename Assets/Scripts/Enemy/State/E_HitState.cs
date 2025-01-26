using UnityEngine;

public class E_HitState : IState
{
    BaseEnemy enemy;
    public E_HitState(BaseEnemy enemy)
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
