using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public abstract class Target : MonoBehaviourPunCallbacks
{
    public float health = 50f;

    [PunRPC]
    public void TakeDamage(float damage, Vector3 normal)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
