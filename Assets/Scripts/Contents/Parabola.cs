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

    [SerializeField] private Transform centerOfRotation;
    [SerializeField] private CameraPos camPos;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        transform.LookAt(centerOfRotation.position);

    }

    void PlayerPosClear()
    {
    }

    void DirectionMove(Define.PlayerState state)
    {
        switch (state)
        {
        }
    }

    void DirectionRot(Define.PlayerState state)
    {
        switch (state)
        {
        }
    }
}
