using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;
using HashTable = ExitGames.Client.Photon.Hashtable;

public enum CameraDirection
{
    S, W, N, E
}

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Variable
    public static GameManager instance;

    public GameType gameType;

    public GameObject playerObject;
    public PlayerMove currentPlayer;

    public CameraDirection cameraDirection;

    public List<PlayerMove> players = new List<PlayerMove>();


    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        float x = PhotonNetwork.IsMasterClient ? -3 : 3;
        playerObject = PhotonNetwork.Instantiate("New Player", new Vector3(x, 1, -7f), Quaternion.identity);
        currentPlayer = playerObject.GetComponent<PlayerMove>();

        gameType = (GameType)NetworkManager.CurrentIntRoomGetCP("GameType");

        _SetPlayerList();
    }
    #endregion

    #region  Function
    private void _SetPlayerList()
    {
        StartCoroutine(Dealy());
        IEnumerator Dealy()
        {
            yield return new WaitForSeconds(1);
            players = players.OrderBy(x => x.PVID).ToList();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {

    }

    #endregion
}
