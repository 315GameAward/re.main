using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject image_gameOver;
    public bool debug = false;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      
        if (debug) return;
        // ���g���C
        if(Life.instance.GetLife() <= 0)
        {
            if(Keyboard.current.rKey.isPressed)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    // �����蔻��
    private void OnTriggerEnter(Collider other)
    {

        // �G�l�~�[�^�O�ƐG�ꂽ��
        if (other.gameObject.tag == "Enemy")
        {
            Life.instance.DelLife();
            //image_gameOver.GetComponent<GameOver>().ShowGameOver();
        }
        // Wall�^�O�ƐG�ꂽ��
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("��������");
        }
    }

    public void SetPotision(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
