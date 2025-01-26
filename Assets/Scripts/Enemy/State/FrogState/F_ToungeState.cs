using UnityEngine;

public class F_ToungeState : MonoBehaviour
{
    BaseEnemy enemy;
    public F_ToungeState(BaseEnemy enemy)
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
