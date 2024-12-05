using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView prefab;
    public Transform spawnPoint;
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerCountText;
    public TextMeshProUGUI playerListText;
    public TMP_InputField roomNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Conectando a sala");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se encontro ninguna sala . Creando una nueva...");

        string generatedRoomName = $" { Random.Range(1000, 9999)}"; 
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "visibleName", generatedRoomName }
            },
            CustomRoomPropertiesForLobby = new string[] { "visibleName" }
        };

        PhotonNetwork.CreateRoom(generatedRoomName, roomOptions);
        Debug.Log($"Sala creada con nombre: {generatedRoomName}");
    }

    public override void OnJoinedRoom()
    {
        /*GameObject player = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, spawnPoint.rotation);
        player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("playerName"));

        string nameMenu = PlayerPrefs.GetString("name");*/

        Debug.Log($"Unido a la sala: {PhotonNetwork.CurrentRoom.Name}");
        UpdateRoomInfo();

        if (prefab != null && spawnPoint != null) 
        {
            GameObject player = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position,spawnPoint.rotation);
            player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
        else
        {
            Debug.LogError("Prefab del jugador o punto de aparicion no configurado");
        }

        if (PhotonNetwork.IsMasterClient && roomNameInputField != null)
        {
            roomNameInputField.interactable = true;
        }
        else if (roomNameInputField != null) 
        {
            roomNameInputField.interactable = false;
        }
    }

    public void ChangeRoomName(string newName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable {
                { "visibleName", newName }
            });
            Debug.Log($"Nombre de la sala camiado a: {newName}");
        }
        else 
        {
            Debug.LogError("Solo el Master Client puede cambiar el nombre de la sala.");
        }
    }

    void UpdateRoomInfo()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("visibleName")) 
        {
            string visibleName = (string)PhotonNetwork.CurrentRoom.CustomProperties["visibleName"];
            roomNameText. text = $"Sala: {visibleName}";
            Debug.Log($"Nombre de la sala mostrado: {visibleName}");
        }
        else
        {
            roomNameText.text = $"Sala: {PhotonNetwork.CurrentRoom.Name}";
            Debug.LogWarning("No se encontro 'visibleName' usando el nombre interno de la sala");
        }

        if (playerCountText != null)
            playerCountText.text = $"Jugadores: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";

        if (playerListText != null)
        {
            playerListText.text = "Jugadores en la sala: \n";
            foreach (var player in PhotonNetwork.PlayerList)
                playerListText.text += $"- {player.NickName}\n";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} se unio a la sala.");
        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} salio de la sala.");
        UpdateRoomInfo();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("visibleName"))
        {
            string updateName = (string)propertiesThatChanged["visibleName"];
            Debug.Log($"Nombre de la sala acutalizado");
            roomNameText.text = $"Sala: {updateName}";
        }
        else 
        {
            Debug.LogWarning("No se encontro en las propiedades actualizadas");
        }
    }

    public void OnChangeRoomName()
    {
        if (roomNameInputField != null)
        {
            ChangeRoomName(roomNameInputField.text);
        }
    }
}
