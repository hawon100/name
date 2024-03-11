using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region # Variable
    public static NetworkManager instance;

    public string NickName;
    public int PlayerNum = 0;

    public string CurrentRoomName = "";
    public string PlayerNumCP = "";

    public List<string> PlayerList = new List<string>();
    public List<Data> RoomList = new List<Data>();
    #endregion

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

    public override void OnConnected() => LobbyManager.SetState("���� �Ǿ����ϴ� !");
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => LobbyManager.SetState("�κ� �����߽��ϴ� !");

    public override void OnCreatedRoom()
    {
        LobbyManager.SetState("���� �����߽��ϴ� ! / " + CurrentRoomName);
    }
    public override void OnJoinedRoom()
    {
        LobbyManager.SetState("�濡 �����߽��ϴ� ! / " + CurrentRoomName);

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

        UpdatePlayerList();
        LobbyManager.SetPlayerList(PlayerList);

        LobbyManager.UIPresetMove(-3840, 0, 0.5f);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("�濡 ������ �����߽��ϴ� !");

        UpdatePlayerList();
        LobbyManager.SetPlayerList(PlayerList);
    }
    public override void OnLeftRoom()
    {
        CurrentRoomName = "";
        PlayerNumCP = "";
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = LobbyManager.RoomInfoToRoomData(roomList);

        LobbyManager.RoomListUpdate(RoomList);
    }

    private void _CurrentRoomSetCP(string name, float value) // ���õ� �濡 Custom Property�� �߰��մϴ�. (float) 
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht[name] = value;
            currentRoom.SetCustomProperties(ht);
        }
    }
    private void _CurrentRoomSetCP(string name, string value) // ���õ� �濡 Custom Property�� �߰��մϴ�. (string) 
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
    public static void SetNickName(string name) => instance._SetNickName(name); // LocalPlayer�� �г����� �����մϴ�.

    private void _UpdatePlayerList()
    {
        List<string> playerList = new List<string>();

        foreach (var item in PhotonNetwork.CurrentRoom.Players) playerList.Add(item.Value.NickName);
        PlayerList = playerList;
    }
    public static void UpdatePlayerList() => instance._UpdatePlayerList();

    private List<string> _GetPlayerList()
    {
        return PlayerList;
    }
    public static List<string> GetPlayerList() => instance._GetPlayerList();
    #endregion
}
