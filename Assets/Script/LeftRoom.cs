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
        // ルームから退出する
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

        // UIにフォーカスを移す（例えば、ボタンや入力フィールド）
        Cursor.lockState = CursorLockMode.Locked; // カーソルをロック            
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);  // フォーカスを一度解除
        escapeUI.SetActive(false);
    }
 

}
