using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    // ���ɓ����Ă���
    public bool HitGround = true;

    //OnCollisionExit()
    private void OnCollisionExit(Collision collision)
    {
        // Cube��Plane���痣�ꂽ�ꍇ
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
