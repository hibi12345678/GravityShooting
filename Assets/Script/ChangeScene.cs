using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public GameObject text;
    AudioSource audioSource;
    bool flag = false;
    // �w�肳�ꂽ�V�[�����ɐ؂�ւ���
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

    }
    public void Change()
    {
        
        if(flag == false)
        {
            audioSource.Play();
            flag = true;
        }
        // 3�b��Ƀ��\�b�h���Ăяo��
        Invoke("DelayedMethod", 2f);

        
    }

    void DelayedMethod()
    {
        SceneManager.LoadScene("MatchmakingScene");
        
    }
}
