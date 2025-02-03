using UnityEngine;

public class B_DropState : IState
{
    BaseEnemy enemy;
    public B_DropState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Drop");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
