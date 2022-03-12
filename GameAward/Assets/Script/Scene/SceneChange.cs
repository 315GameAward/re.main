using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v

public class SceneChange : MonoBehaviour
{
    string SceneName;
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
                    SceneManager.UnloadSceneAsync(SceneName);
                    CoUnload();

                }
                break;
            case "GameScene":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // �^�C�g���V�[���̃��[�h
                    SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
                    SceneManager.UnloadSceneAsync(SceneName);
                    CoUnload();
                }
                break;
            default:
                break;
        }
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
}
