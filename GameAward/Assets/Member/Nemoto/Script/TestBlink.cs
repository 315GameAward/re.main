using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TestBlink : MonoBehaviour
{
    public Graphic r;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var _c = r.color;
        _c.r = Mathf.Abs(Mathf.Repeat(Time.time, 2.2f) - 1f);
        _c.g = Mathf.Abs(Mathf.Repeat(Time.time, 2.2f) - 1f);
        //_c.b = Mathf.Abs(Mathf.Repeat(Time.time, 2.2f) - 1f);
        r.color = _c;

    }
}
