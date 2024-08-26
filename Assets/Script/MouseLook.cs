using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MouseLock : MonoBehaviourPunCallbacks
{
    public Transform player; // プレイヤーキャラクター
    public float mouseSensitivity = 200f; // マウスの感度

    public GameObject escapeUI;
    private float yaw; // 水平方向の回転
    private float pitch; // 垂直方向の回転
    bool flag;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // カーソルをロック
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

            // x軸の回転角度を -90 から 90 度に制限
            pitch = Mathf.Clamp(pitch, -45f, 45f);

            // カメラのローカル回転を設定
            transform.localRotation = Quaternion.Euler(pitch, 0, 0);

            // エスケープキーが押されたかどうかをチェック
            if (Input.GetKeyDown(KeyCode.Escape) && flag == false)
            {

                escapeUI.SetActive(true);
                // UIにフォーカスを移す（例えば、ボタンや入力フィールド）
                EventSystem.current.SetSelectedGameObject(escapeUI);  // フォーカスを一度解除
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                flag = true;

            }

            // エスケープキーが押されたかどうかをチェック
            else if (Input.GetKeyDown(KeyCode.Escape) && flag == true)
            {
        
                EventSystem.current.SetSelectedGameObject(null);
                escapeUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; // カーソルをロック            
                Cursor.visible = false;
                flag = false;
            }
            


        }

       
    }
}