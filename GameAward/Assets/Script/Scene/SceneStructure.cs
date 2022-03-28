//================================================
//
//      SceneManager.cs
//      �V�[���}�l�[�W���[
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//================================================

//================================================
// �J������
// 2022/03/04 �쐬�J�n
// �V�[���𓮓I�ɒǉ�����x�ɕ����u���鏈����ǉ��B
// �J�ڐ�V�[�������[�h����ƂƂ��Ɍ��݂̃V�[����
// ���������ǉ��B
// ���݂̃V�[�������擾����֐��̒ǉ��B
// �ҏW��: �ĎR꣑��Y
//------------------------------------------------
// 2022/03/13 �V�[����
// �ҏW��: �ĎR꣑��Y
//------------------------------------------------
// 2022/03/14 �V�[����񋓑̂ŊǗ�
// Dictionary�N���X���g���āu�V�[�����v�Ɓu�V�[���̊Ǘ��Ɏg��
// �񋓑́v�̕R�Â��������쐬
// �ҏW��: �ĎR꣑��Y
//------------------------------------------------
// 2022/03/27 �r�b�g�t���O�Ǘ��p�~
// �c���[�\���ŊǗ����J�n
// �ҏW��: �ĎR꣑��Y
//================================================

using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v
using System.Linq;


public class SceneStructure : MonoBehaviour
{
    // Start is called before the first frame update
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Start()
    {
        // ����͂����Ɛe�q�֌W�ɂȂ��Ă��邾�낤��
        // ���ڌ��т��Ă���Ƃ������ԐړI�Ȋ���������B

        // �x�[�X�̃C���X�^���X
        NodeBase Base = new NodeBase(null, "SceneBase");
        Base.list.Add(Base);    // ���X�g�ɓo�^
        Debug.Log("Name:" + Base.Name);
        Debug.Log("URI:" + Base.URI);

        // �^�C�g���̃C���X�^���X
        NodeBase Title = new NodeBase(Base, "TitleScene");
        Base.AddChild(Title);   // �x�[�X�Ɏq�m�[�h(Title)�̓o�^
        Debug.Log("Name:" + Title.Name);
        Debug.Log("URI:" + Title.URI);

        // �Q�[���̃C���X�^���X
        NodeBase Game = new NodeBase(Base, "GameScene");
        Base.AddChild(Game);    // �x�[�X�Ɏq�m�[�h(Game)�̓o�^
        Debug.Log("Name:" + Game.Name);
        Debug.Log("URI:" + Game.URI);

        // ���U���g�̃C���X�^���X
        NodeBase Result = new NodeBase(Base, "ResultScene");
        Base.AddChild(Result);    // �x�[�X�Ɏq�m�[�h(Result)�̓o�^
        Debug.Log("Name:" + Result.Name);
        Debug.Log("URI:" + Result.URI);

        // ���̏�Ԃ��ƃ��X�g�ɂׁ[�Y�̃m�[�h���o�^����Ă��邽��
        // �x�[�X�̎q�m�[�h�̐����킩��Â炢
        // ����ς�x�[�X�̓��X�g�ɒǉ����Ȃ������������H
        Debug.Log(Base.list.Count);
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    //========================================
    //  �m�[�h�쐬�p�N���X
    //========================================
    public class NodeBase
    {
        // �m�[�h����o�^���郊�X�g(���̃��X�g�����Ɏg�����͎����ł��킩���Ă��Ȃ�)�����@�\�Ƃ��Ŏg���邾�낤��
        public List<NodeBase> list = new List<NodeBase>() { };

        // �쐬�����m�[�h�̐e���
        public NodeBase parent;

        // �쐬�����m�[�h�̖��O
        public string Name { get; set; }    // �܂��v���p�e�B���܂藝���ł��Ă��Ȃ��H�Ȃ񂩎g�������ȋC�����ĕt���Ă݂�

        // �쐬�����m�[�h��URI
        public string URI = "SceneBase";    // �x�[�X�̃m�[�h�����Ȃ��Ȃ炱���ŏ������͂���Ȃ��H

        // �m�[�h�̃C���X�^���X��������(�����F�e���A�쐬����m�[�h�̖��O)
        public NodeBase(NodeBase _parent,string _name)
        {
            parent = _parent;
            Name = _name;
        }

        // �q�m�[�h�̓o�^&�p�X��URI�o�^
        public void AddChild(NodeBase _child)
        {
            list.Add(_child);
            URI += ("/" + _child.Name);
        }
    }
}


