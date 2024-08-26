using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class LeftRoom : MonoBehaviourPunCallbacks
{
    public GameObject escapeUI;
    public void LeaveRoom()
    {
        // ���[������ޏo����
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MatchMakingScene");

    }

    public void LeaveLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("StartScene");
    }


    public void UIClose()
    {

        // UI�Ƀt�H�[�J�X���ڂ��i�Ⴆ�΁A�{�^������̓t�B�[���h�j
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N            
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);  // �t�H�[�J�X����x����
        escapeUI.SetActive(false);
    }
 

}
