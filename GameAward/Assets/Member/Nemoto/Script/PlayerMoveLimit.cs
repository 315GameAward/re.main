using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveLimit : MonoBehaviour
{
    Camera targetCamera;
    [SerializeField] Transform targetObj;

    Vector3 lb;
    Vector3 rt;
    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();

        lb = GetRayhitPos(new Vector3(0, 0, 0)); // ����
        rt = GetRayhitPos(new Vector3(1, 1, 0)); // �E��
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckInScreen())
            Debug.Log("��ʓ�");
        else
            Debug.Log("��ʊO");

    }

    Vector3 GetRayhitPos(Vector3 targetPos)
    {
        Ray ray = targetCamera.ViewportPointToRay(targetPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;

        return Vector3.zero; // �����q�b�g���Ȃ����(0, 0, 0)��Ԃ�
    }

    // �Ώۂ̃I�u�W�F�N�g�̈ʒu�����ʓ����ǂ������肵�ĕԂ�
    bool CheckInScreen()
    {
        // �e�X�g���̃J�������ςȕ����������Ă��̂ŏ����������Ȕ��蕶�ɂȂ��Ă��܂�
        // �����̏������͓K�X�A�C�����Ă�������
        if (targetObj.position.x < rt.x || lb.x < targetObj.position.x || targetObj.position.z < lb.z || rt.z < targetObj.position.z)
            return false;

        return true;
    }
}
