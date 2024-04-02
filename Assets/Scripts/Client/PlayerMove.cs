using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun.Demo.Cockpit;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerMove : MonoBehaviour, IPunObservable
{
    #region Variable

    private HashTable ht = new HashTable();

    [Header("Component")]
    private PhotonView pv;
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private CameraRotate cameraRotate;

    public int PVID
    {
        get => pv.ViewID;
    }


    [Header("Animation Value")]
    [SerializeField] private int idleCount = 0;
    [SerializeField] private float inputValue = 0;
    [SerializeField] private bool isGround;

    [Header("UI")]
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private Image HPBar;

    [Header("Player Stat")]
    [SerializeField] private float hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;

    public bool isGuard = false;

    [Header("Melee Attack Stat")]
    private WaitForSeconds meleeAttackCount;
    public float meleeAttackTime = 1;
    [SerializeField] private bool canMeleeAttack = true;

    [Header("Player Info")]
    public string playerName = "";

    [Header("Attack Object")]
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

        meleeAttackCount = new WaitForSeconds(meleeAttackTime);

        nameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nameText.color = pv.IsMine ? Color.green : Color.red;

        HPBar.color = pv.IsMine ? Color.green : Color.red;

        hp = 100;
        ht["HP"] = hp;
        ht["ID"] = pv.ViewID;
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);

        GameManager.instance.players.Add(this);
    }

    private void Update()
    {
        _Move();
        _Anim();
        _Jump();
        _Guard();

        _MeleeAttack();
        _SetHPBar();
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
        yield return meleeAttackCount;
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

    private void _SetHPBar()
    {
        HPBar.fillAmount = hp / 100;
    }

    private void _Guard()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.C)) pv.RPC("Guard", RpcTarget.All, true);
            //if (Input.GetKeyUp(KeyCode.C)) pv.RPC("Guard", RpcTarget.All, false);
        }
    }

    [PunRPC]
    private void Guard(bool value) => isGuard = value;

    public void Hit(float damage)
    {
        hp -= damage;
        ht["HP"] = hp;
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    #endregion
}
