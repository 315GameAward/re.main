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


public class SceneStructure : MonoBehaviour
{
    // Start is called before the first frame update
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Start()
    {
        // これはちゃんと親子関係になっているだろうか
        // 直接結びついているというより間接的な感じがする。

        // ベースのインスタンス
        NodeBase Base = new NodeBase(null, "SceneBase");
        Base.list.Add(Base);    // リストに登録
        Debug.Log("Name:" + Base.Name);
        Debug.Log("URI:" + Base.URI);

        // タイトルのインスタンス
        NodeBase Title = new NodeBase(Base, "TitleScene");
        Base.AddChild(Title);   // ベースに子ノード(Title)の登録
        Debug.Log("Name:" + Title.Name);
        Debug.Log("URI:" + Title.URI);

        // ゲームのインスタンス
        NodeBase Game = new NodeBase(Base, "GameScene");
        Base.AddChild(Game);    // ベースに子ノード(Game)の登録
        Debug.Log("Name:" + Game.Name);
        Debug.Log("URI:" + Game.URI);

        // リザルトのインスタンス
        NodeBase Result = new NodeBase(Base, "ResultScene");
        Base.AddChild(Result);    // ベースに子ノード(Result)の登録
        Debug.Log("Name:" + Result.Name);
        Debug.Log("URI:" + Result.URI);

        // この状態だとリストにべーズのノードも登録されているため
        // ベースの子ノードの数がわかりづらい
        // やっぱりベースはリストに追加しない方がいいか？
        Debug.Log(Base.list.Count);
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    //========================================
    //  ノード作成用クラス
    //========================================
    public class NodeBase
    {
        // ノード情報を登録するリスト(このリストを何に使うかは自分でもわかっていない)検索機能とかで使えるだろうか
        public List<NodeBase> list = new List<NodeBase>() { };

        // 作成したノードの親情報
        public NodeBase parent;

        // 作成したノードの名前
        public string Name { get; set; }    // まだプロパティあまり理解できていない？なんか使えそうな気がして付けてみた

        // 作成したノードのURI
        public string URI = "SceneBase";    // ベースのノードを作らないならここで初期化はいらない？

        // ノードのインスタンス生成処理(引数：親情報、作成するノードの名前)
        public NodeBase(NodeBase _parent,string _name)
        {
            parent = _parent;
            Name = _name;
        }

        // 子ノードの登録&パスのURI登録
        public void AddChild(NodeBase _child)
        {
            list.Add(_child);
            URI += ("/" + _child.Name);
        }
    }
}


