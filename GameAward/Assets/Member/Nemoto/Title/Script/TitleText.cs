using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleText : MonoBehaviour
{
    public Graphic r;
    public Graphic shadow;
    public Graphic outline;

    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(camera.GetComponent<TitleScene>().bTrigger)
        {
            var _c = r.color;
            _c.a = Mathf.Abs(Mathf.Repeat(Time.time, 0.2f) - 1f);
            r.color = _c;


            if (gameObject.GetComponent<Shadow>())
            {
                var c = gameObject.GetComponent<Shadow>().effectColor;
                c.a = Mathf.Abs(Mathf.Repeat(Time.time, 0.2f) - 1f);

                gameObject.GetComponent<Shadow>().effectColor = c;
            }
            if (gameObject.GetComponent<Outline>())
            {
                var c = gameObject.GetComponent<Outline>().effectColor;
                c.a = Mathf.Abs(Mathf.Repeat(Time.time, 0.2f) - 1f);

                gameObject.GetComponent<Outline>().effectColor = c;
            }
        }
        else
        {
            var _c = r.color;
            _c.a = Mathf.Abs(Mathf.Repeat(Time.time, 3f) - 1f);
            r.color = _c;


            if (gameObject.GetComponent<Shadow>())
            {
                var c = gameObject.GetComponent<Shadow>().effectColor;
                c.a = Mathf.Abs(Mathf.Repeat(Time.time, 3f) - 1f);

                gameObject.GetComponent<Shadow>().effectColor = c;
            }
            if (gameObject.GetComponent<Outline>())
            {
                var c = gameObject.GetComponent<Outline>().effectColor;
                c.a = Mathf.Abs(Mathf.Repeat(Time.time, 3f) - 1f);

                gameObject.GetComponent<Outline>().effectColor = c;
            }
        }
      
    }
}
