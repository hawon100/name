using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

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
    #endregion

    #region # Function

    public static void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

    public static void CreateRoom(string roomName, RoomOptions roomOptions) => PhotonNetwork.CreateRoom(roomName, roomOptions);

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
    #endregion
}
