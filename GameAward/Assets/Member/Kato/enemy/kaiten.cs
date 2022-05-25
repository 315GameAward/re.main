using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaiten : MonoBehaviour
{
    // Start is called before the first frame update
    float z;
    bool hitground;
    Enemy enemy;
    public bool Hitground
    {
        get
        {
            return hitground;
        }
    }
    void Start()
    {
        Enemy enemy;
        enemy = gameObject.GetComponent<Enemy>();
    }
    void Update()
    {
        ray();

        if (Hitground == false)
        {
            transform.Rotate(0, 1, 0);
        }
        if (Hitground == true)
        {
            Debug.Log("debug comment");
            transform.Rotate(0, 0, 0);
        }

    }
    void ray()
    {
        int back = LayerMask.GetMask("BackGround");
        Vector3 foll;

        RaycastHit rayhit;
        //Raybrockhit 
        rayhit = new RaycastHit();
        foll = new Vector3(0, -1, 0);
        hitground = Physics.Raycast(transform.position, foll, out rayhit, 1, back, QueryTriggerInteraction.Ignore);
    }
}
