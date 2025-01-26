using UnityEngine;

public class F_SpwanState : MonoBehaviour
{
    BaseEnemy enemy;
    public F_SpwanState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Spwan");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
