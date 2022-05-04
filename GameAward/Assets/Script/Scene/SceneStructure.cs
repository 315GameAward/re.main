//================================================
//
//      SceneManager.cs
//      シーンマネージャー
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//================================================

//================================================
// 開発履歴
// 2022/03/04 作成開始
// シーンを動的に追加し一度に複数置ける処理を追加。
// 遷移先シーンをロードするとともに現在のシーンの
// 解放処理を追加。
// 現在のシーン名を取得する関数の追加。
// 編集者: 柴山凜太郎
//------------------------------------------------
// 2022/03/13 シーン名
// 編集者: 柴山凜太郎
//------------------------------------------------
// 2022/03/14 シーンを列挙体で管理
// Dictionaryクラスを使って「シーン名」と「シーンの管理に使う
// 列挙体」の紐づけ処理を作成
// 編集者: 柴山凜太郎
//------------------------------------------------
// 2022/03/27 ビットフラグ管理廃止
// ツリー構造で管理を開始
// 編集者: 柴山凜太郎
//================================================

using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーン切り替えに必要
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
        // シーンデータの構築
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
        //  ノード作成用クラス
        //========================================
        public class Element
        {
            // ノードのインスタンス生成処理(引数：親情報、作成するノードの名前)
            public Element(Element _parent, string _name)
            {
                NodeName = _name;
                parent = _parent;
                SceneName = _name;
                isLoaded = false;
            }

            // 作成したノードの親情報
            public Element parent;

            // ノードに入れるシーン情報
            public Scene scene = new Scene();

            // 作成したノードの名前
            public string NodeName { get; }

            // 読み込むシーンの名前
            public string SceneName { get; }

            // ロード判定
            public bool isLoaded;

            // 作成したノードのURI
            public string URI = "Base";

            // シーンの読込み
            void LoadScene(Element _elem)
            {

                if (_elem.isLoaded)
                    return;


                // この関数の中でシーンのデータを入れるのは果たしていいのか
                // 別の関数として処理を書いた方がいい？
                SceneManager.LoadSceneAsync(_elem.SceneName, LoadSceneMode.Additive);
                parent.isLoaded = _elem.isLoaded;
                scene = SceneManager.GetSceneByName(_elem.SceneName);
            }
            
            // シーンの解放
            public void UnLoadScene()
            {
                SceneManager.UnloadSceneAsync(scene);
            }

            // ロードチェック
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

            // 探索処理
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

            // シーンのインスタンス処理
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

            // 子ノード作成
            public List<Element> Children = new List<Element>();
        }
    }
}


