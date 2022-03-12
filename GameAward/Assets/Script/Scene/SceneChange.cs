//----------------------------------------------------------
//
//
//
//
//
//
//
//----------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v

public class SceneChange : MonoBehaviour
{
    private string SceneName;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
        // ���݂̃V�[�����擾
        SceneName = SceneManager.GetSceneAt(1).name;
    }

    // Update is called once per frame
    void Update()
    {
        SceneName = SceneManager.GetSceneAt(1).name;


        // ���݂̃V�[�����Z�Z��������...
        switch (SceneName)
        {
            case "TitleScene":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // �Q�[���V�[���̃��[�h
                    SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
                    OnUnloadScene();

                }
                break;
            case "GameScene":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // �^�C�g���V�[���̃��[�h
                    SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
                    OnUnloadScene();
                }
                break;
            default:
                break;
        }
    }


    public void OnUnloadScene()
    {
        StartCoroutine(CoUnload());
    }

    IEnumerator CoUnload()
    {
        //SceneA���A�����[�h
        var op = SceneManager.UnloadSceneAsync(SceneName);
        yield return op;

        //�A�����[�h��̏���������

        //�K�v�ɉ����ĕs�g�p�A�Z�b�g���A�����[�h���ă��������������
        //���������d�������Ȃ̂ŁA�ʂɊǗ�����̂���
        yield return Resources.UnloadUnusedAssets();
    }
    //IEnumerator CoUnload(string SceneName)
    //{
    //    //SceneA���A�����[�h
    //    var op = SceneManager.UnloadSceneAsync(SceneName);
    //    yield return op;
    //
    //    //�A�����[�h��̏���������
    //
    //    //�K�v�ɉ����ĕs�g�p�A�Z�b�g���A�����[�h���ă��������������
    //    //���������d�������Ȃ̂ŁA�ʂɊǗ�����̂���
    //    yield return Resources.UnloadUnusedAssets();
    //
    //}
}
