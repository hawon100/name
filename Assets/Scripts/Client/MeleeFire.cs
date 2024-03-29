using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MeleeFire : MonoBehaviour
{
    public float Damage;

    public PlayerMove usePlayer;

    private PhotonView pv;

    private ParticleSystem ps;
    private ParticleSystemRenderer psr;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psr = GetComponent<ParticleSystemRenderer>();
        pv = GetComponent<PhotonView>();

        ps.Play();
    }

    public void SetParticleSystem(bool value)
    {
        psr.pivot = value ? new Vector3(-0.5f, 0, 0) : new Vector3(0.5f, 0, 0);
        psr.flip = value ? new Vector3(1, 0, 0) : new Vector3(0, 0, 0);
    }

}