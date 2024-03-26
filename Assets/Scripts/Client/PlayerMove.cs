using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variable

    private Rigidbody rb;
    private CameraRotate cameraRotate;

    [SerializeField] private float moveSpeed;
    #endregion

    #region Unity_Function
    private void Start()
    {
        cameraRotate = Camera.main.GetComponent<CameraRotate>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _Move();
    }
    #endregion

    #region Function
    private void _Move()
    {
        float input = Input.GetAxis("Horizontal") * moveSpeed;
        var cameraDirection = cameraRotate.cameraDirection;

        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;

        float veloX = rb.velocity.x;
        float veloY = rb.velocity.y;
        float veloZ = rb.velocity.z;

        switch (cameraDirection)
        {
            case CameraDirection.S: rb.velocity = new Vector3(input, veloY, 0); break;
            case CameraDirection.W: rb.velocity = new Vector3(0, veloY, -input); break;
            case CameraDirection.N: rb.velocity = new Vector3(-input, veloY, 0); break;
            case CameraDirection.E: rb.velocity = new Vector3(0, veloY, input); break;
        }
    }
    #endregion
}
