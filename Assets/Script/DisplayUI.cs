using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class DisplayUI : MonoBehaviourPunCallbacks
{
    public GameObject escapeUI;
    bool flag;
    void Start()
    {
        flag = false;
    }

    void Update()
    {

        // エスケープキーが押されたかどうかをチェック
        if (Input.GetKeyDown(KeyCode.Escape) && flag == false)
        {

            // UIにフォーカスを移す（例えば、ボタンや入力フィールド）
            EventSystem.current.SetSelectedGameObject(escapeUI);  // フォーカスを一度解除
            escapeUI.SetActive(true);
        }
        // エスケープキーが押されたかどうかをチェック
        else if (Input.GetKeyDown(KeyCode.Escape) && flag == true)
        {

            // UIにフォーカスを移す（例えば、ボタンや入力フィールド）
            EventSystem.current.SetSelectedGameObject(null);  // フォーカスを一度解除
            escapeUI.SetActive(false);
        }

    }
}