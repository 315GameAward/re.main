using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject player;
    public int state = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                if(gameObject.transform.position.z > player.transform.position.z)
                {
                    var pos = new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
                    player.GetComponent<Player>().SetPotision(pos);
                }
                break;
            case 1:
                if (gameObject.transform.position.z < player.transform.position.z)
                {
                     var pos = new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
                    player.GetComponent<Player>().SetPotision(pos);
                }
                break;
            case 2:
                if (gameObject.transform.position.x > player.transform.position.x)
                {
                    var pos = new Vector3(gameObject.transform.position.x, player.transform.position.y, player.transform.position.z);
                    player.GetComponent<Player>().SetPotision(pos);
                }
                break;
            case 3:
                if (gameObject.transform.position.x < player.transform.position.x)
                {
                    var pos = new Vector3(gameObject.transform.position.x, player.transform.position.y, player.transform.position.z);
                    player.GetComponent<Player>().SetPotision(pos);
                }
                break;
        }

    }
}
