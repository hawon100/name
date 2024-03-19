using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Data
{
    public string Name;

    public int MaxPlayers;
    public int PlayerCount;

    public bool IsVisible;
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

        Debug.Log("방 이름 : " + currentData.Name);
        Debug.Log("인원 : " + currentData.PlayerCount + "/" + currentData.MaxPlayers);
    }
    public void UISetting() => _UISetting();

    public void Join()
    {
        if (currentData.PlayerCount < currentData.MaxPlayers) NetworkManager.JoinRoom(currentData.Name);
        else Debug.Log("방에 최대 인원이 채워져있습니다.");
    }
    public void DestroyRoom() => Destroy(gameObject);
    #endregion
}
