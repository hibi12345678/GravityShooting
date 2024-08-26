using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public GameObject text;
    AudioSource audioSource;
    bool flag = false;
    // 指定されたシーン名に切り替える
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
        // 3秒後にメソッドを呼び出す
        Invoke("DelayedMethod", 2f);

        
    }

    void DelayedMethod()
    {
        SceneManager.LoadScene("MatchmakingScene");
        
    }
}
