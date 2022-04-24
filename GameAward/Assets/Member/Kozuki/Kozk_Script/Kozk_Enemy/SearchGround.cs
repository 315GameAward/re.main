using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    
    public float speed;
    //OnCollisionExit()
    void OnTriggerStay(Collider collision)
    {
        // Š÷‚Æ“–‚Á‚Ä‚¢‚½ê‡
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("“–‚Á‚Ä‚é");
            this.transform.Translate(Vector3.right * speed);
            GetComponent<GuardBT>().enabled = false;
        }
            else
            {
                Debug.Log("“–‚Á‚Ä‚È‚¢");
            }
    }
}
