using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public TextMeshPro playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void SetNameText(string name) { 
        playerNameText.text = name;
    }

}
