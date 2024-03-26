using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RoomSeat : MonoBehaviour
{
    private bool isSeat = false;
    public bool IsSeat
    {
        get { return isSeat; }
        set
        {
            isSeat = value;
            if (!isSeat) NickName = "";
        }
    }

    public string NickName = "";

    
}
