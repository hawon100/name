using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variable

    [Header("Component")]
    private PhotonView pv;
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private CameraRotate cameraRotate;

    [Header("Animation Value")]
    [SerializeField] private int idleCount = 0;
    [SerializeField] private float inputValue = 0;
    [SerializeField] private bool isGround;

    [Header("Player Stat")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    #endregion

    #region Unity_Function
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        cameraRotate = Camera.main.GetComponent<CameraRotate>();
    }

    private void Update()
    {
        _Move();
        _Anim();
        _Jump();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            curJumpCount = maxJumpCount;
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) isGround = false;
    }
    #endregion

    #region Function
    private void _Move()
    {
        if (pv.IsMine)
        {
            float input = Input.GetAxis("Horizontal") * moveSpeed;
            inputValue = input;

            pv.RPC("_Flip", RpcTarget.AllBuffered, input);

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
    }

    private void _Jump()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && curJumpCount > 0)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                animator.SetTrigger("Jump");
                curJumpCount--;
            }
        }
    }

    private void _Anim()
    {
        if (pv.IsMine)
        {
            if (inputValue != 0) animator.SetBool("IsWalk", true);
            else animator.SetBool("IsWalk", false);

            if (idleCount == 10)
            {
                animator.SetTrigger("Sigh");
                idleCount = 0;
            }

            if (!isGround) animator.SetBool("IsFall", true);
            else animator.SetBool("IsFall", false);
        }
    }

    [PunRPC]
    private void _Flip(float input)
    {
         if (input < 0) spriteRenderer.flipX = true;
            else if (input > 0) spriteRenderer.flipX = false;
    }

    private void _IdleCount() => idleCount++;
    #endregion
}
