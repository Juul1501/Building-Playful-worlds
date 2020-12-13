﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable<RaycastHit>
{
    public float health = 50f;
    public Rigidbody rb;
    public float force;
    public void Damage(float damage,RaycastHit hit)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        if(rb != null)
        {
            rb.AddForce(-hit.normal * damage*force,ForceMode.Force);
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
