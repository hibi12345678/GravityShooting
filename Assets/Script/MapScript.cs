using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
public class MapScript : MonoBehaviourPunCallbacks
{
    public Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        // 自分のプレイヤーであればカメラを有効にし、他のプレイヤーであれば無効にする
        if (photonView.IsMine)
        {
            myCamera.enabled = true;
        }
        else
        {
            myCamera.enabled = false;
        }
    }

    
}
