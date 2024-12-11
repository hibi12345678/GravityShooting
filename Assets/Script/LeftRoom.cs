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

    public void Quit()
    {   // ���[������ޏo����
        PhotonNetwork.LeaveRoom();
        // �A�v���P�[�V�������I������
        Application.Quit();

        // Unity�G�f�B�^���ł̓���m�F�p�i�r���h��͕s�v�j
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void UIClose()
    {

        
        EventSystem.current.SetSelectedGameObject(null);  // �t�H�[�J�X����x����
        escapeUI.SetActive(false);
    }


}
