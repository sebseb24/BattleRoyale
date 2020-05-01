using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;

    // instance
    public static NetworkManager instance;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        // Connect to the master server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom() {
        Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        GameManager.instance.alivePlayers--;
        GameUI.instance.UpdatePlayerInfoText();

        if (PhotonNetwork.IsMasterClient) {
            GameManager.instance.CheckWinCondition();
        }
    }

    public void CreateRoom(string roomName) {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
