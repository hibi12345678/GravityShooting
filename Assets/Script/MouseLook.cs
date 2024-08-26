using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MouseLock : MonoBehaviourPunCallbacks
{
    public Transform player; // �v���C���[�L�����N�^�[
    public float mouseSensitivity = 200f; // �}�E�X�̊��x

    public GameObject escapeUI;
    private float yaw; // ���������̉�]
    private float pitch; // ���������̉�]
    bool flag;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        Cursor.visible = false;
        flag = false;

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            pitch -= mouseY;

            // x���̉�]�p�x�� -90 ���� 90 �x�ɐ���
            pitch = Mathf.Clamp(pitch, -45f, 45f);

            // �J�����̃��[�J����]��ݒ�
            transform.localRotation = Quaternion.Euler(pitch, 0, 0);

            // �G�X�P�[�v�L�[�������ꂽ���ǂ������`�F�b�N
            if (Input.GetKeyDown(KeyCode.Escape) && flag == false)
            {

                escapeUI.SetActive(true);
                // UI�Ƀt�H�[�J�X���ڂ��i�Ⴆ�΁A�{�^������̓t�B�[���h�j
                EventSystem.current.SetSelectedGameObject(escapeUI);  // �t�H�[�J�X����x����
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                flag = true;

            }

            // �G�X�P�[�v�L�[�������ꂽ���ǂ������`�F�b�N
            else if (Input.GetKeyDown(KeyCode.Escape) && flag == true)
            {
        
                EventSystem.current.SetSelectedGameObject(null);
                escapeUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N            
                Cursor.visible = false;
                flag = false;
            }
            


        }

       
    }
}