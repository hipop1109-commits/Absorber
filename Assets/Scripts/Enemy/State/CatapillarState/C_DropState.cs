using UnityEngine;

public class C_DropState : MonoBehaviour
{
    EnemyController enemy;
    public C_DropState(EnemyController enemy)
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
