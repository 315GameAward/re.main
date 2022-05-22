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
    static float delayTime = 0.0f;
    static bool damage = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �_���[�W�̃f�B���C
        if (damage)
        {
            delayTime += 1.0f / 60.0f;
            if (delayTime > 3.0f)
            {
                delayTime = 0.0f;
                damage = false;
            }
        }

        // �_���[�W���C���Ɠ���������
        for(int i = 0;i < gameObject.GetComponent<CutPoint2>().objList.Count;i++)
        {
            if (!gameObject.GetComponent<CutPoint2>().objList[i].GetComponent<DamageLine>().damage) continue;

            if (!damage) 
            {
                Life.instance.DelLife();
                damage = true;
            }
        }

        

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
        if (other.gameObject.tag == "Enemy" && !damage)
        {
            Life.instance.DelLife();
            damage = true;
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
