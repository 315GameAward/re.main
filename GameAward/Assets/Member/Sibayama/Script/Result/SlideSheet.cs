using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UIいじる

public class SlideSheet : MonoBehaviour
{
    public RectTransform slide;
    Arrow arrow;
    
    public float moveDistance;     // 移動量
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        slide.transform.position = new Vector3(960.0f, 1690.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 画面の中心に来たら止める
        if (slide.transform.position.y <= 540.0f)
        {
            pos = slide.transform.position;
            pos.y = 540.0f;
            return;
        }
        // 特定の位置まで下げる
        slide.transform.position -= new Vector3(0.0f, moveDistance, 0.0f);
    }

    // キャンバスのサイズ取得
    static Vector2 GetRectSize(RectTransform self)
    {
        // asはCで言うところの型キャスト
        // スクリプトを指定した親のUI情報を入れる
        var parent = self.parent as RectTransform;
        if (parent == null)
        {
            return new Vector2(self.rect.width, self.rect.height);
        }

        return new Vector2();
    }
}
