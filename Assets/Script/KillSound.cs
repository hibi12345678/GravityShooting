using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
public class KillSound : MonoBehaviourPunCallbacks
{
    int count;
    public GameObject killuiPrefab;
    public TextMeshProUGUI killtextPrefab = default;
    public Transform uiParent;
    private AudioSource[] audioSources;
    GameObject killuiInstance;
    TextMeshProUGUI killtextInstance;
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        count = 0;
    }
    [PunRPC]
    public void KillEnemy(int death ,int kill)
    {
        
        if (gameObject.name == "MainCamera"+kill.ToString())
        {
            count++;
            if (PhotonNetwork.CurrentRoom.PlayerCount== count)
            {
                
                killuiInstance = Instantiate(killuiPrefab, uiParent);
                killuiInstance.transform.localPosition = new Vector3(0, -200, 0);
                killuiInstance.transform.localRotation = Quaternion.identity;
                Destroy(killuiInstance, 2.0f);
                killtextInstance = Instantiate(killtextPrefab, uiParent);
                killtextInstance.text = "Killed Player" + death.ToString();
                killtextInstance.transform.localPosition = new Vector3(0, -200, 0);
                killtextInstance.transform.localRotation = Quaternion.identity;
                Destroy(killtextInstance, 2.0f);
                count = 0;

                PlaySound();
            }

           

        }
        
        
    }

    void PlaySound()
    {
        if (photonView.IsMine)
        {
            audioSources[3].Play();
        }
        
    }
    

}
