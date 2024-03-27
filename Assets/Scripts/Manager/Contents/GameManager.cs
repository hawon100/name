using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraDirection
{
    S, W, N, E
}

public class GameManager : MonoBehaviour
{
    #region Variable
    public static GameManager instance;

    public GameObject playerObject;

    public CameraDirection cameraDirection;
    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    #endregion
}
