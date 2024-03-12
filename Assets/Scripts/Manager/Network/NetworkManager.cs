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

    public int PlayerCount
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null) return 0;
            else return PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

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
    public static void ExitRoom()
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LeaveRoom(false);
        else PhotonNetwork.LeaveRoom(true);
    }

    public override void OnConnected() => LobbyManager.SetState("연결 되었습니다 !");
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => LobbyManager.SetState("로비에 참가했습니다 !");

    public override void OnCreatedRoom()
    {
        Debug.Log("방을 생성했습니다 ! / " + CurrentRoomName);
    }
    public override void OnJoinedRoom()
    {
        LobbyManager.SetState("방에 참가했습니다 ! / " + CurrentRoomName);

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

        LocalPlayerSetCP("Ready", false);

        LobbyManager.UIPresetMove(-3840, 0, 0.5f);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("방에 유저가 접속했습니다 !");

        UpdatePlayerList();
        LobbyManager.SetPlayerList(PlayerList);
    }
    public override void OnLeftRoom()
    {
        CurrentRoomName = "";
        PlayerNumCP = "";

        DeleteLocalPlayerCP("Ready");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("RoomListUpdate");
        RoomList = LobbyManager.RoomInfoToRoomData(roomList);

        LobbyManager.RoomListUpdate(RoomList);
        foreach (var room in roomList) Debug.Log(room.Name); 
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

    private void _LocalPlayerSetCP(string name, bool value)
    {
        var player = PhotonNetwork.LocalPlayer;

        Hashtable ht = new Hashtable();
        ht[name] = value;

        Debug.Log("LocalPlayerSetCP / Result : " + ht[name]);

        player.SetCustomProperties(ht);
    }
    public void LocalPlayerSetCP(string name, bool value) => instance._LocalPlayerSetCP(name, value);

    private bool _GetOtherPlayerCP(int index, string name)
    {
        var player = PhotonNetwork.PlayerListOthers[index];
        bool result = (bool)player.CustomProperties[name];

        Debug.Log("GetOtherPlayerCP / Result = " + player.CustomProperties[name]);
        Debug.Log("NickName : " + player.NickName);

        return result;
    }
    public static bool GetOtherPlayerCP(int index, string name) => instance._GetOtherPlayerCP(index, name);

    private void _DeleteLocalPlayerCP(string name)
    {
        var player = PhotonNetwork.LocalPlayer;
        player.CustomProperties[name] = null;
    }
    public static void DeleteLocalPlayerCP(string name) => instance._DeleteLocalPlayerCP(name);

    private void _SetValueLocalPlayerCP(string name, bool value)
    {
        var player = PhotonNetwork.LocalPlayer;
        player.CustomProperties[name] = value;
    }
    public static void SetValueLocalPlayerCP(string name, bool value) => instance._SetValueLocalPlayerCP(name, value);

    private void _SetNickName(string name)
    {
        PhotonNetwork.LocalPlayer.NickName = name;
        NickName = PhotonNetwork.LocalPlayer.NickName;
    }
    public static void SetNickName(string name) => instance._SetNickName(name); // LocalPlayer의 닉네임을 설정합니다.

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
