using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variable
    public Direction playerDirection = Direction.E;

    private Rigidbody rb;
    #endregion

    #region Unity_Function
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerDirection = Direction.E;
    }

    private void Update()
    {
        _Move();
    }
    #endregion

    #region Function
    private void _Move()
    {
        float x = Input.GetAxis("Horizontal");

        switch (playerDirection)
        {
            case Direction.E: rb.velocity = new Vector3(5 * x, rb.velocity.y, 0); break;
            case Direction.W: rb.velocity = new Vector3(5 * x, rb.velocity.y, 0); break;
            case Direction.S: rb.velocity = new Vector3(0, rb.velocity.y, 5 * x); break;
            case Direction.N: rb.velocity = new Vector3(0, rb.velocity.y, 5 * x); break;
        }
    }
    #endregion
}
