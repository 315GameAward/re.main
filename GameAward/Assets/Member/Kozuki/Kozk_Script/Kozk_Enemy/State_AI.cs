using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class State_AI<T>
    {
        // このステートを利用するインスタンス
        protected T owner;

        public State_AI(T owner)
        {
            this.owner = owner;
        }

        // このステートに遷移する時に一度だけ呼ばれる
        public virtual void Enter() { }

        // このステートである間、毎フレーム呼ばれる
        public virtual void Execute() { }

        // このステートから他のステートに遷移するときに一度だけ呼ばれる
        public virtual void Exit() { }
    }
