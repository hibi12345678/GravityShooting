using UnityEngine;
using Photon.Pun;
public class RotationY : MonoBehaviourPunCallbacks
{

    private float yaw; // ���������̉�]
    void Start()
    {
        // �v���C���[�̉�]��ݒ�i������]�j
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Y�����̉�]���������o���Č��݂̃��[�J����]�ɓK�p����
            float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;

            yaw += mouseX;

            // �v���C���[�̉�]��ݒ�i������]�j
            transform.localRotation = Quaternion.Euler(0, yaw, 0);
        }

    }
}
