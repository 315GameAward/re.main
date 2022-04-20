using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stateful_Obj<T, TEnum> : MonoBehaviour
    where T : class where TEnum : System.IConvertible
{
    protected List<State_AI<T>> stateList = new List<State_AI<T>>();

    protected StateMachine<T> stateMachine;

   public TEnum a;

    public virtual void ChangeState(TEnum state)
    {
        if (stateMachine == null)
        {
            return;
        }
        a = state;
        stateMachine.ChangeState(stateList[state.ToInt32(null)]);
    }

    public virtual bool IsCurrentState(TEnum state)
    {
        if (stateMachine == null)
        {
            return false;
        }

        return stateMachine.CurrentState == stateList[state.ToInt32(null)];
    }

    protected virtual void Update()
    {
        if (stateMachine != null)
        {
            stateMachine.Update();
        }
    }
    public virtual TEnum GetEnum()
    {
        return a;
    }
}
