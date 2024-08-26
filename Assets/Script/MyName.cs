using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyName : MonoBehaviour
{

    public GameObject parent;

    void Start()
    {
        if(gameObject.name == "Name1")
        {
            if(parent.name == "Player3(Clone)" || parent.name == "Player4(Clone)")
            {
                Destroy(gameObject);
            }
        }
        
        else if (gameObject.name == "Name2")
        {
            if (parent.name == "Player1(Clone)" || parent.name == "Player2(Clone)")
            {
                Destroy(gameObject);
            }
        }
       
    }
    
}
