using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variable

    [Header("Component")]
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private CameraRotate cameraRotate;

    [Header("Animation Value")]
    [SerializeField] private int idleCount = 0;
    [SerializeField] private float inputValue = 0;

    [Header("Player Stat")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    #endregion

    #region Unity_Function
    private void Start()
    {
        cameraRotate = Camera.main.GetComponent<CameraRotate>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _Move();
        _Anim();
        _Jump();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) curJumpCount = maxJumpCount;
    }
    #endregion

    #region Function
    private void _Move()
    {
        float input = Input.GetAxis("Horizontal") * moveSpeed;
        inputValue = input;

        if (input < 0) spriteRenderer.flipX = true;
        else if (input > 0) spriteRenderer.flipX = false;

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

    private void _Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curJumpCount > 0)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            curJumpCount--;
        }
    }

    private void _Anim()
    {
        if (inputValue != 0) animator.SetBool("IsWalk", true);
        else animator.SetBool("IsWalk", false);

        if (idleCount == 10)
        {
            animator.SetTrigger("Sigh");
            idleCount = 0;
        }
    }

    private void _IdleCount() => idleCount++;
    #endregion
}
