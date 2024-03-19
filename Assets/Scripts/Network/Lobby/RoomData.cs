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
        roomNameText.text = "�� �̸� : " + currentData.Name;
        playerCountText.text = "�ο� : " + currentData.PlayerCount + "/" + currentData.MaxPlayers;

        Debug.Log("�� �̸� : " + currentData.Name);
        Debug.Log("�ο� : " + currentData.PlayerCount + "/" + currentData.MaxPlayers);
    }
    public void UISetting() => _UISetting();

    public void Join()
    {
        if (currentData.PlayerCount < currentData.MaxPlayers) NetworkManager.JoinRoom(currentData.Name);
        else Debug.Log("�濡 �ִ� �ο��� ä�����ֽ��ϴ�.");
    }
    public void DestroyRoom() => Destroy(gameObject);
    #endregion
}
