using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class State_AI<T>
    {
        // ���̃X�e�[�g�𗘗p����C���X�^���X
        protected T owner;

        public State_AI(T owner)
        {
            this.owner = owner;
        }

        // ���̃X�e�[�g�ɑJ�ڂ��鎞�Ɉ�x�����Ă΂��
        public virtual void Enter() { }

        // ���̃X�e�[�g�ł���ԁA���t���[���Ă΂��
        public virtual void Execute() { }

        // ���̃X�e�[�g���瑼�̃X�e�[�g�ɑJ�ڂ���Ƃ��Ɉ�x�����Ă΂��
        public virtual void Exit() { }
    }
