using UnityEngine;

public class HealState : IState
{
    PlayerController player;
    public HealState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        player.GetComponent<Animator>().SetTrigger("Heal");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}