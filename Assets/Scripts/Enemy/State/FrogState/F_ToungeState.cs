using UnityEngine;

public class F_ToungeState : MonoBehaviour
{
    EnemyController enemy;
    public F_ToungeState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Tounge");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
