using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.Events;
public abstract class Target : MonoBehaviourPunCallbacks
{
    public UnityEvent OnTakeDamage;
    public float health = 50f;

    private void Awake()
    {
        OnTakeDamage = new UnityEvent();
    }
    [PunRPC]
    public void TakeDamage(float damage, Vector3 normal)
    {
        OnTakeDamage.Invoke();
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
