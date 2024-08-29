using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CountdownTimer : MonoBehaviourPunCallbacks
{

    public float totalTime = 100f;  // �J�E���g�_�E�����鎞�ԁi�b�j

    float timeRemaining;

    public int team1 = 1;
    public int team2 = 0;
    // UI Text�ւ̎Q��
    public Text uiText;
    // �e�L�X�g�̐F��ύX���邽�߂̕ϐ�
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
        // �܂��Q�[���̊J�n�������ݒ肳��Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }
        // �Q�[���̌o�ߎ��Ԃ����߂āA�������ʂ܂ŕ\������
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
            // �ϐ��̒l���e�L�X�g�ɕ\��
            uiText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            // �ϐ��̒l���e�L�X�g�ɕ\��
            uiText.text = minutes.ToString() + ":" + seconds.ToString();
        }

        // �����ɉ����ĐF��ύX�����
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