using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraPos
{
    public Transform s_Pos;
    public Transform w_Pos;
    public Transform n_Pos;
    public Transform e_Pos;
}

public class Parabola : MonoBehaviour
{
    public static Parabola Instance {  get; private set; }

    public Transform centerOfRotation;
    public CameraPos camPos;
    public float elapsedTime = 0.0f;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        transform.LookAt(centerOfRotation.position);

        if (Managers.Game.isS_posStay)
        {
            elapsedTime = 0.0f;
            transform.position = Vector3.Slerp(transform.position, camPos.s_Pos.position, 0.03f);
        }
        if (Managers.Game.isW_posStay)
        {
            elapsedTime = 0.0f;
            transform.position = Vector3.Slerp(transform.position, camPos.w_Pos.position, 0.03f);
        }
        if (Managers.Game.isN_posStay)
        {
            elapsedTime = 0.0f;
            transform.position = Vector3.Slerp(transform.position, camPos.n_Pos.position, 0.03f);
        }
        if (Managers.Game.isE_posStay)
        {
            elapsedTime = 0.0f;
            transform.position = Vector3.Slerp(transform.position, camPos.e_Pos.position, 0.03f);
        }
    }
}
