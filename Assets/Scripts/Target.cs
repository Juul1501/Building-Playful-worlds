using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public Rigidbody rb;
    public void Damage(float damage,GameObject sender)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        if(rb != null)
        {
            Vector3 pos = sender.transform.position;
            Vector3 dir = transform.position - pos;
            dir = dir.normalized;
            rb.AddForce(dir * damage,ForceMode.Impulse);
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
