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

enum SceneType
{
    None = -1,
    Title = 0,
    Game,
    Select
}

public enum SearchProcess
{
    None = -1,
    Load = 0,
    Instance
}



public class SceneStructure : MonoBehaviour
{
    public static NodeInfo node;
    // Start is called before the first frame update
    void Start()
    {
        node = new NodeInfo();
        node.root.Children[(int)SceneType.Title].Find(node.root.Children[0],SearchProcess.Load);
    }
    // Update is called once per frame
    void Update()
    {

    }



    public class NodeInfo
    {
        // �V�[���f�[�^�̍\�z
        public NodeInfo()
        {

            root.Children.Add(new Element(null, "Title"));
            root.Children.Add(new Element(null, "Game"));
            root.Children.Add(new Element(null, "Select"));

            root.Children[(int)SceneType.Title].Children.Add(new Element(root, "GUI"));

            root.Children[(int)SceneType.Title].Children[0].Children.Add(new Element(root.Children[(int)SceneType.Title], ""));
            root.Children[(int)SceneType.Game].Children.Add(new Element(root, ""));
            root.Children[(int)SceneType.Select].Children.Add(new Element(root, ""));
            
        }


        public Element root = new Element(null, "root");
        //========================================
        //  �m�[�h�쐬�p�N���X
        //========================================
        public class Element
        {
            // �m�[�h�̃C���X�^���X��������(�����F�e���A�쐬����m�[�h�̖��O)
            public Element(Element _parent, string _name)
            {
                NodeName = _name;
                parent = _parent;
                SceneName = _name;
                isLoaded = false;
            }

            // �쐬�����m�[�h�̐e���
            public Element parent;

            // �m�[�h�ɓ����V�[�����
            public Scene scene = new Scene();

            // �쐬�����m�[�h�̖��O
            public string NodeName { get; }

            // �ǂݍ��ރV�[���̖��O
            public string SceneName { get; }

            // ���[�h����
            public bool isLoaded;

            // �쐬�����m�[�h��URI
            public string URI = "Base";

            // �V�[���̓Ǎ���
            void LoadScene(Element _elem)
            {

                if (_elem.isLoaded)
                    return;


                // ���̊֐��̒��ŃV�[���̃f�[�^������͉̂ʂ����Ă����̂�
                // �ʂ̊֐��Ƃ��ď��������������������H
                SceneManager.LoadSceneAsync(_elem.SceneName, LoadSceneMode.Additive);
                parent.isLoaded = _elem.isLoaded;
                scene = SceneManager.GetSceneByName(_elem.SceneName);
            }
            
            // �V�[���̉��
            public void UnLoadScene()
            {
                SceneManager.UnloadSceneAsync(scene);
            }

            // ���[�h�`�F�b�N
            public bool CheckLoaded(Element _elem)
            {
                return _elem.scene.isLoaded;
            }

            public void Type(Element _elem,SearchProcess _searchprocess)
            {
                switch (_searchprocess)
                {
                    case SearchProcess.Load:
                        LoadScene(_elem);
                        break;
                    case SearchProcess.Instance:
                        break;
                    default:
                        break;
                }
            }

            // �T������
            public List<Element> Find(Element _elem, SearchProcess _searchprocess)
            {
                if (_elem.Children.Count == 0)
                {

                    var list = new List<Element>();
                    list.Add(_elem);
                    return list;
                }
                var LoopList = new List<Element>();
                foreach (var child in _elem.Children)
                {
                    foreach (var ret in Find(child, _searchprocess))
                    {
                        LoopList.Add(ret);
                    }
                }
                return LoopList;
            }

            // �V�[���̃C���X�^���X����
            public List<Element> SceneInstance(Element _elem)
            {
                if (_elem.Children.Count == 0)
                {
                    _elem.scene = SceneManager.GetSceneByName("");
                    var list = new List<Element>();
                    list.Add(_elem);
                    return list;
                }
                var LoopList = new List<Element>();
                foreach (var child in _elem.Children)
                {
                    foreach (var ret in SceneInstance(child))
                    {
                        LoopList.Add(ret);
                    }
                }
                return LoopList;
            }

            // �q�m�[�h�쐬
            public List<Element> Children = new List<Element>();
        }
    }
}


