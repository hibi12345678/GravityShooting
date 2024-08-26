using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class SeverConect : MonoBehaviourPunCallbacks
{

    void Start()
    {
        // PhotonServerSettingsの設定内容を使って、マスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
    }

    // Photonのサーバーから切断された時に呼ばれるコールバック
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
    }

}
