using UnityEngine;

public class C_DropState : IState
{
    BaseEnemy enemy;
    public C_DropState(BaseEnemy enemy)
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
