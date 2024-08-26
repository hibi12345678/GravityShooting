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

        // �G�X�P�[�v�L�[�������ꂽ���ǂ������`�F�b�N
        if (Input.GetKeyDown(KeyCode.Escape) && flag == false)
        {

            // UI�Ƀt�H�[�J�X���ڂ��i�Ⴆ�΁A�{�^������̓t�B�[���h�j
            EventSystem.current.SetSelectedGameObject(escapeUI);  // �t�H�[�J�X����x����
            escapeUI.SetActive(true);
        }
        // �G�X�P�[�v�L�[�������ꂽ���ǂ������`�F�b�N
        else if (Input.GetKeyDown(KeyCode.Escape) && flag == true)
        {

            // UI�Ƀt�H�[�J�X���ڂ��i�Ⴆ�΁A�{�^������̓t�B�[���h�j
            EventSystem.current.SetSelectedGameObject(null);  // �t�H�[�J�X����x����
            escapeUI.SetActive(false);
        }

    }
}