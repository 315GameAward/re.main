using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomMovement : MonoBehaviour
{
    private float chargeTime = 5.0f;
    private float timeCount;
    void Update()
    {
        timeCount += Time.deltaTime;
        // �����őO�i����B
        transform.position += transform.forward * Time.deltaTime;
        // �w�肵�����Ԃ��o�߂���ƁA
        if (timeCount > chargeTime)
        {
            // �i�H�������_���ɕύX����B
            Vector3 course = new Vector3(0, Random.Range(0, 180), 0);
            transform.localRotation = Quaternion.Euler(course);
            // �^�C���J�E���g��0�ɖ߂��B
            timeCount = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Փ˔���
        if (collision.gameObject.tag == "test")
        {
            //�X�R�A������ǉ�
            FindObjectOfType<Score>().AddScore(10);

            //����̃^�O��Scicssor�ł���Ȃ�΁A�����������ɕς���
            Destroy(this.gameObject);
        }

    }
    //void GameC_O()
    //{
    //    if (Destroy(this.gameObject))
    //    {
    //        
    //    }
    //}
}