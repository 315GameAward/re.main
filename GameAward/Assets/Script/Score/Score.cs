using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI�̃p�����[�^��ύX����
using UnityEngine.UI;

//���̃X�N���v�g�œ��_�����Z����̂ł͂Ȃ��A
//���_�̕\�����s��

public class Score : MonoBehaviour
{
    //-----�X�R�A�̕\��
    public Text scoreText;

    //-----�n�C�X�R�A��\������
    //public Text highScoreText;

    //-----�X�R�A
    private int score;

    //-----�n�C�X�R�A
    //private int highScore;

    //-----�ۑ����邽�߂̃L�[�i�������j

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //�X�R�A���n�C�X�R�A��葽�����
        //if (highScore < score)
        //{
        //    highScore = score;
        //}

        //�X�R�A�E�n�C�X�R�A��\������
        //�X�R�A
        scoreText.text = score.ToString();

        //�n�C�X�R�A
        //highscoretext.text = highscore.tostring();


    }

    private void Initialize()
    {
        //�X�R�A��0�ɖ߂�
        score = 0;

        //�n�C�X�R�A
    }

    //
    public void AddScore(int point)
    {
        score = score + point;
    }

    //�n�C�X�R�A�̕ێ�
    public void Save()
    {
        //�n�C�X�R�A��ێ�����

        //�Q�[���J�n�O�̏�Ԃɖ߂�
        Initialize();
    }

}
