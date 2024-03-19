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

        switch (Managers.Game.state)
        {
            case Define.PlayerState.S: PlayerPosClear(); DirectionMove(Define.PlayerState.S); DirectionRot(Define.PlayerState.S); break;
            case Define.PlayerState.W: PlayerPosClear(); DirectionMove(Define.PlayerState.W); DirectionRot(Define.PlayerState.W); break;
            case Define.PlayerState.N: PlayerPosClear(); DirectionMove(Define.PlayerState.N); DirectionRot(Define.PlayerState.N); break;
            case Define.PlayerState.E: PlayerPosClear(); DirectionMove(Define.PlayerState.E); DirectionRot(Define.PlayerState.E); break;
        }
    }

    void PlayerPosClear()
    {
        PlayerController.Instance.inputVec = Vector3.zero;
    }

    void DirectionMove(Define.PlayerState state)
    {
        switch (state)
        {
            case Define.PlayerState.S: PlayerController.Instance.inputVec.x = Input.GetAxisRaw("Horizontal_2"); break;
            case Define.PlayerState.W: PlayerController.Instance.inputVec.z = Input.GetAxisRaw("Horizontal"); break;
            case Define.PlayerState.N: PlayerController.Instance.inputVec.x = Input.GetAxisRaw("Horizontal"); break;
            case Define.PlayerState.E: PlayerController.Instance.inputVec.z = Input.GetAxisRaw("Horizontal_2"); break;
        }
    }

    void DirectionRot(Define.PlayerState state)
    {
        switch (state)
        {
            case Define.PlayerState.S: transform.position = Vector3.Slerp(transform.position, camPos.s_Pos.position, 0.03f); break;
            case Define.PlayerState.W: transform.position = Vector3.Slerp(transform.position, camPos.w_Pos.position, 0.03f); break;
            case Define.PlayerState.N: transform.position = Vector3.Slerp(transform.position, camPos.n_Pos.position, 0.03f); break;
            case Define.PlayerState.E: transform.position = Vector3.Slerp(transform.position, camPos.e_Pos.position, 0.03f); break;
        }
    }
}
