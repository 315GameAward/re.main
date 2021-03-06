//========================
// 
//      EditorWindow
// 		エディタウィンドウ
//
//--------------------------------------------
// 作成者：上月大地
//========================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

// カスタムの Serializable クラス
[Serializable]
//public class Ingredient
//{
//    public string name;
//    public int amount = 1;
//}
//public class Recipe : MonoBehaviour
//{
//    public Ingredient potionResult;
//    public Ingredient[] potionIngredients;
//}

// シーンウィンドウ
public class EditorScene : EditorWindow
{
    // 検索用文字列入れ
    string myString = null;

    // ボタンの大きさ
    private readonly Vector2 _buttonMinSize = new Vector2(100, 20);
    private readonly Vector2 _buttonMaxSize = new Vector2(300, 60);

    // シーンのウィンドウ
    [MenuItem("Editor/Scene")]
    private static void Scene()
    {
        //既存のウィンドウのインスタンスを表示。ない場合は作成します。
        EditorWindow.GetWindow(typeof(EditorScene));

        // 生成
        EditorScene window = GetWindow<EditorScene>("シーン");

        // 最小サイズ設定
        window.minSize = new Vector2(320, 160);
    }

    void OnGUI()
    {
        // レイアウトを整える
        GUIStyle buttonStyle = new GUIStyle("button") { fontSize = 30 };
        var layoutOptions = new GUILayoutOption[]
        {
            GUILayout.MinWidth(_buttonMinSize.x),
            GUILayout.MinHeight(_buttonMinSize.y),
            GUILayout.MaxWidth(_buttonMaxSize.x),
            GUILayout.MaxHeight(_buttonMaxSize.y)
        };

        // Titleボタン
        if (GUILayout.Button("タイトルα", buttonStyle, layoutOptions))
        {
            // シーンを保存するか確認
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Titleシーンを開く
            OpenScene("TitleScene");
        }

        // メインゲームボタン
        if (GUILayout.Button("ゲームα", buttonStyle, layoutOptions))
        {
            // シーンを保存するか確認
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Titleシーンを開く
            OpenScene("GameScene");
        }

        // プロトタイプ
        if (GUILayout.Button("プロトタイプ", buttonStyle, layoutOptions))
        {
            // シーンを保存するか確認
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Titleシーンを開く
            OpenScene("prototype");
        }
        // Nemotoボタン
        if (GUILayout.Button("ネモト", buttonStyle, layoutOptions))
        {
            // シーンを保存するか確認
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { SceneManager.GetActiveScene() })) return;
            // Titleシーンを開く
            OpenScene("NemotoScene");
        }

        // 文字列入力場所
        myString = EditorGUILayout.TextField("その他のシーン", myString);
        // 検索ボタン
        if (GUILayout.Button("入力したシーンに飛ぶ"))
        {
            OpenScene(myString);
        }
    }

    // シーンを開ける関数
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
// オブジェクト名に文字を追加
public class PrefixAdder : ScriptableWizard
{
    [SerializeField] string prefix;
    [SerializeField] string subfix;

    [MenuItem("Editor/String Adder", true)]
    static bool CreateWizardValidator()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.ExcludePrefab);
        return transforms.Length >= 1;
    }
    [MenuItem("Editor/String Adder", false)]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("String Adder", typeof(PrefixAdder), "追加して閉じる","追加");
    }
    void OnWizardCreate()
    {
        ApplyPrefix();
    }
    void ApplyPrefix()
    {
        GameObject[] gos = Selection.gameObjects;
        foreach (GameObject go in gos)
        {
            var parent = go.GetComponentInParent(typeof(Transform));
            var children = go.GetComponentsInChildren(typeof(Transform));

            //子のリネーム
            foreach (Transform child in children)
            {
                Undo.RegisterCompleteObjectUndo(child.gameObject,"Added: " + (string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + (string.IsNullOrEmpty(subfix) ? "" : subfix));

                // Don't apply to root object.
                if (child == go.transform)
                    continue;

                if (!string.IsNullOrEmpty(prefix))
                {
                    child.name = prefix + child.name;
                }
                if (!string.IsNullOrEmpty(subfix))
                {
                    child.name = child.name + subfix;
                }
            }

            //親のリネーム
            if (!string.IsNullOrEmpty(prefix))
            {
                parent.name = prefix + parent.name;
            }
            if (!string.IsNullOrEmpty(subfix))
            {
                parent.name = parent.name + subfix;
            }

        }
    }
    void OnWizardOtherButton()
    {
        ApplyPrefix();
    }
    void OnWizardUpdate()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.ExcludePrefab);
        helpString = "Objects selected: " + transforms.Length;
        errorString = "";
        isValid = true;

        if (transforms.Length < 1)
        {
            errorString += "No object selected to rename";
        }
        isValid = string.IsNullOrEmpty(errorString);

    }
    void OnSelectionChange()
    {
        OnWizardUpdate();
    }
}