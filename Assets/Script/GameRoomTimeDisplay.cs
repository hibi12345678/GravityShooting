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
    }
    private void Update()
    {
        // �܂����[���ɎQ�����Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.InRoom) { return; }
        // �܂��Q�[���̊J�n�������ݒ肳��Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }
        UpdatePlayerCount();
        if (playerCount == 4)
        {
            timeLabel.text = "";
            waitingText.text = "";
        }
        else{
            // �Q�[���̌o�ߎ��Ԃ����߂āA�������ʂ܂ŕ\������
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