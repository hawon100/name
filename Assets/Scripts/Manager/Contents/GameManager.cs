using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraDirection
{
    S,W,N,E
}

public class GameManager
{
    public bool isS_posStay = false;
    public bool isW_posStay = false;
    public bool isN_posStay = false;
    public bool isE_posStay = false;
    
    public CameraDirection cameraDirection;
}
