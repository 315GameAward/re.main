using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーン切り替えに必要

public class SceneChange : MonoBehaviour
{
    string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
        // 現在のシーン名取得
        SceneName = SceneManager.GetSceneAt(1).name;
    }

    // Update is called once per frame
    void Update()
    {
        SceneName = SceneManager.GetSceneAt(1).name;


        // 現在のシーンが〇〇だったら...
        switch (SceneName)
        {
            case "TitleScene":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // ゲームシーンのロード
                    SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
                    SceneManager.UnloadSceneAsync(SceneName);
                    CoUnload();

                }
                break;
            case "GameScene":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // タイトルシーンのロード
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
        //SceneAをアンロード
        var op = SceneManager.UnloadSceneAsync(SceneName);
        yield return op;

        //アンロード後の処理を書く

        //必要に応じて不使用アセットをアンロードしてメモリを解放する
        //けっこう重い処理なので、別に管理するのも手
        yield return Resources.UnloadUnusedAssets();

    }
}
