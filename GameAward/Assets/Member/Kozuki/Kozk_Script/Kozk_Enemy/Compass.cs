//========================
// 
//      Compass.cs
// 		コンパスエネミー
//
//--------------------------------------------
// 作成者：上月大地
//========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCompass
{
    // コンパスの状態
    public enum CompassState
    {
        WAIT,            // 行動を一旦停止
        MOVE,            // 俳諧移動
        MOVEPLAYER,      // プレイヤー方向に移動
        ATTACK,          // 停止して攻撃
        IDLE,            // 待機
    }

    public class Compass : Stateful_Obj<Compass, CompassState>
    {
        private Transform player;

        public float speed = 0.1f;
        private float rotationSmooth = 1f;
        private float attackInterval = 2f;

        private float pursuitSqrDistance = 25f;
        private float attackSqrDistance = 20f;
        private float margin = 5f;

        private float changeTargetSqrDistance = 20f;

        // ゲーム開始時
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            // 始めにプレイヤーの位置を取得できるようにする
            player = GameObject.FindWithTag("Player").transform;
           
            // ステートマシンの初期設定
            stateList.Add(new StateMove(this));
            stateList.Add(new StateMovePlayer(this));
            stateList.Add(new StateAttack(this));
            stateList.Add(new StateIdle(this));

            stateMachine = new StateMachine<Compass>();

            ChangeState(CompassState.MOVE);
        }
       
        #region States

        /// <summary>
        /// ステート: 徘徊
        /// </summary>
        private class StateMove : State_AI<Compass>
        {
            private Vector3 targetPosition;

            public StateMove(Compass owner) : base(owner) { }

            public override void Enter()
            {
                // 始めの目標地点を設定する
                targetPosition = GetRandomPositionOnLevel();
            }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // プレイヤーとの距離が小さければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)
                {
                    owner.ChangeState(CompassState.MOVEPLAYER);
                }

                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);

                Debug.Log(sqrDistanceToTarget);
                if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
                {
                    targetPosition = GetRandomPositionOnLevel();
                }

                // 目標地点の方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
            }

            public override void Exit() { }

            public Vector3 GetRandomPositionOnLevel()
            {
                float levelSize = 55f;
                return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));
            }
        }

        /// <summary>
        /// ステート: 追跡
        /// </summary>
        private class StateMovePlayer : State_AI<Compass>
        {
            public StateMovePlayer(Compass owner) : base(owner) { }

            public override void Enter() { }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // プレイヤーとの距離が小さければ、攻撃ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)
                {
                    owner.ChangeState(CompassState.ATTACK);
                }

                // プレイヤーとの距離が大きければ、徘徊ステートに遷移
                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
                {
                    owner.ChangeState(CompassState.MOVE);
                }

                // プレイヤーの方向を向く
               // Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                //owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
            }

            public override void Exit() { }
        }

        /// <summary>
        /// ステート: 攻撃
        /// </summary>
        private class StateAttack : State_AI<Compass>
        {
            private float lastAttackTime;

            public StateAttack(Compass owner) : base(owner) { }

            public override void Enter() { }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // プレイヤーとの距離が大きければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin)
                {
                    owner.ChangeState(CompassState.MOVE);
                }

                
                // 一定間隔で攻撃
                if (Time.time > lastAttackTime + owner.attackInterval)
                {
                    // ここに攻撃を入れる
                }
            }

            public override void Exit() { }
        }

        /// <summary>
        /// ステート: 待機
        /// </summary>
        private class StateIdle : State_AI<Compass>
        {
            public StateIdle(Compass owner) : base(owner) { }

            public override void Enter()
            {            
            }

            public override void Execute() {
                Debug.Log(owner.GetEnum());
            }

            public override void Exit() { }
        }

        #endregion        
    }
}
