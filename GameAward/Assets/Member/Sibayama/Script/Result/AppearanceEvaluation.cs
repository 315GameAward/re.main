using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceEvaluation : MonoBehaviour
{
    // �]��
    public enum EvaType
    {
        Too_Bad,
        Good,
        Great,

        Max_Type
    }

    // �]���Ɏg���X�^���v�e�N�X�`��
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //switch ()
        //{
        //    default:
        //        break;
        //}
    }
}
