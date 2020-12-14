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
            Die();
        }
        if(rb != null)
        {
            rb.AddForce(-hit.normal * damage*force,ForceMode.Force);
        }
    }

    void Die()
    {
        //var go = Instantiate(explosion, transform.position, transform.rotation);
        var go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
        GameManager.instance.destroy(go, 2f);
        GameManager.instance.destroy(this.gameObject,0f);
    }
}
