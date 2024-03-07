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
    #endregion

    #region # Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }
    private void Start()
    {
        nameEnterBtn.onClick.AddListener(() =>
        {
            NetworkManager.SetNickName(nameInputField.text);
            //UIPreset.DOLocalMoveX(-1920, 0.5f).SetEase(Ease.OutQuad);
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
    #endregion
}
