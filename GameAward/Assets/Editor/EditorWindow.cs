using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWindowSample : EditorWindow
{
    // �����̏��ł�
    string myString = "Kozuki is Dead";
    bool groupEnabled;
    bool myBool = true;
    bool myBool2 = true;
    float myFloat = 1.23f;

    [MenuItem("Editor/Sample")]
    private static void Create()
    {
        // ����
        EditorWindowSample window = GetWindow<EditorWindowSample>("�T���v��");

        // �ŏ��T�C�Y�ݒ�
        window.minSize = new Vector2(320, 160);
    }

    /// <summary>
    /// ScriptableObjectSample�̕ϐ�
    /// </summary>
    private ScriptableObjectSample _sample;

    private void OnGUI()
    {
        // ���ۂ̃E�B���h�E�̃R�[�h�͂����ɏ����܂���
        if (_sample == null)
        {
            _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();
        }

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        myString = EditorGUILayout.TextField("Text Field", myString);

        // ���ꂪ
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

        myBool = EditorGUILayout.Toggle("Toggle", myBool);

        myBool2 = EditorGUILayout.Toggle("Toggle2", myBool2);

        myFloat = EditorGUILayout.Slider("Slider", myFloat, -10, 10);

        EditorGUILayout.EndToggleGroup();
    }
}
