using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public bool isLobby;

    #region # Variable
    [Header("Transform")]
    [SerializeField] private Transform roomListContent;

    [Header("Rect Transform")]
    [SerializeField] private RectTransform UIPreset;

    [Header("Image")]
    [SerializeField] private Image PlayerColorPreviewImage;
    [SerializeField] private List<Image> playerImages = new List<Image>();

    [Header("TextMeshPro UGUI")]
    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI registerFailText;
    [SerializeField] private TextMeshProUGUI idText;

    [Header("TMP_InputField")]
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_InputField loginIdInputField;
    [SerializeField] private TMP_InputField loginPassInputField;
    [SerializeField] private TMP_InputField registerIdInputField;
    [SerializeField] private TMP_InputField registerPassInputField;

    [Header("Button")]
    [SerializeField] private Button roomCreateBtn;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button loginEnterBtn;
    [SerializeField] private Button registerBtn;
    [SerializeField] private Button registerEngerBtn;
    [SerializeField] private Button roomExitBtn;
    [SerializeField] private Button PlayerColorRedBtn;
    [SerializeField] private Button PlayerColorBlueBtn;
    [SerializeField] private Button PlayerColorGreenBtn;
    [SerializeField] private Button PlayerColorYellowBtn;
    [SerializeField] private Button startBtn;

    [Header("Prefabs")]
    [SerializeField] private GameObject roomListPrefab;

    [Header("Other")]
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

        PhotonNetwork.AutomaticallySyncScene = true;


        loginBtn.onClick.AddListener(() => { UIPresetMove(0, 0, 0.5f); });
        loginEnterBtn.onClick.AddListener(() =>
        {
            GoogleSheetManager.Login(loginIdInputField.text, loginPassInputField.text);
            NetworkManager.SetNickName(loginIdInputField.text);
            idText.text = loginIdInputField.text + "님 환영합니다!";
        });

        registerBtn.onClick.AddListener(() => { UIPresetMove(0, 1080, 0.5f); });
        registerEngerBtn.onClick.AddListener(() => { GoogleSheetManager.Register(registerIdInputField.text, registerPassInputField.text); });

        roomCreateBtn.onClick.AddListener(() =>
        {
            NetworkManager.CreateRoom(roomNameInputField.text, new Photon.Realtime.RoomOptions() { MaxPlayers = 4 });

        });
        roomExitBtn.onClick.AddListener(() => NetworkManager.ExitRoom());

        startBtn.onClick.AddListener(() =>  PhotonNetwork.LoadLevel("InGame"));

        PlayerColorRedBtn.onClick.AddListener(() => { PlayerColorPreviewImage.color = PlayerColorRedBtn.GetComponent<Image>().color; });
        PlayerColorBlueBtn.onClick.AddListener(() => { PlayerColorPreviewImage.color = PlayerColorBlueBtn.GetComponent<Image>().color; });
        PlayerColorGreenBtn.onClick.AddListener(() => { PlayerColorPreviewImage.color = PlayerColorGreenBtn.GetComponent<Image>().color; });
        PlayerColorYellowBtn.onClick.AddListener(() => { PlayerColorPreviewImage.color = PlayerColorYellowBtn.GetComponent<Image>().color; });

    }
    private void Update()
    {

    }
    #endregion

    #region # Function
    private void _UIPresetMove(float x, float y, float time) => UIPreset.DOLocalMove(new Vector3(x, y), time).SetEase(Ease.OutQuad);
    public static void UIPresetMove(float x, float y, float time) => instance._UIPresetMove(x, y, time);

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
        RoomListReset();

        foreach (var data in roomDatas)
        {
            RoomData roomData = Instantiate(roomListPrefab, roomListContent).GetComponent<RoomData>();

            roomData.CurrentData = data;

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

    private IEnumerator _SetVisibleUI(GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    public static void RegisterFailVisible(string text, float time)
    {
        instance.registerFailText.text = text;

        var obj = instance.registerFailText.gameObject;

        instance.StopAllCoroutines();
        instance.StartCoroutine(instance._SetVisibleUI(obj, time));
    }

    private void _LoginComplete()
    {
        UIPresetMove(-1920, 0, 0.5f);
    }
    public static void LoginComplete() => instance._LoginComplete();

    private void _SetMasterClient()
    {
        startBtn.gameObject.SetActive(true);
    }
    public static void SetMasterClient() => instance._SetMasterClient();
    #endregion
}
