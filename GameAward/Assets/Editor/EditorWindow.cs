using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWindowSample : EditorWindow
{
    // 初期の情報です
    string myString = "Kozuki is Dead";
    bool groupEnabled;
    bool myBool = true;
    bool myBool2 = true;
    float myFloat = 1.23f;

    [MenuItem("Editor/Sample")]
    private static void Create()
    {
        // 生成
        EditorWindowSample window = GetWindow<EditorWindowSample>("サンプル");

        // 最小サイズ設定
        window.minSize = new Vector2(320, 160);
    }

    /// <summary>
    /// ScriptableObjectSampleの変数
    /// </summary>
    private ScriptableObjectSample _sample;

    private void OnGUI()
    {
        // 実際のウィンドウのコードはここに書きます↓
        if (_sample == null)
        {
            _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();
        }

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        myString = EditorGUILayout.TextField("Text Field", myString);

        // これが
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

        myBool = EditorGUILayout.Toggle("Toggle", myBool);

        myBool2 = EditorGUILayout.Toggle("Toggle2", myBool2);

        myFloat = EditorGUILayout.Slider("Slider", myFloat, -10, 10);

        EditorGUILayout.EndToggleGroup();
    }
}
