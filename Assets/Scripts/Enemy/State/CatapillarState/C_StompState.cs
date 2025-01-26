using UnityEngine;

public class C_StompState : MonoBehaviour
{
    BaseEnemy enemy;
    public C_StompState(BaseEnemy enemy)
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
