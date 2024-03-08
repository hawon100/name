using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    #region # Variable
    [Header("Transform")]
    [SerializeField] private Transform roomListContent;

    [Header("Rect Transform")]
    [SerializeField] private RectTransform UIPreset;

    [Header("TextMeshPro UGUI")]
    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private TextMeshProUGUI stateText;

    [Header("TMP_InputField")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Button")]
    [SerializeField] private Button nameEnterBtn;
    [SerializeField] private Button roomCreateBtn;
    [SerializeField] private Button roomJoindBtn;

    [Header("Prefabs")]
    [SerializeField] private GameObject roomListPrefab;

    [Header("List")]
    [SerializeField] private List<GameObject> roomListObjects = new List<GameObject>();
    #endregion

    #region # Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        nameEnterBtn.onClick.AddListener(() =>
        {
            NetworkManager.SetNickName(nameInputField.text);
            UIPreset.DOLocalMoveX(-1920, 0.5f).SetEase(Ease.OutQuad);
        });

        roomCreateBtn.onClick.AddListener(() => NetworkManager.CreateRoom(roomNameInputField.text, new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }));
        roomJoindBtn.onClick.AddListener(() => NetworkManager.JoinRoom(roomNameInputField.text));
    }
    private void Update()
    {

    }
    #endregion

    #region # Function
    private void _SetState(string state) => stateText.text = state;
    public static void SetState(string state) => instance._SetState(state);

    private void _SetPlayerList(List<string> playerList)
    {
        playerListText.text = "Player List";

        foreach (var item in playerList) playerListText.text += ("\n" + item);
    }
    public static void SetPlayerList(List<string> playerList) => instance._SetPlayerList(playerList);

    private void _RoomListUpdate(List<Data> roomDatas)
    {
        foreach (var data in roomDatas)
        {
            RoomData roomData = Instantiate(roomListPrefab, roomListContent).GetComponent<RoomData>();

            roomData.data = data;

            roomListObjects.Add(roomData.gameObject);
        }

        foreach (var item in roomListObjects) 
        { 
            RoomData roomData = item.GetComponent<RoomData>();
            roomData.UISetting();
        }
    }
    public static void RoomListUpdate(List<Data> roomDatas) => instance._RoomListUpdate(roomDatas);

    private void _RoomListReset()
    {
        foreach (var item in roomListObjects) Destroy(item);
        roomListObjects.Clear();
    }
    public static void RoomListReset() => instance._RoomListReset();

    private List<Data> _RoomInfoToRoomData(List<Photon.Realtime.RoomInfo> roomInfos)
    {
        List<Data> roomDataList = new List<Data>();

        foreach (var roomInfo in roomInfos)
        {
            Data roomData = new Data()
            {
                Name = roomInfo.Name,
                IsVisible = roomInfo.IsVisible,
                MaxPlayers = roomInfo.MaxPlayers,
                PlayerCount = roomInfo.PlayerCount,
            };

            roomDataList.Add(roomData);
        }

        return roomDataList;
    }
    public static List<Data> RoomInfoToRoomData(List<Photon.Realtime.RoomInfo> roomInfos) => instance._RoomInfoToRoomData(roomInfos);
    #endregion
}
