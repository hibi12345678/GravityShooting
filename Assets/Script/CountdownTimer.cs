using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CountdownTimer : MonoBehaviourPunCallbacks
{

    public float totalTime = 100f;  // カウントダウンする時間（秒）

    float timeRemaining;

    public int team1 = 1;
    public int team2 = 0;
    // UI Textへの参照
    public Text uiText;
    // テキストの色を変更するための変数
    private Color textColor = Color.white;
    float elapsedTime;
    public float currentTime;
    
    void Start()
    {
        
        currentTime = 100f;
        timeRemaining = totalTime;
        UpdateTimerText();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }

    void Update()
    {
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }
        // ゲームの経過時間を求めて、小数第一位まで表示する
        elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        if (currentTime > 0)
        {
            currentTime = timeRemaining - elapsedTime;

            if (currentTime < 0)
            {
                currentTime = 0;
                
            }
            UpdateTimerText();


        }
    }

  
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        if (seconds < 10)
        {
            // 変数の値をテキストに表示
            uiText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            // 変数の値をテキストに表示
            uiText.text = minutes.ToString() + ":" + seconds.ToString();
        }

        // 条件に応じて色を変更する例
        if (currentTime <= 10)
        {
            uiText.color = Color.red;
        }
        else
        {
            uiText.color = Color.white;
        }

    }
}