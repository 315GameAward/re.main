using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] Timer timer1;

    public int second;

    //[SerializeField] Timer SetDuration;
    // Start is called before the first frame update
    private void Start()
    {
        timer1.SetDuration(second).Begin();
    }

    // Update is called once per frame
    void Update()
    {
        //ÉCÉxÉìÉgç∑ÇµçûÇ›
        //if (timer1.Duration == 0)
        //{
        //    Destroy(gameObject);
        //}
    }
}
