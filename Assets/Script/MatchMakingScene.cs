using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchMakingScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        
    }

    public override void OnJoinedRoom()
    {
 
    
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }

        // ���[���������ɂȂ�����A�ȍ~���̃��[���ւ̎Q����s���ɂ���
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

}
