using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    // °‚É“–‚Á‚Ä‚¢‚é
    public bool HitGround = true;

    //OnCollisionExit()
    private void OnCollisionExit(Collision collision)
    {
        // Cube‚ªPlane‚©‚ç—£‚ê‚½ê‡
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
