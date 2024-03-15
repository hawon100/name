using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[System.Serializable]
public class RoomSeatList 
{
    public List<bool> RoomSeats = new List<bool>();
}
public class RoomManager : MonoBehaviourPunCallbacks
{
    #region Variable

    public static RoomManager instance;

    private PhotonView photonView;

    public Room CurrentRoom => NetworkManager.instance.CurrentRoom;
    public bool IsJoined
    {
        get
        {
            if (CurrentRoom == null) return false;
            else return true;
        }
    }

    public RoomSeat CurrentRoomSeat = null;

    public RoomSeatList roomSeatList = new RoomSeatList();
    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    #endregion

    #region Function
    public override void OnJoinedRoom()
    {
        GetSeatInfo();
        FindEmptySeat();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _GetSeatInfoWait();
    }

    private void _GetSeatInfo()
    {
        string json = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomSeats"];
        Debug.Log(json);

        if(json != null)
        roomSeatList.RoomSeats = JsonUtility.FromJson<RoomSeatList>(json).RoomSeats;
    }
    public static void GetSeatInfo() => instance._GetSeatInfo();

    private void _GetSeatInfoWait() 
    {
        Thread.Sleep(5000);

        string json = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomSeats"];
        Debug.Log(json);

        if (json != null)
            roomSeatList.RoomSeats = JsonUtility.FromJson<RoomSeatList>(json).RoomSeats;
    }

    private void _SetSeatInfo()
    {
        var currentRoom = PhotonNetwork.CurrentRoom;

        string json = JsonUtility.ToJson(roomSeatList);

        Debug.Log(json);

        bool hasInfo = false;

        foreach (var key in currentRoom.CustomProperties.Keys)
            if (key.Equals("RoomSeats")) { hasInfo = true; break; }

        if (hasInfo)
        {
            currentRoom.CustomProperties["RoomSeats"] = json;
            currentRoom.SetCustomProperties(currentRoom.CustomProperties);
            Debug.Log("Set Property");
        }
        else
        {
            Hashtable ht = new Hashtable();
            ht["RoomSeats"] = json;

            currentRoom.SetCustomProperties(ht);
            Debug.Log("Not Found Property");
        }
    }
    public static void SetSeatInfo() => instance._SetSeatInfo();

    private void _FindEmptySeat()
    {
        Debug.Log(roomSeatList.RoomSeats.Count);
        for (int i = 0; i < roomSeatList.RoomSeats.Count; i++) 
        {
            if (!roomSeatList.RoomSeats[i]) 
            {
                roomSeatList.RoomSeats[i] = true;
                break;
            }
        }
        SetSeatInfo();
    }
    public static void FindEmptySeat() => instance._FindEmptySeat();
    #endregion
}
