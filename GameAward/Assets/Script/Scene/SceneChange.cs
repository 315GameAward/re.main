using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneChange : MonoBehaviour
{
    SceneStructure.NodeInfo Node = new SceneStructure.NodeInfo();
    // Start is called before the first frame update
    

    void Start()
    {
        Node = SceneStructure.node;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
