using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnGUI()
    {
        
            anim.CrossFade("ado", 0);
        

        // "walk"ボタンが押されたら
     
     
     // anim.CrossFade("walk", 0);
     
    }


}
