using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateMachine<T>
{
    private State_AI<T> currentState;

    public StateMachine()
    {
        currentState = null;
    }

    public State_AI<T> CurrentState
    {
        get { return currentState; }
    }

    // ��ԃ`�F���W
    public void ChangeState(State_AI<T> state)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = state;
        currentState.Enter();
    }

    // ����Ă΂��֐�
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
