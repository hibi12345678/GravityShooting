using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameRoomTimeDisplay : MonoBehaviour
{
    private TextMeshProUGUI timeLabel;
    public Text playerCountText;
    private void Start()
    {
        timeLabel = GetComponent<TextMeshProUGUI>();
       
    }

    private void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        // ゲームの経過時間を求めて、小数第一位まで表示する
        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        
        if (elapsedTime > 30.0f)
        {
            playerCountText.text = "4/4";
            timeLabel.text = (35.0f - elapsedTime).ToString("f1");
            if (65.0f - elapsedTime < 0)
                ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}