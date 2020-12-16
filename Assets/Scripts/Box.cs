using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Box : Target, IDamageable<RaycastHit>, IPunObservable
{

    protected override void Die()
    {
        if (GetComponent<PhotonView>() == null)
        {
            gameObject.AddComponent<PhotonView>();
        }
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
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
}
