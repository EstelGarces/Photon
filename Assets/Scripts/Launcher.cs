using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView prefab;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, spawnPoint.rotation);
        player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("playerName"));

        //string nameMenu = PlayerPrefs.GetString("name");
    }
}
