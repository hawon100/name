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
    public int PlayerNum = 0;

    public string CurrentRoomName = "";
    public string PlayerNumCP = "";

    public List<string> PlayerList = new List<string>();


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
    public static void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);

        instance.CurrentRoomName = roomName;
    }
    public static void CreateRoom(string roomName, RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(roomName, roomOptions);

        instance.CurrentRoomName = roomName;
    }

    public override void OnConnected() => LobbyManager.SetState("연결 되었습니다 !");
    public override void OnCreatedRoom()
    {
        LobbyManager.SetState("방을 생성했습니다 ! / " + CurrentRoomName);


    }
    public override void OnJoinedRoom()
    {
        LobbyManager.SetState("방에 참가했습니다 ! / " + CurrentRoomName);

        Hashtable roomCP = PhotonNetwork.CurrentRoom.CustomProperties;

        float roomPlayerNum = 1;

        if (PhotonNetwork.IsMasterClient)
        {

            PlayerNumCP = CurrentRoomName + "_roomPlayerNum";
            _CurrentRoomSetCP(PlayerNumCP, roomPlayerNum);

            PlayerNum = (int)roomPlayerNum;
        }
        else
        {

            roomPlayerNum += 1;

            PlayerNum = (int)roomPlayerNum;
        }

        List<string> playerList = new List<string>();

        foreach (var item in PhotonNetwork.CurrentRoom.Players) playerList.Add(item.Value.NickName);
        PlayerList = playerList;
    }
    public override void OnLeftRoom()
    {
        CurrentRoomName = "";
        PlayerNumCP = "";
    }

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
    }
    public static void UpdatePlayerList() => instance._UpdatePlayerList();

    private List<string> _GetPlayerList()
    {
        return PlayerList;
    }
    public static List<string> GetPlayerList() => instance._GetPlayerList();
    #endregion
}
