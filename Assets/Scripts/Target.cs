using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Target : MonoBehaviourPunCallbacks, IDamageable<RaycastHit>,IPunObservable
{
    public float health = 50f;
    public Rigidbody rb;
    public float force;
    public GameObject explosion;

    void Die()
    {
        if(GetComponent<PhotonView>() == null)
        {
            gameObject.AddComponent<PhotonView>(); 
        }
        var go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Damage(float damage, RaycastHit hit)
    {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, hit.normal);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, Vector3 normal)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.DestroySceneObject(this.photonView);
            Die();
        }
        if (rb != null)
        {
            rb.AddForce(-normal * damage * force, ForceMode.Force);
        }
    }
}
