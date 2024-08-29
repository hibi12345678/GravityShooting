using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomPlayerCount : MonoBehaviourPunCallbacks
{
    public Text playerCountText; // UnityのUI Textをアサイン
    float time;
    private TextMeshProUGUI timeLabel;
    public GameObject timer;
    int playerCount;
    bool flag = false;
    // ルーム参加時に呼ばれる
    public override void OnJoinedRoom()
    {
        UpdatePlayerCount();
    }

    // プレイヤーがルームに入ったときに呼ばれる
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
    }

    // プレイヤーがルームを退出したときに呼ばれる
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    // プレイヤー数を更新する
    void UpdatePlayerCount()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCountText.text = playerCount.ToString() + "/4";

    }
    void Start()
    {
        timeLabel = timer.GetComponent<TextMeshProUGUI>();
       
    }
    void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        timeLabel.text = "";

        
        

        if (playerCount == 4)
        { 
            
            if(flag == false)
            {
                time = unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f;
                flag = true;
            }
            playerCountText.text = "4/4";
            timeLabel.text = (time + 5.0f - elapsedTime).ToString("f1");
            PhotonNetwork.IsMessageQueueRunning = false;
            if (time + 5.0f - elapsedTime < 0)
                 ChangeScene();
            
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}