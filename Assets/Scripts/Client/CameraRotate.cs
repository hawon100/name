using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    E,W,S,N
}

public class CameraRotate : MonoBehaviour
{
    public Direction cameraDirection { get; private set; }
}
