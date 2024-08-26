using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MainScene : MonoBehaviourPunCallbacks
{
    int count;
   
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.CurrentRoom.IsOpen = false;

        count = 0;
        
        CreatePlayer();

    }


    public void CreatePlayer()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            var position = new Vector3(-2f, 15.0f, -2f);
            GameObject obj = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
            

            
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {      
            var position = new Vector3(-2f, -15.0f, 2f);
            GameObject obj = PhotonNetwork.Instantiate("Player2", position, Quaternion.identity);
            
            
        }

        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {

            var position = new Vector3(2f, 15.0f, 2f);
            GameObject obj = PhotonNetwork.Instantiate("Player3", position, Quaternion.identity);
            
            

        }


        else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
       
            var position = new Vector3(2f, -15.0f, 2f);
            GameObject obj = PhotonNetwork.Instantiate("Player4", position, Quaternion.identity);
            
            

        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                var position = new Vector3(-2f, -15.0f, -2f);
                GameObject obj = PhotonNetwork.Instantiate("Player3-1", position, Quaternion.identity);
                position = new Vector3(2f, 15.0f, 2f);
                obj = PhotonNetwork.Instantiate("Player3-2", position, Quaternion.identity);
                position = new Vector3(2f, -15.0f,2f);
                obj = PhotonNetwork.Instantiate("Player3-3", position, Quaternion.identity);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                var position = new Vector3(2f, 15.0f,2f);
                GameObject obj = PhotonNetwork.Instantiate("Player3-2", position, Quaternion.identity);
                position = new Vector3(2f, -15.0f, Random.Range(-2f, 2f));
                obj = PhotonNetwork.Instantiate("Player3-3", position, Quaternion.identity);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                var position = new Vector3(2f, -15.0f, 2f);
                GameObject obj = PhotonNetwork.Instantiate("Player3-3", position, Quaternion.identity);
            }
        }
        

        


    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedRoom()
    {


        // ルームを作成したプレイヤーは、現在のサーバー時刻をゲームの開始時刻に設定する
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }
}


