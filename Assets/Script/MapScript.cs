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
        // �����̃v���C���[�ł���΃J������L���ɂ��A���̃v���C���[�ł���Ζ����ɂ���
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
