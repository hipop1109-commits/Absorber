using UnityEngine;

public class E_WalkState : IState
{
    BaseEnemy enemy;
    public E_WalkState(BaseEnemy enemy)
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
