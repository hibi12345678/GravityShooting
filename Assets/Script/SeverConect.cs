using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class SeverConect : MonoBehaviourPunCallbacks
{

    void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���āA�}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        Debug.Log("�}�X�^�[�T�[�o�[�ɐڑ����܂���");
    }

    // Photon�̃T�[�o�[����ؒf���ꂽ���ɌĂ΂��R�[���o�b�N
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"�T�[�o�[�Ƃ̐ڑ����ؒf����܂���: {cause.ToString()}");
    }

}
