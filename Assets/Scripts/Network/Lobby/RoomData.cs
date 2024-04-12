using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameType = Photon.Realtime.GameType;

[System.Serializable]
public class Data
{
    public string Name;

    public int MaxPlayers;
    public int PlayerCount;

    public bool IsVisible;

    public GameType CurGameType = GameType.Solo;
}

public class RoomData : MonoBehaviour
{
    #region Variable
    [SerializeField] private Data currentData;
    public Data CurrentData
    {
        get { return currentData; }
        set
        {
            currentData = value;
            playerCount = currentData.PlayerCount;
            if (currentData.PlayerCount <= 0) DestroyRoom();
        }
    }
    private int playerCount;

    [SerializeField] private Button button;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI gameTypeText;

    #endregion

    #region Unity_Function
    private void Start()
    {
        button.onClick.AddListener(() => Join());

    }
    #endregion

    #region Function
    private void _UISetting()
    {
        roomNameText.text = "방 이름 : " + currentData.Name;
        playerCountText.text = "인원 : " + currentData.PlayerCount + "/" + currentData.MaxPlayers;
        gameTypeText.text = CurrentData.CurGameType == GameType.Solo ? "개인전" : "팀전";
    }
    public void UISetting() => _UISetting();

    public void Join()
    {
        if (currentData.PlayerCount < currentData.MaxPlayers) NetworkManager.JoinRoom(currentData.Name);
    }
    public void DestroyRoom() => Destroy(gameObject);
    #endregion
}
