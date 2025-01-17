using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    PlayerController player;

    public IdleState idleState;
    public JumpState jumpState;
    public DashState dashState; 
    public WalkState walkState;
    public HurtState hurtState;

    public StateMachine(PlayerController player)
    {
        this.player = player;
        idleState = new IdleState(player);
        walkState = new WalkState(player);
        dashState = new DashState(player);
        jumpState = new JumpState(player);
        hurtState = new HurtState(player);
    }

    //최초 state를 받아 이를 CurrentState에 넣고 Enter
    public void Initalize(IState state)
    {
        CurrentState = state;
        state.Enter();
    }

    //바뀔 state를 받아 현재 state에 대해서는 Exit를 수행하고 CurrentState를 바꾸며 
    //바뀔 state의 Enter를 수행
    public void TransitionTo(IState nextState) 
    { 
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Execute()
    {
        CurrentState.Execute();
    }
}
