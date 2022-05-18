using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI‚¢‚¶‚é

public class SlideSheet : MonoBehaviour
{
    public RectTransform slide;
    Arrow arrow;
    
    public float moveDistance;     // ˆÚ“®—Ê
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        slide.transform.position = new Vector3(960.0f, 1690.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // ‰æ–Ê‚Ì’†S‚É—ˆ‚½‚ç~‚ß‚é
        if (slide.transform.position.y <= 540.0f)
        {
            pos = slide.transform.position;
            pos.y = 540.0f;
            return;
        }
        // “Á’è‚ÌˆÊ’u‚Ü‚Å‰º‚°‚é
        slide.transform.position -= new Vector3(0.0f, moveDistance, 0.0f);
    }
}
