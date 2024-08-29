using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomPlayerCount : MonoBehaviourPunCallbacks
{
    public Text playerCountText; // Unity��UI Text���A�T�C��
    float time;
    private TextMeshProUGUI timeLabel;
    public GameObject timer;
    int playerCount;
    bool flag = false;
    // ���[���Q�����ɌĂ΂��
    public override void OnJoinedRoom()
    {
        UpdatePlayerCount();
    }

    // �v���C���[�����[���ɓ������Ƃ��ɌĂ΂��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
    }

    // �v���C���[�����[����ޏo�����Ƃ��ɌĂ΂��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    // �v���C���[�����X�V����
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
        // �܂����[���ɎQ�����Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.InRoom) { return; }
        // �܂��Q�[���̊J�n�������ݒ肳��Ă��Ȃ��ꍇ�͍X�V���Ȃ�
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