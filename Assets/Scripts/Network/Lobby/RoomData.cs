using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct Data
{
    public string Name;

    public int MaxPlayers;
    public int PlayerCount;

    public bool IsVisible;
}
public class RoomData : MonoBehaviour 
{
    #region Variable
    public Data data;

    [SerializeField] private Button button;

    [Header ("Text")]
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
        roomNameText.text = "방 이름 : " + data.Name;
        playerCountText.text = "인원 : " + data.PlayerCount + "/" + data.MaxPlayers;

        Debug.Log("방 이름 : " + data.Name);
        Debug.Log("인원 : " + data.PlayerCount + "/" + data.MaxPlayers);
    }
    public void UISetting() => _UISetting();

    public void Join() => NetworkManager.JoinRoom(data.Name);
    #endregion
}
