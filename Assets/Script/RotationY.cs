using UnityEngine;
using Photon.Pun;
public class RotationY : MonoBehaviourPunCallbacks
{

    private float yaw; // 水平方向の回転
    void Start()
    {
        // プレイヤーの回転を設定（水平回転）
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Y軸回りの回転だけを取り出して現在のローカル回転に適用する
            float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;

            yaw += mouseX;

            // プレイヤーの回転を設定（水平回転）
            transform.localRotation = Quaternion.Euler(0, yaw, 0);
        }

    }
}
