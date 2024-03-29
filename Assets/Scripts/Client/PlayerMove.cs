using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Photon.Pun.Demo.Cockpit;

public class PlayerMove : MonoBehaviour, IPunObservable
{
    #region Variable

    [Header("Component")]
    private PhotonView pv;
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private CameraRotate cameraRotate;


    [SerializeField] private Transform rangedAttackPos;

    [SerializeField] private TextMeshPro nameText;

    [Header("Prefab")]
    [SerializeField] private GameObject fireBallPrefab;

    [Header("Animation Value")]
    [SerializeField] private int idleCount = 0;
    [SerializeField] private float inputValue = 0;
    [SerializeField] private bool isGround;

    [Header("Player Stat")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private bool canMeleeAttack = true;

    [Header("Player Info")]
    public string playerName = "";

    [Header ("Attack Object")]
    [SerializeField] private MeleeFire meleeFire;
    #endregion

    #region Unity_Function
    private void Start()
    {
        gameObject.layer = 11;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        cameraRotate = Camera.main.GetComponent<CameraRotate>();

        nameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nameText.color = pv.IsMine ? Color.green : Color.red;
    }

    private void Update()
    {
        _Move();
        _Anim();
        _Jump();

        _MeleeAttack();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
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
            if (inputValue != 0 && isGround) animator.SetBool("IsWalk", true);
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

    private void _MeleeAttack()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F) && canMeleeAttack)
            {
                CountMeleeAttack();

                animator.SetTrigger("MeleeAttack");
            }
        }
    }
    
    public void MeleeAttack() => pv.RPC("_MeleeAttackRPC", RpcTarget.All);
    [PunRPC]
    private void _MeleeAttackRPC()
    {
        meleeFire.SetParticleSystem(spriteRenderer.flipX);
        meleeFire.gameObject.SetActive(true);
    }

    private IEnumerator _CountMeleeAttack()
    {
        canMeleeAttack = false;
        yield return new WaitForSeconds(2);
        canMeleeAttack = true;
    }
    public void CountMeleeAttack() => StartCoroutine(_CountMeleeAttack());

    [PunRPC]
    private void _Flip(float input)
    {
        if (input < 0) spriteRenderer.flipX = true;
        else if (input > 0) spriteRenderer.flipX = false;
    }

    private void _IdleCount() => idleCount++;
    #endregion
}
