using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class GameRoomTimeDisplay : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI timeLabel;
    public Text playerCountText;

    public Text waitingText;
    int playerCount;
    private void Start()
    {
        timeLabel = GetComponent<TextMeshProUGUI>();
       
    }

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
    }
    private void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }
        UpdatePlayerCount();
        if (playerCount == 4)
        {
            timeLabel.text = "";
            waitingText.text = "";
        }
        else{
            // ゲームの経過時間を求めて、小数第一位まで表示する
            float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
            timeLabel.text = (35.0f - elapsedTime).ToString("f1");
            if (elapsedTime > 30.0f)
            {
                playerCountText.text = "4/4";

                if (35.0f - elapsedTime < 0)
                {
                    ChangeScene();
                    waitingText.text = "";
                }
                    
            }

        }

    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}