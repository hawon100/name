using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    public string NickName;

    public List<string> PlayerNickNameList = new List<string>(4);

    #region # Unity_Function
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        ConnectUsingSettings();
    }
    #endregion

    #region # Function

    public static void ConnectUsingSettings() => PhotonNetwork.ConnectUsingSettings();
    public static void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);
    public static void CreateRoom(string roomName, RoomOptions roomOptions) => PhotonNetwork.CreateRoom(roomName, roomOptions);

    public override void OnConnected() => Debug.Log("OnConnected");
    public override void OnCreatedRoom() => Debug.Log("OnCreatedRoom");
    public override void OnJoinedRoom() => Debug.Log("OnJoinedRoom");

    private void _CurrentRoomSetCP(string name, float value) // 선택된 방에 Custom Property를 추가합니다. (float) 
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht[name] = value;
            currentRoom.SetCustomProperties(ht);
        }
    }
    private void _CurrentRoomSetCP(string name, string value) // 선택된 방에 Custom Property를 추가합니다. (string) 
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht[name] = value;
            currentRoom.SetCustomProperties(ht);
        }
    }

    public static void CurrentRoomSetCP(string name, float value) => instance._CurrentRoomSetCP(name, value);
    public static void CurrentRoomSetCP(string name, string value) => instance._CurrentRoomSetCP(name, value);

    private void _SetNickName(string name)
    {
        PhotonNetwork.LocalPlayer.NickName = name;
        NickName = PhotonNetwork.LocalPlayer.NickName;
    }
    public static void SetNickName(string name) => instance._SetNickName(name); // LocalPlayer의 닉네임을 설정합니다.

    private void _UpdatePlayerList()
    {
        var playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            PlayerNickNameList[i] = playerList[i].NickName;
        }
    }
    public static void UpdatePlayerList() => instance._UpdatePlayerList();

    #endregion
}
