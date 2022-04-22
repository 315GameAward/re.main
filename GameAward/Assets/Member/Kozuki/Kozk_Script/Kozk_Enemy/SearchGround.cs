using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    // 床に当っている
    public bool HitGround = true;

    //OnCollisionExit()
    private void OnCollisionExit(Collision collision)
    {
        // CubeがPlaneから離れた場合
        if (collision.gameObject.name == "Ground")
        {
            HitGround = false;
        }
    }
    public bool GetHitGround
    {
        get
        { 
        return HitGround;
        }
    }
}
