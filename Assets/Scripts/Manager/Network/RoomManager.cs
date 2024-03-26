using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    #endregion
}
