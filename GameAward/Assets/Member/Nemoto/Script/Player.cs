using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject image_gameOver;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �����蔻��
    private void OnTriggerEnter(Collider other)
    {

        // �G�l�~�[�^�O�ƐG�ꂽ��
        if (other.gameObject.tag == "Enemy")
        {
            image_gameOver.GetComponent<GameOver>().ShowGameOver();
        }
    }
}
