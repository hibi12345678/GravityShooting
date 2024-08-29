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
        // �܂����[���ɎQ�����Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.InRoom) { return; }
        // �܂��Q�[���̊J�n�������ݒ肳��Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        // �Q�[���̌o�ߎ��Ԃ����߂āA�������ʂ܂ŕ\������
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