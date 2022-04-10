using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node __root = null;

        // Start is called before the first frame update
        protected void Start()
        {
            __root = SetupTree();
        }

        // Update is called once per frame
        private void Update()
        {
            if (__root != null)
                __root.Evaluate();
            
        }
        protected abstract Node SetupTree();


    }
}



