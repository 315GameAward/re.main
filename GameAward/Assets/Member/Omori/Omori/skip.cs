using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class skip : MonoBehaviour
{
   // TimelinePreferences timeline;
    private PlayableDirector director;
    public GameObject controlPanel;
    
    // Start is called before the first frame update
    void Start()
    {
       // timeline = GetComponent<TimelinePreferences>();
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
       // director.time = 10;
        
        // �X�y�[�X�L�[����������
        if(Keyboard.current.oKey.isPressed)
        {
            director.initialTime = 10;
            // Time.e = 30;
        }
    }
}
