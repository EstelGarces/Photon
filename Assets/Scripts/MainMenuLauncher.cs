using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLauncher : MonoBehaviourPunCallbacks
{
    public TMP_InputField name;
    public TMP_Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickConnect()
    {
        if (name != null) 
        {
            PhotonNetwork.NickName = name.text;
            PlayerPrefs.SetString("playerName", name.text);
            buttonText.text = "Conectando...";
            PhotonNetwork.ConnectUsingSettings();

        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
