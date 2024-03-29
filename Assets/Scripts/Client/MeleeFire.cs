using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFire : MonoBehaviour
{
    public bool IsFlip;

    public float Damage;

    private ParticleSystem ps;
    private ParticleSystemRenderer psr;


    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psr = GetComponent<ParticleSystemRenderer>();

        psr.pivot = IsFlip ? new Vector3(-0.5f, 0, 0) : new Vector3(0.5f, 0, 0);
        psr.flip = IsFlip ? new Vector3(1, 0, 0) : new Vector3(0, 0, 0);
    }

    private void OnDestroy()
    {
        GameManager.instance.currentPlayer.canMeleeAttack = true;
    }
}