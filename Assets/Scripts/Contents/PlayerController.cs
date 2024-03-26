using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerPos
{
    public Transform s_Pos;
    public Transform w_Pos;
    public Transform n_Pos;
    public Transform e_Pos;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigid;
    public PlayerPos pos;
    public float moveSpeed;
    public Vector3 inputVec;

    private void Start()
    {
        _rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 0) inputVec.x = Input.GetAxisRaw("Horizontal");
        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 90) inputVec.z = Input.GetAxisRaw("Horizontal_2");
        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 180) inputVec.x = Input.GetAxisRaw("Horizontal_2");
        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 270) inputVec.z = Input.GetAxisRaw("Horizontal");
        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 360) inputVec.x = Input.GetAxisRaw("Horizontal");

        Vector3 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;

        Vector3 newPosition = _rigid.position + nextVec;
        newPosition.x = Mathf.Clamp(newPosition.x, -4.0f, 4.0f);
        newPosition.z = Mathf.Clamp(newPosition.z, -4.0f, 4.0f);

        _rigid.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("S_Pos"))
        {
            Managers.Game.cameraDirection = CameraDirection.S;
            //transform.position = pos.s_Pos.position;
        }
        if (other.CompareTag("W_Pos"))
        {
            Managers.Game.cameraDirection = CameraDirection.W;
            //transform.position = pos.w_Pos.position;
        }
        if (other.CompareTag("N_Pos"))
        {
            Managers.Game.cameraDirection = CameraDirection.N;
            //transform.position = pos.n_Pos.position;
        }
        if (other.CompareTag("E_Pos"))
        {
            Managers.Game.cameraDirection = CameraDirection.E;
            //transform.position = pos.e_Pos.position;
        }
    }
}
