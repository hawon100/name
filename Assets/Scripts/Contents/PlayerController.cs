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
    public static PlayerController Instance { get; set; }

    private Rigidbody _rigid;
    [SerializeField] private PlayerPos pos;
    [SerializeField] private float moveSpeed;
    public Vector3 inputVec;
    [SerializeField] private float jumpPower;
    [SerializeField] private Animator anim;
    [SerializeField] private bool _isGround = false;

    private void Start()
    { 
        Instance = this;
        _rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector3 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;

        Vector3 newPosition = _rigid.position + nextVec;

        if (_isGround)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, pos.e_Pos.position.x, pos.w_Pos.position.x);
            newPosition.z = Mathf.Clamp(newPosition.z, pos.n_Pos.position.z, pos.s_Pos.position.z);
        }

        _rigid.MovePosition(newPosition);
    }

    private void Jump()
    {
        // Á¡ÇÁ
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            _rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            _isGround = false;
        }

        // ÂøÁö ÇÃ·§Æû
        if (_rigid.velocity.y < 0)
        {
            Debug.DrawRay(_rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit rayHit;
            if (Physics.Raycast(_rigid.position, Vector3.down, out rayHit, 1, LayerMask.GetMask("Platform")))
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    _isGround = true;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("S_Pos"))
        {
            Managers.Game.state = Define.PlayerState.S;
        }
        if (other.CompareTag("W_Pos"))
        {
            Managers.Game.state = Define.PlayerState.W;
        }
        if (other.CompareTag("N_Pos"))
        {
            Managers.Game.state = Define.PlayerState.N;
        }
        if (other.CompareTag("E_Pos"))
        {
            Managers.Game.state = Define.PlayerState.E;
        }
    }
}
