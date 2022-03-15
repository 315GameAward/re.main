//======================================================
//
//        Cutter.cs
//        �n�T�~�̐؂鏈��
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================

//======================================================
// �J������
// 2022/02/16 �v���g�^�C�v�쐬�J�n
// �ҏW��:���{���V��
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    //--- Unity�Ɍ��J���Ȃ��p�����[�^
    private bool m_bBiginPoint = false; // �n�ʂɓ��������Ƃ��ɐ؂�n�߂̃|�C���g�����A���ꂪ��������true�ɂ���


    // ���������Őؒf����ꍇ
    //  private Vector3 prePos = Vector3.zero;
    //  private Vector3 prePos2 = Vector3.zero;

    //  void FixedUpdate ()
    //  {
    //      prePos = prePos2;
    //      prePos2 = transform.position;
    //  }

    // ���̃R���|�[�l���g��t�����I�u�W�F�N�g��Collider.IsTrigger��ON�ɂ���
    void OnTriggerEnter(Collider other)
    {
        m_bBiginPoint = true;
        

        //var meshCut = other.gameObject.GetComponent<MeshCut>();
        //if (meshCut == null) { return; }
        ////������݂̂Őؒf������@�A�����ɂ��Ă͓K�X�ύX
        //var cutPlane = new Plane(transform.right, transform.position);
        ////�����Őؒf����ꍇ
        ////var cutPlane = new Plane (Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
        //meshCut.Cut(cutPlane);
    }

    // �n�ʂɓ��������Ƃ�
    void OnTriggerStay(Collider other)
    {
        
    }

    // �n�ʂ��痣�ꂽ�Ƃ�
   void OnTriggerExit(Collider other)
   {
        if(m_bBiginPoint)
        {
            var meshCut = other.gameObject.GetComponent<MeshCut>();
            if (meshCut == null) { return; }
            //������݂̂Őؒf������@�A�����ɂ��Ă͓K�X�ύX
            var cutPlane = new Plane(transform.right, transform.position);
            //�����Őؒf����ꍇ
            //var cutPlane = new Plane (Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
            meshCut.Cut(cutPlane);  // �n�ʂ�؂�
            m_bBiginPoint = false;  // �|�C���g����
        }
   }
}