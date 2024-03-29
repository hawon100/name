using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody rb;
    private PhotonView pv;

    [Header("Stat")]
    [SerializeField] private int bounceCount = 5;
    [SerializeField] private float bouncePower = 1;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        if (pv.IsMine) gameObject.layer = 12;

        rb.AddForce(new Vector3(transform.localPosition.x * 5, 0, 0));

        Destroy(gameObject, 5);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && bounceCount > 0)
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 otherPos = other.gameObject.transform.position;

            float x = myPos.x - otherPos.x > 0 ? 1 : -1;
            float y = myPos.y - otherPos.y > 0 ? 1 : -1;

            bounceCount--;

            rb.AddForce(new Vector3(x, y) * bouncePower, ForceMode.Impulse);
        }
        else if (other.gameObject.CompareTag("Ground")) if (bounceCount == 0) Destroy(gameObject);
    }
}
