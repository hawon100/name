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

    private PhotonView pv;
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

    public Room CurrentRoom
    {
        get
        {
            return PhotonNetwork.CurrentRoom;
        }
    }

    public string CurrentRoomName = "";
    public string PlayerNumCP = "";

    public List<string> PlayerList = new List<string>();
    public List<bool> RoomSeatList = new List<bool>();

    public List<Data> RoomList = new List<Data>();

    public bool HasSeat = false;
    public int SeatNum = 0;

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
        pv = GetComponent<PhotonView>();
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

    public override void OnConnected() => LobbyManager.SetState("���� �Ǿ����ϴ� !");
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("���� �����߽��ϴ� ! / " + CurrentRoomName);
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 4; i++) RoomSeatList.Add(false);

            Hashtable ht = new Hashtable();
            ht["RoomSeat"] = RoomSeatList.ToArray();
            PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        }
    }
    public override void OnJoinedRoom()
    {
        LobbyManager.SetState("�濡 �����߽��ϴ� ! / " + CurrentRoomName);

        float roomPlayerNum = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            PlayerNumCP = CurrentRoomName + "_roomPlayerNum";
            _CurrentRoomSetCP(PlayerNumCP, roomPlayerNum);

            PlayerNum = (int)roomPlayerNum;
            LobbyManager.SetMasterClient();
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

        UpdatePlayerList();
        LobbyManager.SetPlayerList(PlayerList);
    }
    public override void OnLeftRoom()
    {
        CurrentRoomName = "";
        PlayerNumCP = "";

        RoomSeatList = null;

        DeleteLocalPlayerCP("Ready");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = LobbyManager.RoomInfoToRoomData(roomList);

        LobbyManager.RoomListUpdate(RoomList);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
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
    private void _CurrentRoomSetCP(string name, bool value)
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht[name] = value;
            currentRoom.SetCustomProperties(ht);
        }
    }
    private void _CurrentRoomSetCP(string name, int value)
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
    public static void CurrentRoomSetCP(string name, bool value) => instance._CurrentRoomSetCP(name, value);
    public static void CurrentRoomSetCP(string name, int value) => instance._CurrentRoomSetCP(name, value);

    private bool _CurrentBoolRoomGetCP(string name)
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        if (currentRoom == null) Debug.Log("null");
        else Debug.Log(PhotonNetwork.CurrentRoom.Name);

        return (bool)currentRoom.CustomProperties[name];
    }
    private int _CurrentIntRoomGetCP(string name)
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        Hashtable ht = currentRoom.CustomProperties;

        foreach (var key in ht.Keys) Debug.Log(key);

        return (int)currentRoom.CustomProperties[name];
    }
    public static bool CurrentBoolRoomGetCP(string name) => instance._CurrentBoolRoomGetCP(name);
    public static int CurrentIntRoomGetCP(string name) => instance._CurrentIntRoomGetCP(name);

    private void _SetValueCurrentRoomCP(string name, bool value)
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        currentRoom.CustomProperties[name] = value;
    }
    private void _SetValueCurrentRoomCP(string name, int value)
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        currentRoom.CustomProperties[name] = value;
    }
    public static void SetValueCurrentRoomCP(string name, bool value) => instance._SetValueCurrentRoomCP(name, value);
    public static void SetValueCurrentRoomCP(string name, int value) => instance._SetValueCurrentRoomCP(name, value);
    private void _LocalPlayerSetCP(string name, bool value)
    {
        var player = PhotonNetwork.LocalPlayer;

        Hashtable ht = new Hashtable();
        ht[name] = value;

        Debug.Log("LocalPlayerSetCP / Result : " + ht[name]);

        player.SetCustomProperties(ht);
    }
    private void _LocalPlayerSetCP(string name, int value)
    {
        var player = PhotonNetwork.LocalPlayer;

        Hashtable ht = new Hashtable();
        ht[name] = value;

        Debug.Log("LocalPlayerSetCP / Result : " + ht[name]);

        player.SetCustomProperties(ht);
    }
    public static void LocalPlayerSetCP(string name, bool value) => instance._LocalPlayerSetCP(name, value);
    public static void LocalPlayerSetCP(string name, int value) => instance._LocalPlayerSetCP(name, value);

    private bool _GetBoolLocalPlayerCP(string name)
    {
        var player = PhotonNetwork.LocalPlayer;

        return (bool)player.CustomProperties[name];
    }
    public static bool GetBoolLocalPlayerCP(string name) => instance._GetBoolLocalPlayerCP(name);
    private int _GetIntLocalPlayerCP(string name)
    {
        var player = PhotonNetwork.LocalPlayer;

        return (int)player.CustomProperties[name];
    }
    public static int GetIntLocalPlayerCP(string name) => instance._GetIntLocalPlayerCP(name);

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
