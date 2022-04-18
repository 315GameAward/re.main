//========================
// 
//      Compass.cs
// 		�R���p�X�G�l�~�[
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCompass
{
    // �R���p�X�̏��
    public enum CompassState
    {
        WAIT,            // �s������U��~
        MOVE,            // �o�~�ړ�
        MOVEPLAYER,      // �v���C���[�����Ɉړ�
        ATTACK,          // ��~���čU��
        IDLE,            // �ҋ@
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

        // �Q�[���J�n��
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            // �n�߂Ƀv���C���[�̈ʒu���擾�ł���悤�ɂ���
            player = GameObject.FindWithTag("Player").transform;
           
            // �X�e�[�g�}�V���̏����ݒ�
            stateList.Add(new StateMove(this));
            stateList.Add(new StateMovePlayer(this));
            stateList.Add(new StateAttack(this));
            stateList.Add(new StateIdle(this));

            stateMachine = new StateMachine<Compass>();

            ChangeState(CompassState.MOVE);
        }
       
        #region States

        /// <summary>
        /// �X�e�[�g: �p�j
        /// </summary>
        private class StateMove : State_AI<Compass>
        {
            private Vector3 targetPosition;

            public StateMove(Compass owner) : base(owner) { }

            public override void Enter()
            {
                // �n�߂̖ڕW�n�_��ݒ肷��
                targetPosition = GetRandomPositionOnLevel();
            }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // �v���C���[�Ƃ̋�������������΁A�ǐՃX�e�[�g�ɑJ��
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)
                {
                    owner.ChangeState(CompassState.MOVEPLAYER);
                }

                // �ڕW�n�_�Ƃ̋�������������΁A���̃����_���ȖڕW�n�_��ݒ肷��
                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);

                Debug.Log(sqrDistanceToTarget);
                if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
                {
                    targetPosition = GetRandomPositionOnLevel();
                }

                // �ڕW�n�_�̕���������
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // �O���ɐi��
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
        /// �X�e�[�g: �ǐ�
        /// </summary>
        private class StateMovePlayer : State_AI<Compass>
        {
            public StateMovePlayer(Compass owner) : base(owner) { }

            public override void Enter() { }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // �v���C���[�Ƃ̋�������������΁A�U���X�e�[�g�ɑJ��
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)
                {
                    owner.ChangeState(CompassState.ATTACK);
                }

                // �v���C���[�Ƃ̋������傫����΁A�p�j�X�e�[�g�ɑJ��
                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
                {
                    owner.ChangeState(CompassState.MOVE);
                }

                // �v���C���[�̕���������
               // Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                //owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // �O���ɐi��
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
            }

            public override void Exit() { }
        }

        /// <summary>
        /// �X�e�[�g: �U��
        /// </summary>
        private class StateAttack : State_AI<Compass>
        {
            private float lastAttackTime;

            public StateAttack(Compass owner) : base(owner) { }

            public override void Enter() { }

            public override void Execute()
            {
                Debug.Log(owner.GetEnum());
                // �v���C���[�Ƃ̋������傫����΁A�ǐՃX�e�[�g�ɑJ��
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin)
                {
                    owner.ChangeState(CompassState.MOVE);
                }

                
                // ���Ԋu�ōU��
                if (Time.time > lastAttackTime + owner.attackInterval)
                {
                    // �����ɍU��������
                }
            }

            public override void Exit() { }
        }

        /// <summary>
        /// �X�e�[�g: �ҋ@
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
