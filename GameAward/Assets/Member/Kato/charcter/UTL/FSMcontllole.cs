using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMcontllole
{
    public FSMcontllole(FSM pa)
    {
        parent = pa;
    }
    FSM parent;
    public FSM Parent
    {
        get
        {
            return parent;
        }
    }

    string Currentnord;
    string Prevnord;


    Dictionary<string, FSMnord> allnode = new Dictionary<string, FSMnord>();
    public void Startnode(string key)
    {
        Currentnord = Prevnord = key;
        allnode[Currentnord].Start(this);
    }
    public void changenode(string key)
    {
        Prevnord = Currentnord;
        allnode[Currentnord].End(this);
        Currentnord = key;
        allnode[Currentnord].Start(this);

    }
    public void Updatenode()
    {
        allnode[Currentnord].Update(this);
    }

    public void addnode(string key, FSMnord adnode)
    {
        allnode.Add(key, adnode);
    }
}
