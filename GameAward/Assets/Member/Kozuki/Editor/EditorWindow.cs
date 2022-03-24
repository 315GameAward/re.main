//========================
// 
//      EditorWindow
// 		�G�f�B�^�E�B���h�E
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
public class EditorWindowSample : EditorWindow
{
    // �����̏��ł�-------------------
    string myString = "Kozuki is Dead";
    bool groupEnabled;
    bool myBool = true;
    bool myBool2 = true;
    float myFloat = 1.0f;
    // ---------------------------------

    [MenuItem("Editor/Sample")]
    private static void Create()
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
