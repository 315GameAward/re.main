using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    FSMcontllole contller;

    public FSM()
    {
        contller = new FSMcontllole(this);
    }
    public void addnode(string s, FSMnord f)
    {
        contller.addnode(s, f);
    }
    // Start is called before the first frame update
    public void changenode(string t)
    {
        contller.changenode(t);
    }
    public void startnode(string s)
    {
        contller.Startnode(s);
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        contller.Updatenode();
    }
}
