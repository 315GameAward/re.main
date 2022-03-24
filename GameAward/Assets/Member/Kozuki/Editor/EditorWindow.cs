//========================
// 
//      EditorWindow
// 		エディタウィンドウ
//
//--------------------------------------------
// 作成者：上月大地
//========================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

enum IngredientUnit { Spoon, Cup, Bowl, Piece }

// カスタムの Serializable クラス
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
    // 初期の情報です-------------------
    string myString = "Kozuki is Dead";
    bool groupEnabled;
    bool myBool = true;
    bool myBool2 = true;
    float myFloat = 1.0f;
    // ---------------------------------

    [MenuItem("Editor/Sample")]
    private static void Create()
    {
         //既存のウィンドウのインスタンスを表示。ない場合は作成します。
        EditorWindow.GetWindow(typeof(EditorWindowSample));

        // 生成
        EditorWindowSample window = GetWindow<EditorWindowSample>("サンプル");

        // 最小サイズ設定
        window.minSize = new Vector2(320, 160);
    }

    /// <summary>
    /// ScriptableObjectSampleの変数
    /// </summary>
    // private ScriptableObjectSample _sample;

    private void OnGUI()
    {
        // 実際のウィンドウのコードはここに書きます↓
        using (new GUILayout.VerticalScope())
        {
            //if (_sample == null)
            //{
            //    _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();
            //}

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            myString = EditorGUILayout.TextField("Text Field", myString);

            // これがオンになったらこれより下を弄れる
            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

            myBool = EditorGUILayout.Toggle("Toggle", myBool);

            myBool2 = EditorGUILayout.Toggle("Toggle2", myBool2);

            myFloat = EditorGUILayout.Slider("Slider", myFloat, -10, 10);

            // これでグループ終了
            EditorGUILayout.EndToggleGroup();
        }

    }
}
