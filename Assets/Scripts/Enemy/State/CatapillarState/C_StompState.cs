using UnityEngine;

public class C_StompState : MonoBehaviour
{
    EnemyController enemy;
    public C_StompState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Stomp");
    }

    public void Exit()
    {
    }

    public void Execute()
    {
    }
}
