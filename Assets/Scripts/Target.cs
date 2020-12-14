using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Target : MonoBehaviourPunCallbacks, IDamageable<RaycastHit>
{
    public float health = 50f;
    public Rigidbody rb;
    public float force;
    public GameObject explosion;
    public void Damage(float damage,RaycastHit hit)
    {
        health -= damage;
        if(health <= 0)
        {
            Die(hit);
        }
        if(rb != null)
        {
            rb.AddForce(-hit.normal * damage*force,ForceMode.Force);
        }
    }

    void Die(RaycastHit hit)
    {
        var go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);   
    }
}
