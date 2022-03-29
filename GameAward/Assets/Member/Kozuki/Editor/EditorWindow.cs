//========================
// 
//      EditorWindow
// 		�G�f�B�^�E�B���h�E
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;


enum IngredientUnit { Spoon, Cup, Bowl, Piece }

// �J�X�^���� Serializable �N���X
[Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    private IngredientUnit unit;
}

public class Recipe : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
}
// �T���v���E�B���h�E
public class EditorWindowSample : EditorWindow
{
    // �����̏��ł�-------------------
    string myString = "Kozuki is Dead";
    bool groupEnabled;
    bool myBool = true;
    bool myBool2 = true;
    float myFloat = 1.0f;
    // ---------------------------------

    // �T���v���̃E�B���h�E
    [MenuItem("Editor/Sample")]
    private static void Sample()
    {
         //�����̃E�B���h�E�̃C���X�^���X��\���B�Ȃ��ꍇ�͍쐬���܂��B
        EditorWindow.GetWindow(typeof(EditorWindowSample));

        // ����
        EditorWindowSample window = GetWindow<EditorWindowSample>("�T���v��");

        // �ŏ��T�C�Y�ݒ�
        window.minSize = new Vector2(320, 160);
    }
    /// <summary>
    /// ScriptableObjectSample�̕ϐ�
    /// </summary>
    // private ScriptableObjectSample _sample;

    private void OnGUI()
    {
        // ���ۂ̃E�B���h�E�̃R�[�h�͂����ɏ����܂���
        using (new GUILayout.VerticalScope())
        {
            //if (_sample == null)
            //{
            //    _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();
            //}

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            myString = EditorGUILayout.TextField("Text Field", myString);

            // ���ꂪ�I���ɂȂ����炱���艺��M���
            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

            myBool = EditorGUILayout.Toggle("Toggle", myBool);

            myBool2 = EditorGUILayout.Toggle("Toggle2", myBool2);

            myFloat = EditorGUILayout.Slider("Slider", myFloat, -10, 10);

            // ����ŃO���[�v�I��
            EditorGUILayout.EndToggleGroup();
        }

    }
}

// �V�[���E�B���h�E
public class EditorScene : EditorWindow
{
    // �{�^���̑傫��
    private readonly Vector2 _buttonMinSize = new Vector2(100, 20);
    private readonly Vector2 _buttonMaxSize = new Vector2(300, 60);

    // �V�[���̃E�B���h�E
    [MenuItem("Editor/Scene")]
    private static void Scene()
    {
        //�����̃E�B���h�E�̃C���X�^���X��\���B�Ȃ��ꍇ�͍쐬���܂��B
        EditorWindow.GetWindow(typeof(EditorScene));

        // ����
        EditorScene window = GetWindow<EditorScene>("�V�[��");

        // �ŏ��T�C�Y�ݒ�
        window.minSize = new Vector2(320, 160);
    }

    void OnGUI()
    {
        // ���C�A�E�g�𐮂���
        GUIStyle buttonStyle = new GUIStyle("button") { fontSize = 30 };
        var layoutOptions = new GUILayoutOption[]
        {
            GUILayout.MinWidth(_buttonMinSize.x),
            GUILayout.MinHeight(_buttonMinSize.y),
            GUILayout.MaxWidth(_buttonMaxSize.x),
            GUILayout.MaxHeight(_buttonMaxSize.y)
        };

        // Title�{�^��
        if (GUILayout.Button("�^�C�g����", buttonStyle, layoutOptions))
        {
            // �V�[����ۑ����邩�m�F
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Title�V�[�����J��
            OpenScene("TitleScene");
        }

        // Select�{�^��
        if (GUILayout.Button("�Q�[����", buttonStyle, layoutOptions))
        {
            // �V�[����ۑ����邩�m�F
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Title�V�[�����J��
            OpenScene("GameScene");
        }

        // Stage�{�^��
        if (GUILayout.Button("Kozk003", buttonStyle, layoutOptions))
        {
            // �V�[����ۑ����邩�m�F
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Title�V�[�����J��
            OpenScene("Kozk003");
        }

        // �v���g�^�C�v
        if (GUILayout.Button("�v���g�^�C�v", buttonStyle, layoutOptions))
        {
            // �V�[����ۑ����邩�m�F
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Title�V�[�����J��
            OpenScene("prototype");
        }
    }

    // �V�[�����J����֐�
    private void OpenScene(string sceneName)
    {
        var sceneAssets = AssetDatabase.FindAssets("t:SceneAsset")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)))
            .Where(obj => obj != null)
            .Select(obj => (SceneAsset)obj)
            .Where(asset => asset.name == sceneName);
        var scenePath = AssetDatabase.GetAssetPath(sceneAssets.First());
        EditorSceneManager.OpenScene(scenePath);
    }
}