using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasamiModel : MonoBehaviour
{
    public GameObject PlayerState;  // �v���C���[�̏��i�[

    public Material blue;   // �n�T�~�̐F�̃}�e���A��
    public Material red;   // �n�T�~�̐ԐF�̃}�e���A��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerState.GetComponent<PlayerControl>().eCutMode);
        // �J�b�g���[�h�ɂ��}�e���A���̐؂�ւ�
        switch (PlayerState.GetComponent<PlayerControl>().eCutMode)
        {
            // �F
            case PlayerControl.CutMode.CUT_ONE:
                gameObject.GetComponent<MeshRenderer>().material = blue;
                Debug.Log("blue");
                break;
            // �ԐF
            case PlayerControl.CutMode.CUT_SMOOTH:
                gameObject.GetComponent<MeshRenderer>().material = red;
                Debug.Log("red");
                break;
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log(PlayerState.GetComponent<PlayerControl>().eCutMode);
        //// �J�b�g���[�h�ɂ��}�e���A���̐؂�ւ�
        //switch (PlayerState.GetComponent<PlayerControl>().eCutMode)
        //{
        //    // �F
        //    case PlayerControl.CutMode.CUT_ONE:
        //        gameObject.GetComponent<MeshRenderer>().material = blue;
        //        break;
        //    // �ԐF
        //    case PlayerControl.CutMode.CUT_SMOOTH:
        //        gameObject.GetComponent<MeshRenderer>().material = blue;
        //        break;
        //}

       
    }
}
