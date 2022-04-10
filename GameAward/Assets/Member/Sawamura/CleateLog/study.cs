/*Behavior Treeについて
・挙動(Behavior)を木構造で記述
・構築済みのBehavior Treeのデータ構造はDAGまたは木になる
・ひとまとまりのタスクが部分木になっている
・木を構成するための大まかな要素３つ
    ・root node：根
    ・control flow node：根でも葉でもない
    ・execution node：葉ノード(タスク)
・評価時は各nodeは深さ優先探索される
・探索結果は子⇒親の順
    ・Success：実行成功
    ・Failure：実行失敗
    ・Running(Continue)：実行中、次回にRunningを返したノードが再度呼ばれる
・全てのノードには評価可能かどうかを示すactive/inactive状態を設定できる
 
・Selector：子ノードのうちいずれか1つを実行するためのノード
            Selector の子ノードのうちどれかが Success か Running を返した場合、
            Selector は即座に Success か Running を親ノードに返します。
            Selector のすべての子ノードがfailureを返した場合、
            Selector も failure を親ノードに返します。

・Sequence：子ノードを順番に実行するためのノード
            同上
・
 */