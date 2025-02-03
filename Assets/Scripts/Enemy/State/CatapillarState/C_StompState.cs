using UnityEngine;

public class B_ShootState : IState
{
    BaseEnemy enemy;
    public B_ShootState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Shoot");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
