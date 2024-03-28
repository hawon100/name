using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum CameraDirection
{
    S, W, N, E
}

public class GameManager : MonoBehaviour
{
    #region Variable
    public static GameManager instance;

    public GameObject playerObject;
    public PlayerMove currentPlayer;

    public CameraDirection cameraDirection;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerObject = PhotonNetwork.Instantiate("Player", new Vector3(0, 1, -4.5f), Quaternion.identity);
        currentPlayer = playerObject.GetComponent<PlayerMove>();
    }

    #endregion
}
