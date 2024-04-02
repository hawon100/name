using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MeleeFire : MonoBehaviour
{
    public float Damage = 5;

    public PlayerMove usePlayer;

    private PhotonView pv;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private ParticleSystemRenderer psr;

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        ps.Play();
    }
    
    public void SetParticleSystem(bool value)
    {
        psr.pivot = value ? new Vector3(-0.5f, 0, 0) : new Vector3(0.5f, 0, 0);
        psr.flip = value ? new Vector3(1, 0, 0) : new Vector3(0, 0, 0);

        boxCollider.center = value ? new Vector3(-0.5f, 0, 0) : new Vector3(0.5f, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();

            if (player != usePlayer && !player.isGuard) player.Hit(Damage);
        }
    }
}