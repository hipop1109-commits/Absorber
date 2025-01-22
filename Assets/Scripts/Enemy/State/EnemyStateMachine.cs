using UnityEngine;

public class EnemyStateMachine 
{
    public IState CurrentState { get; private set; }
    EnemyController enemy;

    public E_IdleState idleState;
    public E_WalkState walkState;
    public E_AttackState attackState;
    public E_HitState hitState;
    public E_DieState dieState;
    public A_AngryState a_AngryState;
    public A_StompState a_StompState;
    public A_SpineState a_SpineState;
    public EnemyStateMachine(EnemyController enemy)
    {
        this.enemy = enemy;
        idleState = new E_IdleState(enemy);
        walkState = new E_WalkState(enemy);
        attackState = new E_AttackState(enemy);
        hitState = new E_HitState(enemy);
        dieState = new E_DieState(enemy);
        a_AngryState = new A_AngryState(enemy);
        a_StompState = new A_StompState(enemy);
        a_SpineState = new A_SpineState(enemy);
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
